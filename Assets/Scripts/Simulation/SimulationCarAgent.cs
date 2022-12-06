using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class SimulationCarAgent : AbstractCarAgent
{
    //Nowe pola
    private Toggle m_autoRestartToggle;
    private Toggle m_otherCarsToggle;
    //Koniec nowych pól
    //Is Empty - sprawdziæ 

    [Tooltip("Kara za pierwsze uderzenie. Pierwszy wyraz ci¹gu geometrycznego o sumie 0.5.")]
    public float startingCollisionPenalty = 1 / 4f; // 1/3, 1/10
    private float currentCollisionPenalty;
    [Tooltip("Liczba przez któr¹ przemna¿amy karê za uderzenie za ka¿de kolejne uderzenie. Iloraz ci¹gu geometrycznego o sumie 0.5.")]
    public float collisionPenaltyMultiplier = 1 / 2f; // 1/3, 4/5
    public float collisionPenalty = 0.2f;

    private float existencePenalty;

    public float distanceRewardMultiplier = 0.2f;
    public float parkingRewardMultiplier = 0.4f;
    public float enteredBoundsFirstTimeReward = 0.05f;
    public float enteredTargetFirstTimeReward = 0.05f;
    private float targetRewardMultiplier;
    private float currentDistanceReward = 0f;
    private float currentTargetReward = 0f;
    private float distanceRewardPerStep;
    private float targetRewardPerStep;
    private float currentNegativeDistanceReward;

    public int framesToPark = 100;
    public List<WheelElements> wheelData;
    public Transform wheelSteer;
    //private List<List<ParkingSlot>> parkingSlots;
    private List<ParkingSlot> parkingSlots;
    public GameObject parking;
    private int currentSlotNumber = 0;

    //public BoxCollider targetCollider;

    public float maxTorque;
    public float maxSteerAngle;

    private Rigidbody rigidBody;
    public Transform massCenter;

    public BoxCollider carCollider;

    public string parkingAreaName = "parkingArea";
    public string baseParkingChildName = "parkingSlot";
    public string targetName = "Target";
    public string boundsName = "slotBounds";
    public string staticCarName = "static-car";

    public float maxRespawnZ = 10f;
    public float minRespawnZ = -10f;

    private int enteredBoundsCount = 0;
    private bool enteredTarget = false;
    private bool enteredBoundsFirstTime = false;
    private bool enteredTargetFirstTime = false;

    private int parkingCount = 0;
    private bool firstStep = true;
    private float lastDistance;

    private readonly string tagObstacle = "Obstacle";
    public bool isEmpty = true;

    public override void Initialize()
    {
        //Nowe pola
        m_autoRestartToggle = MapLoadStaticVars.m_autoRestartTransform.GetComponentInChildren<Toggle>();
        m_otherCarsToggle = MapLoadStaticVars.m_otherCarsTransform.GetComponentInChildren<Toggle>();
        //Koniec nowych pól

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (MaxStep);
        distanceRewardPerStep = existencePenalty * distanceRewardMultiplier;
        targetRewardPerStep = existencePenalty * targetRewardMultiplier;
        targetRewardMultiplier = 1f - enteredBoundsFirstTimeReward - enteredTargetFirstTimeReward - distanceRewardMultiplier - parkingRewardMultiplier;

        if (parking == null)
            throw new MissingReferenceException();
        parkingSlots = new List<ParkingSlot>();
        //EDIT
        //List<ParkingSlot> parkingSlotsInParking = new List<ParkingSlot>();
        foreach (Transform parkingSlot in GetParkingSlotsFromParking(parking))
            parkingSlots.Add(new ParkingSlot(parkingSlot.Find(targetName).gameObject, parkingSlot.Find(boundsName).gameObject, parkingSlot.Find(staticCarName).gameObject));
    }

    protected override void RandomOccupy()
    {
        List<ParkingSlot> tempParkingSlots = new List<ParkingSlot>();
        tempParkingSlots.AddRange(parkingSlots);
        tempParkingSlots.RemoveAt(currentSlotNumber);
        int occupySize = Random.Range(tempParkingSlots.Count/2, tempParkingSlots.Count);
        while (occupySize > 0)
        {
            int tempOccupied = Random.Range(0, tempParkingSlots.Count);
            tempParkingSlots[tempOccupied].Occupy();
            tempParkingSlots.RemoveAt(tempOccupied);
            occupySize--;
        }
    }
    IEnumerator waitForSummary()
    {
        yield return new WaitForSeconds(.5f);
    }
    public override void OnEpisodeBegin()
    {
        //
        SimulationSummaryStaticVars.simulationEnd = System.DateTime.Now;
        SimulationSummaryStaticVars.summaryClosed = false;
        SimulationSummaryStaticVars.parkingSuccessful = false;
        /*if (!m_autoRestartToggle.isOn)
        {
            SceneManager.LoadScene("SimulationSummary", LoadSceneMode.Additive);
            
            while(!SimulationSummaryStaticVars.summaryClosed)
            {
                StartCoroutine(waitForSummary());
            }
        }*/
        //
        foreach (ParkingSlot parkingSlot in parkingSlots)
            parkingSlot.Restart();
        /*parking[currentParkingNumber].SetActive(false);
        currentParkingNumber = Random.Range(0, parkingSlots.Count);
        parking[currentParkingNumber].SetActive(true);*/
        currentSlotNumber = Random.Range(0, parkingSlots.Count);
        parkingSlots[currentSlotNumber].Activate();
        //
        if (!isEmpty && m_otherCarsToggle.isOn)
            RandomOccupy();
        //
        TargetDetection targetDetection = parkingSlots[currentSlotNumber].target.GetComponent<TargetDetection>();
        targetDetection.Initialize(this);
        BoundDetection boundDetection = parkingSlots[currentSlotNumber].bounds.GetComponent<BoundDetection>();
        boundDetection.Initialize(this);

        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0.5f, Random.Range(minRespawnZ, maxRespawnZ));
        transform.localEulerAngles = new Vector3(0, 0, 0);

        enteredBoundsCount = 0;
        enteredTarget = false;
        enteredBoundsFirstTime = false;
        enteredTargetFirstTime = false;
        firstStep = true;
        parkingCount = 0;
        currentDistanceReward = 0f;
        currentTargetReward = 0f;
        currentNegativeDistanceReward = 0f;

        currentCollisionPenalty = startingCollisionPenalty;


        foreach (WheelElements element in wheelData)
        {
            if (element.shouldSteer == true)
            {
                element.leftWheel.steerAngle = 0;
                element.rightWheel.steerAngle = 0;
            }
            element.leftWheel.motorTorque = 0;
            element.rightWheel.motorTorque = 0;
            element.leftWheel.brakeTorque = 0;
            element.rightWheel.brakeTorque = 0;

            DoTyres(element.leftWheel);
            DoTyres(element.rightWheel);
        }
        //
        SimulationSummaryStaticVars.simulationStart = System.DateTime.Now;
        //
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.forward));

        sensor.AddObservation((parkingSlots[currentSlotNumber].target.transform.parent.localPosition - transform.localPosition).normalized);
        sensor.AddObservation((parkingSlots[currentSlotNumber].target.transform.parent.localPosition - transform.localPosition).magnitude);

        //sensor.AddObservation(transform.localPosition.normalized);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        sensor.AddObservation(rigidBody.velocity.normalized);
        sensor.AddObservation(rigidBody.velocity.magnitude);
        //sensor.AddObservation(wheelSteer.localEulerAngles.y < 180f ? wheelSteer.localEulerAngles.y : wheelSteer.localEulerAngles.y - 360f);
        sensor.AddObservation(Vector3.Dot(transform.forward, wheelSteer.right));

        /*Debug.Log("Dot: " + Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.right));
        Debug.Log("Dir: " + (parkingSlots[currentSlotNumber].target.transform.parent.localPosition - transform.localPosition).normalized);
        Debug.Log("Pos: " + transform.localPosition.normalized);
        Debug.Log("Forward: " + transform.forward);
        Debug.Log("Right: " + transform.right);
        Debug.Log("Vel: " + rigidBody.velocity.normalized);
        //Debug.Log("Steer: " + (wheelSteer.localEulerAngles.y < 180f ? wheelSteer.localEulerAngles.y : wheelSteer.localEulerAngles.y - 360f));
        Debug.Log("Steer: " + Vector3.Dot(transform.forward, wheelSteer.right));*/
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float speed = actionBuffers.ContinuousActions[0] * maxTorque;
        float steer = actionBuffers.ContinuousActions[1] * maxSteerAngle;
        bool brake = actionBuffers.DiscreteActions[0] == 2;

        foreach (WheelElements element in wheelData)
        {
            if (element.shouldSteer == true)
            {

                element.leftWheel.steerAngle = steer;
                element.rightWheel.steerAngle = steer;

            }
            if (element.addWheelTorque == true)
            {

                element.leftWheel.motorTorque = speed;
                element.rightWheel.motorTorque = speed;

            }
            if (brake == true)
            {

                element.leftWheel.brakeTorque = 4000;
                element.rightWheel.brakeTorque = 4000;

            }

            if (brake == false)
            {

                element.leftWheel.brakeTorque = 0;
                element.rightWheel.brakeTorque = 0;

            }
            DoTyres(element.leftWheel);
            DoTyres(element.rightWheel);
        }


        float currentDistance = Vector3.Distance(transform.localPosition, parkingSlots[currentSlotNumber].target.transform.parent.localPosition);
        if (enteredBoundsCount == 0 && enteredTarget)
        {
            Debug.Log("Target stay!");
            currentDistanceReward += distanceRewardPerStep;
            currentTargetReward += targetRewardPerStep;
            AddReward(targetRewardPerStep + distanceRewardPerStep);
            parkingCount++;
            if (parkingCount >= framesToPark)
            {
                AddReward(parkingRewardMultiplier - currentDistanceReward - currentTargetReward + (1 - StepCount / MaxStep) * (distanceRewardMultiplier + targetRewardMultiplier));
                //Debug.Log("Parked! " + GetCumulativeReward());
                Debug.Log("Parked!");
                EndEpisode();
            }
        }
        else
        {
            parkingCount = 0;
            if (firstStep)
            {
                firstStep = false;
            }
            else
            {
                if (currentDistance < lastDistance)
                {
                    //float dot = Mathf.Abs(Vector3.Dot(transform.forward, parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.forward));
                    float dot = Mathf.Abs(Vector3.Dot(transform.forward, (parkingSlots[currentSlotNumber].target.transform.localPosition - transform.localPosition).normalized));
                    currentDistanceReward += 0.5f * distanceRewardPerStep * dot + 0.5f * distanceRewardPerStep;
                    AddReward(0.5f * distanceRewardPerStep * dot + 0.5f * distanceRewardPerStep);
                }
                else
                {
                    //float dot = Mathf.Abs(Vector3.Dot(transform.right, parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.right));
                    //currentNegativeDistanceReward -= 0.5f * distanceRewardPerStep * dot + 0.5f * distanceRewardPerStep;
                    //AddReward(-(0.5f * distanceRewardPerStep * dot + 0.5f * distanceRewardPerStep));
                    currentNegativeDistanceReward -= distanceRewardPerStep;
                    AddReward(-distanceRewardPerStep);
                }
            }
        }
        lastDistance = currentDistance;
        //Debug.Log(GetCumulativeReward());

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetButton("Jump") ? 2 : 1;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tagObstacle)
        {

            /*Debug.Log("Collision!");
            float penalty = CalculateCollisionPenalty();
            AddReward(penalty);*/
            Debug.Log("Collision!");
            float penalty = CalculateCollisionPenalty();
            AddReward(penalty - distanceRewardMultiplier + currentNegativeDistanceReward);
            EndEpisode();
        }
    }

    public override void OnTargetEnter(Collider other)
    {
        enteredTarget = true;
        if (!enteredTargetFirstTime)
        {
            Debug.Log("Target first time!");
            enteredTargetFirstTime = true;
            AddReward(enteredTargetFirstTimeReward);
        }
    }

    public override void OnBoundsEnter(Collider other)
    {
        enteredBoundsCount++;
        if (!enteredBoundsFirstTime)
        {
            Debug.Log("Bounds first time!");
            enteredBoundsFirstTime = true;
            AddReward(enteredBoundsFirstTimeReward);
        }
    }

    public override void OnTargetExit(Collider other)
    {
        enteredTarget = false;
    }

    public override void OnBoundsExit(Collider other)
    {
        enteredBoundsCount--;
    }

    protected override float CalculateCollisionPenalty()
    {
        float result = currentCollisionPenalty;
        currentCollisionPenalty *= collisionPenaltyMultiplier;
        //Debug.Log(-collisionPenalty * result);
        return -collisionPenalty * result;
    }

    protected override void DoTyres(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform tyre = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        tyre.transform.SetPositionAndRotation(position, rotation);
    }

    protected override List<Transform> GetParkingSlotsFromParking(GameObject parking)
    {
        List<Transform> result = new List<Transform>();
        Transform parkingArea = parking.transform.Find(parkingAreaName);
        Transform parkingSlot = parkingArea.Find(baseParkingChildName);
        int count = 0;
        while (parkingSlot != null)
        {
            result.Add(parkingSlot);
            count++;
            parkingSlot = parkingArea.Find(baseParkingChildName + " (" + count + ")");
        }
        return result;
    }
}
