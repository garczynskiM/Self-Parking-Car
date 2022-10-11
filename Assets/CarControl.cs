using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public class WheelElements
{

    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public bool addWheelTorque;
    public bool shouldSteer;

}*/

public class CarControl : MonoBehaviour
{
    public List<WheelElements> wheelData;

    public float maxTorque;
    public float maxSteerAngle;

    private Rigidbody rb;
    public Transform massCenter;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = massCenter.localPosition;

    }

    private void FixedUpdate()
    {
        float speed = Input.GetAxis("Vertical") * maxTorque;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        bool brake = Input.GetButton("Jump");

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

    }

    void DoTyres(WheelCollider collider)
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
