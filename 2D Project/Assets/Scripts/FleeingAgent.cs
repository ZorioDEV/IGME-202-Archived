using UnityEngine;

public class FleeingAgent : Agent
{
    public GameObject target;

    void Start()
    {
        
    }

    public override Vector3 CalcSteeringForce()
    {
        Vector3 fleeForce = Flee(target);
        return fleeForce;
    }
}
