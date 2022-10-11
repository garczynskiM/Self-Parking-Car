using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

/*[System.Serializable]
public class WheelElements
{

    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public bool addWheelTorque;
    public bool shouldSteer;

}*/

public class CarAgent : Agent
{
    [Tooltip("Baza do liczenia nagrody za parkowanie, pozostawiæ w przedziale [0, 1].")]
    public float baseReward;
    private float helpingReward;
    private float currentHelpingReward;
    private float maxHelpingReward;
    private float startingDistance;
    private float minDistance;
    [Tooltip("Czêœæ nagrody przyznawana za odleg³oœæ parkowania od œrodka. Przedzia³ [0, 1]")]
    public float distanceReward = 0.8f;
    private float angleReward;
    private float existencePenalty;
    [Tooltip("Kara za pierwsze uderzenie. Pierwszy wyraz ci¹gu geometrycznego o sumie 0.5.")]
    public float startingCollisionPenalty = 1 / 4f; // 1/3, 1/10
    private float currentCollisionPenalty;
    [Tooltip("Liczba przez któr¹ przemna¿amy karê za uderzenie za ka¿de kolejne uderzenie. Iloraz ci¹gu geometrycznego o sumie 0.5.")]
    public float collisionPenaltyMultiplier = 1 / 2f; // 1/3, 4/5
    
    [Tooltip("Maksymalny zasiêg od œrodka celu, w jakim zatrzymanie siê uznawane jest za parkowanie, pobierane z wymiarów celu. \nWyœwietlane dla podgl¹du.")]
    public float maxAcceptableParkingRange = 5f;

    public Transform Target;
    private float targetModuloAngle;

    public List<WheelElements> wheelData;
    public Transform WheelSteer;

    public float maxTorque;
    public float maxSteerAngle;

    private Rigidbody rb;
    public Transform massCenter;

    public BoxCollider carCollider;

    public readonly float maxPassingVelocity = 0.001f;


    private void Awake()
    {
        CarCollisionDetection carCollisionDetection = carCollider.gameObject.GetComponent<CarCollisionDetection>();
        carCollisionDetection.Initialize(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = massCenter.localPosition;
        existencePenalty = 1f / (2 * MaxStep);
        angleReward = System.Math.Max(System.Math.Min(1 - distanceReward, 1f), 0f);
        maxAcceptableParkingRange = Target.GetComponent<MeshRenderer>().bounds.size.x / 2f;
        targetModuloAngle = Modulo(Target.transform.localEulerAngles.y, 360);
        helpingReward = 1f - baseReward;
    }

    public override void OnEpisodeBegin()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(25, 0.5f, -3.5f);
        transform.localEulerAngles = new Vector3(0, -90, 0);

        currentCollisionPenalty = startingCollisionPenalty;
        maxHelpingReward = 0f;
        currentHelpingReward = 0f;
        startingDistance = Vector3.Distance(transform.localPosition, Target.localPosition);
        minDistance = startingDistance;


        foreach (WheelElements element in wheelData)
        {
            if(element.shouldSteer == true)
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
        sensor.AddObservation(Target.localPosition.x);
        sensor.AddObservation(Target.localPosition.z);
        sensor.AddObservation(Target.localEulerAngles.y);

        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);
        sensor.AddObservation(this.transform.localEulerAngles.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
        sensor.AddObservation(WheelSteer.localEulerAngles.y);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float speed = actionBuffers.ContinuousActions[0] * maxTorque;
        float steer = actionBuffers.ContinuousActions[1] * maxSteerAngle;
        bool brake = actionBuffers.DiscreteActions[0] == 2 ;

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

        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);

        if(distanceToTarget < minDistance)
        {
            minDistance = distanceToTarget;
            currentHelpingReward = 1f - minDistance / startingDistance;

            AddReward(helpingReward * (currentHelpingReward - maxHelpingReward));

            maxHelpingReward = currentHelpingReward;
        }

        if (distanceToTarget < maxAcceptableParkingRange && System.Math.Abs(rb.velocity.x) <= maxPassingVelocity  && System.Math.Abs(rb.velocity.z) <= maxPassingVelocity)
        {
            AddReward(baseReward * CalculateAngleRewardModificator(transform.localEulerAngles.y) * CalculateDistanceRewardModificator(distanceToTarget));
            EndEpisode();
        }
        AddReward(-existencePenalty);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetButton("Jump")?2:1;
    }

    public void OnCollisionEnter(Collision collision)
    {
        float penalty = CalculateCollisionPenalty();
        AddReward(penalty);
    }

    public void OnCollisionExit(Collision collision)
    {
        
    }

    public void OnCollisionStay(Collision collision)
    {
        float penalty = CalculateCollisionPenalty();
        AddReward(penalty);
    }


    private float CalculateAngleRewardModificator(float agentRotation)
    {
        return (float)(-System.Math.Abs(System.Math.Sin(System.Math.PI * (targetModuloAngle - Modulo(agentRotation, 360)) / 360)) + 1) * angleReward;
    }

    private float CalculateDistanceRewardModificator(float distance)
    {
        return (float)System.Math.Pow(distance / maxAcceptableParkingRange - 1, 2) * distanceReward;
    }

    private float CalculateCollisionPenalty()
    {
        float result = currentCollisionPenalty;
        currentCollisionPenalty *= collisionPenaltyMultiplier;
        return -result;
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

    private float Modulo(float a, float b)
    {
        return (float)(a - b * System.Math.Floor(a / b));
    }
}
