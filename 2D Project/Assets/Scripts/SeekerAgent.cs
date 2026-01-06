using UnityEngine;

public class SeekerAgent : Agent
{
    public GameObject target;
    public GameObject fleeTarget;
    public float fleeRadius;
    public float fleeWeight;
    public float seekWeight;

    void Start()
    {
        
    }

    public override Vector3 CalcSteeringForce()
    {
        Vector3 vecFromAgentToTarget = fleeTarget.transform.position - transform.position;

        float squaredDistance = vecFromAgentToTarget.sqrMagnitude;

        Vector3 ultimateForce = Vector3.zero;

        if (squaredDistance > fleeRadius * fleeRadius)
        {
            seekWeight = 1f;
            fleeWeight = 0f;
        }
        else
        {
            seekWeight = 0f;
            fleeWeight = 1f;
        }

        ultimateForce += Seek(target) * seekWeight;
        ultimateForce += Flee(fleeTarget) * fleeWeight;

        return ultimateForce;
    }
}
