using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;
using Unity.MLAgents.Policies;
using Unity.Barracuda;

public class ModelRaceCarAgent : AbstractCarAgent
{
    //Nowe pola
    //private Toggle m_autoRestartToggle;
    //private Toggle m_otherCarsToggle;
    [SerializeField] public Transform m_scriptHolder;
    //Koniec nowych pól

    private int numberOfSimulations;
    public int NumberOfSimulations { get => numberOfSimulations; }

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
    //public RaceState presetRaceState = RaceState.None;
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
    private List<ParkingSlot> listOfOccupiedSpaces;

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
    private bool currentParkingSuccess = false;

    public override void Initialize()
    {
        //Nowe pola
        //m_autoRestartToggle = MapLoadVarsSingleton.m_autoRestartTransform.GetComponentInChildren<Toggle>();
        //m_otherCarsToggle = MapLoadVarsSingleton.m_otherCarsTransform.GetComponentInChildren<Toggle>();
        //Koniec nowych pól
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (MaxStep);
        distanceRewardPerStep = existencePenalty * distanceRewardMultiplier;
        targetRewardPerStep = existencePenalty * targetRewardMultiplier;
        targetRewardMultiplier = 1f - enteredBoundsFirstTimeReward - enteredTargetFirstTimeReward - distanceRewardMultiplier - parkingRewardMultiplier;

        BehaviorParameters behaviour = (BehaviorParameters)GetComponent("BehaviorParameters");
        NNModel modelToLoad = Resources.Load<NNModel>(string.Format("NN Models/{0}", MapLoadVarsSingleton.Instance.modelInfo.name));
        behaviour.Model = modelToLoad;
        behaviour.BehaviorType = BehaviorType.InferenceOnly;
        if (parking == null)
            throw new MissingReferenceException();
        parkingSlots = new List<ParkingSlot>();
        //EDIT
        //List<ParkingSlot> parkingSlotsInParking = new List<ParkingSlot>();
        foreach (Transform parkingSlot in GetParkingSlotsFromParking(parking))
            parkingSlots.Add(new ParkingSlot(parkingSlot.Find(targetName).gameObject, parkingSlot.Find(boundsName).gameObject, parkingSlot.Find(staticCarName).gameObject));
        RaceSettingsSingleton.Instance.numberOfParkingSlots = parkingSlots.Count;
        numberOfSimulations = 0;
    }

    protected override void RandomOccupy() { }
    protected void fillParkingSlots()
    {
        foreach (ParkingSlot parkingSlot in parkingSlots)
            parkingSlot.Restart();
        parkingSlots[RaceSettingsSingleton.Instance.targetParkingSlot].Activate();
        for(int i = 0; i < RaceSettingsSingleton.Instance.occupiedParkingSlots.Count; i++)
        {
            parkingSlots[RaceSettingsSingleton.Instance.occupiedParkingSlots[i]].Occupy();
        }
    }
    public override void OnEpisodeBegin()
    {
        //
        numberOfSimulations++;
        RaceSummarySingleton.Instance.carEnd = System.DateTime.Now;
        RaceSummarySingleton.Instance.carParkingSuccessful = currentParkingSuccess;
        currentParkingSuccess = false;
        RaceSummarySingleton.Instance.summaryClosed = false;
        if (!RaceSettingsSingleton.Instance.carManualRestart && numberOfSimulations > 1)
        {
            var raceParallerManager = m_scriptHolder.gameObject.GetComponent<RaceParallelManager>();
            raceParallerManager.finishedParking(CarOwner.Model);
        }
        else
        {
            var raceParallerManager = m_scriptHolder.gameObject.GetComponent<RaceParallelManager>();
            raceParallerManager.manuallyRestarted();
            RaceSettingsSingleton.Instance.carManualRestart = false;
            fillParkingSlots();
            currentSlotNumber = RaceSettingsSingleton.Instance.targetParkingSlot;
            TargetDetection targetDetection = parkingSlots[currentSlotNumber].target.GetComponent<TargetDetection>();
            targetDetection.Initialize(this);
            BoundDetection boundDetection = parkingSlots[currentSlotNumber].bounds.GetComponent<BoundDetection>();
            boundDetection.Initialize(this);

            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, RaceSettingsSingleton.Instance.currentCarRespawnZ);
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
            RaceSummarySingleton.Instance.carStart = System.DateTime.Now;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.forward));
        sensor.AddObservation(parkingSlots[currentSlotNumber].target.transform.forward);
        sensor.AddObservation((parkingSlots[currentSlotNumber].target.transform.parent.position - transform.position).normalized);
        sensor.AddObservation((parkingSlots[currentSlotNumber].target.transform.parent.position - transform.position).magnitude);
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
                Debug.Log("Parked! " + GetCumulativeReward());
                Debug.Log("Parked!");
                currentParkingSuccess = true;
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
        Debug.Log("Bounds!");
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
