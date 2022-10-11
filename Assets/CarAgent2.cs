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

    public ParkingSlot(GameObject _target, GameObject _bounds)
    {
        target = _target;
        bounds = _bounds;
    }
}

    public class CarAgent2 : Agent
{
    [Tooltip("Kara za pierwsze uderzenie. Pierwszy wyraz ci¹gu geometrycznego o sumie 0.5.")]
    public float startingCollisionPenalty = 1 / 4f; // 1/3, 1/10
    private float currentCollisionPenalty;
    [Tooltip("Liczba przez któr¹ przemna¿amy karê za uderzenie za ka¿de kolejne uderzenie. Iloraz ci¹gu geometrycznego o sumie 0.5.")]
    public float collisionPenaltyMultiplier = 1 / 2f; // 1/3, 4/5
    private float collisionPenalty = 0.1f;

    [Tooltip("Maksymalny zasiêg od œrodka celu, w jakim zatrzymanie siê uznawane jest za parkowanie, pobierane z wymiarów celu. \nWyœwietlane dla podgl¹du.")]
    public float maxAcceptableParkingRange = 5f;

    private float existencePenalty;

    public List<WheelElements> wheelData;
    public Transform wheelSteer;
    public GameObject parking;
    private List<ParkingSlot> parkingSlots;

    private int currentSlotNumber = 0;

    //public BoxCollider targetCollider;

    public float maxTorque;
    public float maxSteerAngle;

    private Rigidbody rigidBody;
    public Transform massCenter;

    public BoxCollider carCollider;

    public string baseParkingChildName = "parkingSlot";
    public string targetName = "Target";
    public string boundsName = "slotBounds";

    public float maxRespawnZ = 11f;
    public float minRespawnZ = -11f;

    private int enteredBoundsCount = 0;
    private bool enteredTarget = false;
    private bool enteredBoundsFirstTime = false;
    private bool enteredTargetFirstTime = false;
    private float enteredBoundsFirstTimeReward = 0.1f;
    private float enteredTargetFirstTimeReward = 0.1f;
    private float baseReward;

    private void Awake()
    {
        CarCollisionDetection2 carCollisionDetection = carCollider.gameObject.GetComponent<CarCollisionDetection2>();
        carCollisionDetection.Initialize(this);
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (2f * MaxStep);
        baseReward = 1f - enteredBoundsFirstTimeReward - enteredTargetFirstTimeReward;

        parkingSlots = new List<ParkingSlot>();
        int count = 0;
        Transform parkingSlot = parking.transform.Find(baseParkingChildName);
        while(parkingSlot != null)
        {
            parkingSlots.Add(new ParkingSlot(parkingSlot.Find(targetName).gameObject, parkingSlot.Find(boundsName).gameObject));
            count++;
            parkingSlot = parking.transform.Find(baseParkingChildName + " (" + count + ")");
        }
    }

    public override void OnEpisodeBegin()
    {
        parkingSlots[currentSlotNumber].target.SetActive(false);
        parkingSlots[currentSlotNumber].bounds.SetActive(false);
        currentSlotNumber = Random.Range(0, parkingSlots.Count);
        parkingSlots[currentSlotNumber].target.SetActive(true);
        parkingSlots[currentSlotNumber].bounds.SetActive(true);
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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(parkingSlots[currentSlotNumber].target.transform.localPosition.x);
        sensor.AddObservation(parkingSlots[currentSlotNumber].target.transform.localPosition.z);
        sensor.AddObservation(parkingSlots[currentSlotNumber].target.transform.localEulerAngles.y);

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(transform.localEulerAngles.y);
        sensor.AddObservation(rigidBody.velocity.x);
        sensor.AddObservation(rigidBody.velocity.z);
        sensor.AddObservation(wheelSteer.localEulerAngles.y);
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

        if(enteredBoundsCount == 0 && enteredTarget)
        {
            AddReward(2*existencePenalty*baseReward);
        }
        else
        {
            AddReward(-existencePenalty);
        }
        // Debug.Log(GetCumulativeReward());

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetButton("Jump") ? 2 : 1;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        float penalty = CalculateCollisionPenalty();
        AddReward(penalty);
    }

    public void OnCollisionStay(Collision collision)
    {
        float penalty = CalculateCollisionPenalty();
        AddReward(penalty);
    }
    public void OnTargetEnter(Collider other)
    {
        enteredTarget = true;
        if(!enteredTargetFirstTime)
        {
            enteredTargetFirstTime = true;
            AddReward(enteredTargetFirstTimeReward);
        }
    }

    public void OnBoundsEnter(Collider other)
    {
        enteredBoundsCount++;
        if(!enteredBoundsFirstTime)
        {
            enteredBoundsFirstTime = true;
            AddReward(enteredBoundsFirstTimeReward);
        }
    }

    public void OnTargetExit(Collider other)
    {
        enteredTarget = false;
    }

    public void OnBoundsExit(Collider other)
    {
        enteredBoundsCount--;
    }

    private float CalculateCollisionPenalty()
    {
        float result = currentCollisionPenalty;
        currentCollisionPenalty *= collisionPenaltyMultiplier;
        return -collisionPenalty*result;
    }

    private void DoTyres(WheelCollider collider)
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
}
