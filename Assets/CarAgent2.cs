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

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (MaxStep);
        distanceRewardPerStep = existencePenalty * distanceRewardMultiplier;
        targetRewardPerStep = existencePenalty * targetRewardMultiplier;
        targetRewardMultiplier = 1f - enteredBoundsFirstTimeReward - enteredTargetFirstTimeReward - distanceRewardMultiplier - parkingRewardMultiplier;

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
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.forward));
        
        sensor.AddObservation((parkingSlots[currentSlotNumber].target.transform.parent.localPosition - transform.localPosition).normalized);

        sensor.AddObservation(transform.localPosition.normalized);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        sensor.AddObservation(rigidBody.velocity.normalized);
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
            if(parkingCount >= framesToPark)
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
                    float dot = Mathf.Abs(Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.forward));
                    currentDistanceReward += distanceRewardPerStep * dot;
                    AddReward(distanceRewardPerStep * dot);
                }
                else
                {
                    float dot = Mathf.Abs(Vector3.Dot(transform.forward, parkingSlots[currentSlotNumber].target.transform.right));
                    currentNegativeDistanceReward -= distanceRewardPerStep*dot;
                    AddReward(-distanceRewardPerStep*dot);
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
    
    public void OnCollisionEnter(Collision collision)
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

    public void OnTargetEnter(Collider other)
    {
        enteredTarget = true;
        if(!enteredTargetFirstTime)
        {
            Debug.Log("Target first time!");
            enteredTargetFirstTime = true;
            AddReward(enteredTargetFirstTimeReward);
        }
    }

    public void OnBoundsEnter(Collider other)
    {
        enteredBoundsCount++;
        if(!enteredBoundsFirstTime)
        {
            Debug.Log("Bounds first time!");
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
        //Debug.Log(-collisionPenalty * result);
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
