using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[System.Serializable]
public class WheelElements
{

    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public bool addWheelTorque;
    public bool shouldSteer;
}

public class ParkingSlot
{
    public GameObject target;
    public GameObject bounds;
    public GameObject staticCar;
    public GameObject setTarget;
    private readonly Vector3 staticCarStartingPosition;
    
    public ParkingSlot(GameObject _target, GameObject _bounds, GameObject _staticCar)
    {
        target = _target;
        bounds = _bounds;
        staticCar = _staticCar;
        staticCarStartingPosition = staticCar.transform.localPosition;
    }
    public ParkingSlot(GameObject _target, GameObject _bounds, GameObject _staticCar, GameObject _setTarget)
    {
        target = _target;
        bounds = _bounds;
        staticCar = _staticCar;
        staticCarStartingPosition = staticCar.transform.localPosition;
        setTarget = _setTarget;
    }

    public void Restart()
    {
        target.SetActive(false);
        bounds.SetActive(false);
        staticCar.transform.localPosition = staticCarStartingPosition;
        staticCar.SetActive(false);
    }

    public void Activate()
    {
        target.SetActive(true);
        bounds.SetActive(true);
        staticCar.transform.localPosition = staticCarStartingPosition;
        staticCar.SetActive(false);
    }
    
    public void Occupy()
    {
        target.SetActive(false);
        bounds.SetActive(false);
        staticCar.transform.localPosition = staticCarStartingPosition;
        staticCar.SetActive(true);
    }
    public void MarkSetEnd()
    {
        setTarget.SetActive(true);
    }
    public void UnmarkSetEnd()
    {
        setTarget.SetActive(false);
    }
}

public class CarAgent2 : AbstractCarAgent
{
    [Tooltip("Kara za pierwsze uderzenie. Pierwszy wyraz ci�gu geometrycznego o sumie 0.5.")]
    public float startingCollisionPenalty = 0.05f; // 1/3, 1/10

    private float existencePenalty;

    public float distanceRewardMultiplier = 0.25f;
    public float parkingRewardMultiplier = 0.4f;
    public float enteredBoundsFirstTimeReward = 0.05f;
    public float enteredTargetFirstTimeReward = 0.05f;
    private float targetRewardMultiplier;
    private float currentDistanceReward = 0f;
    private float currentTargetReward = 0f;
    private float distanceRewardPerStep;
    private float targetRewardPerStep;

    public int framesToPark = 100;
    public List<WheelElements> wheelData;
    public Transform wheelSteer;
    private List<List<ParkingSlot>> parkingSlots;
    public List<GameObject> parkings;
    private int currentParkingNumber = 0;
    private int currentSlotNumber = 0;

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

    private readonly string tagObstacle = "Obstacle";
    public bool isEmpty = true;

    private bool currentTargetRewardReset = true;

    public override void Initialize()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (MaxStep);
        distanceRewardPerStep = existencePenalty * distanceRewardMultiplier;
        targetRewardMultiplier = 1f - enteredBoundsFirstTimeReward - enteredTargetFirstTimeReward - distanceRewardMultiplier - parkingRewardMultiplier;
        targetRewardPerStep = targetRewardMultiplier / framesToPark;

        if (parkings.Count == 0)
            throw new MissingReferenceException();
        parkingSlots = new List<List<ParkingSlot>>();
        foreach (GameObject parking in parkings)
        {
            List<ParkingSlot> parkingSlotsInParking = new List<ParkingSlot>();
            foreach(Transform parkingSlot in GetParkingSlotsFromParking(parking))
                parkingSlotsInParking.Add(new ParkingSlot(parkingSlot.Find(targetName).gameObject, parkingSlot.Find(boundsName).gameObject, parkingSlot.Find(staticCarName).gameObject));
            parkingSlots.Add(parkingSlotsInParking);
        }
    }

    protected override void RandomOccupy()
    {
        List<ParkingSlot> tempParkingSlots = new List<ParkingSlot>();
        tempParkingSlots.AddRange(parkingSlots[currentParkingNumber]);
        tempParkingSlots.RemoveAt(currentSlotNumber);
        int occupySize = Random.Range(0, tempParkingSlots.Count + 1);
        while(occupySize > 0)
        {
            int tempOccupied = Random.Range(0, tempParkingSlots.Count);
            tempParkingSlots[tempOccupied].Occupy();
            tempParkingSlots.RemoveAt(tempOccupied);
            occupySize--;
        }
    }

    public override void OnEpisodeBegin()
    {
        foreach (ParkingSlot parkingSlot in parkingSlots[currentParkingNumber])
            parkingSlot.Restart();
        parkings[currentParkingNumber].SetActive(false);
        currentParkingNumber = Random.Range(0, parkingSlots.Count);
        parkings[currentParkingNumber].SetActive(true);
        currentSlotNumber = Random.Range(0, parkingSlots[currentParkingNumber].Count);
        parkingSlots[currentParkingNumber][currentSlotNumber].Activate();
        if (!isEmpty)
            RandomOccupy();
        TargetDetection targetDetection = parkingSlots[currentParkingNumber][currentSlotNumber].target.GetComponent<TargetDetection>();
        targetDetection.Initialize(this);
        BoundDetection boundDetection = parkingSlots[currentParkingNumber][currentSlotNumber].bounds.GetComponent<BoundDetection>();
        boundDetection.Initialize(this);

        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0.5f, Random.Range(minRespawnZ, maxRespawnZ));
        transform.localEulerAngles = new Vector3(0, 0, 0);

        enteredBoundsCount = 0;
        enteredTarget = false;
        enteredBoundsFirstTime = false;
        enteredTargetFirstTime = false;
        parkingCount = 0;
        currentDistanceReward = 0f;
        currentTargetReward = 0f;
        currentTargetRewardReset = true;


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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Dot(transform.forward, parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.forward));
        sensor.AddObservation(parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.forward);
        sensor.AddObservation((parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.parent.position - transform.position).normalized);
        sensor.AddObservation((parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.parent.position - transform.position).magnitude);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        sensor.AddObservation(rigidBody.velocity.normalized);
        sensor.AddObservation(rigidBody.velocity.magnitude);
        sensor.AddObservation(Vector3.Dot(transform.forward, wheelSteer.right));
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


        float currentDistance = Vector3.Distance(transform.localPosition, parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.parent.localPosition);
        if (enteredBoundsCount == 0 && enteredTarget)
        {
            Debug.Log("Target stay!");
            if (!currentTargetRewardReset)
            {
                currentTargetRewardReset = true;
                AddReward(-currentTargetReward);
                currentTargetReward = 0;
            }
            currentDistanceReward += distanceRewardPerStep;
            currentTargetReward += targetRewardPerStep;
            AddReward(targetRewardPerStep + distanceRewardPerStep);
            parkingCount++;
            if(parkingCount >= framesToPark)
            {
                AddReward(parkingRewardMultiplier - currentDistanceReward - currentTargetReward + (1 - StepCount / MaxStep) * (distanceRewardMultiplier + targetRewardMultiplier));
                Debug.Log("Parked!");
                EndEpisode();
            }
        }
        else
        {
            currentTargetRewardReset = false;
            parkingCount = 0;

            float dot = Vector3.Dot(rigidBody.velocity.normalized, (parkingSlots[currentParkingNumber][currentSlotNumber].target.transform.position - transform.position).normalized);
            if(dot > 0)
            {
                currentDistanceReward += dot * distanceRewardPerStep;
            }
            AddReward(dot * distanceRewardPerStep);
        }
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
            AddReward(-startingCollisionPenalty);
            Debug.Log("Collision!");
            EndEpisode();
        }
    }

    public override void OnTargetEnter(Collider other)
    {
        enteredTarget = true;
        if(!enteredTargetFirstTime)
        {
            enteredTargetFirstTime = true;
            AddReward((1 - StepCount / MaxStep) * enteredTargetFirstTimeReward);
        }
    }

    public override void OnBoundsEnter(Collider other)
    {
        enteredBoundsCount++;
        if(!enteredBoundsFirstTime)
        {
            enteredBoundsFirstTime = true;
            AddReward((1 - StepCount / MaxStep) * enteredBoundsFirstTimeReward);
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
        return -startingCollisionPenalty;
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
        while(parkingSlot != null)
        {
            result.Add(parkingSlot);
            count++;
            parkingSlot = parkingArea.Find(baseParkingChildName + " (" + count + ")");
        }
        return result;
    }
}
