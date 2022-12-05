using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
public abstract class AbstractCarAgent: Agent
{
    protected abstract void RandomOccupy();
    public abstract void OnCollisionEnter(Collision collision);
    public abstract void OnTargetEnter(Collider other);
    public abstract void OnBoundsEnter(Collider other);
    public abstract void OnTargetExit(Collider other);
    public abstract void OnBoundsExit(Collider other);
    protected abstract float CalculateCollisionPenalty();
    protected abstract void DoTyres(WheelCollider collider);
    protected abstract List<Transform> GetParkingSlotsFromParking(GameObject parking);
}
