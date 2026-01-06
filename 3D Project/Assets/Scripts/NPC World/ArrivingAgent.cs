using UnityEngine;

public class ArrivingAgent : Agent
{
    public GameObject target;
    public float slowingDistance;

    void Start()
    {
        
    }

    public override Vector3 CalcSteeringForce()
    {
        Vector3 arrivingForce = Arrive(target, slowingDistance);
        return arrivingForce;
    }

}
