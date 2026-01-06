using UnityEngine;

public class FleeingAgent : Agent
{
    public GameObject[] target;
    public float fleeRadius = 10f;

    public override Vector3 CalcSteeringForce()
    {
        Vector3 fleeForce = Vector3.zero;

        // goes through all targets & applies flee force when within the flee radius
        foreach (GameObject go in target)
        {
            float distance = Vector3.Distance(transform.position, go.transform.position);

            if (distance < fleeRadius)
            {
                fleeForce += Flee(go);
            }
        }

        // applies a braking force when almost no flee force is present, but the agent is still moving
        if (fleeForce.magnitude < 0.1f && velocity.magnitude > 0.1f)
        {
            Vector3 brakingForce = -velocity.normalized * maxForce * 0.5f;
            return brakingForce;
        }
        // stops all movement when velocity is nearly zero for flee
        else if (velocity.magnitude < 0.1f)
        {
            velocity = Vector3.zero;
        }

        return fleeForce;
    }
}