using UnityEngine;

public class FlockingAgent : Agent
{
    public Agent[] flockmates;
    public GameObject followTarget;
    public float followWeight = 1f;

    public float separationDistance = 2f;
    public float cohesionDistance = 4f;
    public float alignmentDistance = 3f;

    public float separationWeight = 1.5f;
    public float cohesionWeight = 1f;
    public float alignmentWeight = 1f;
    public float wanderWeight = 0.5f;

    public override Vector3 CalcSteeringForce()
    {
        Vector3 force = Vector3.zero;

        // combines all core flocking behavior forces
        force += Separation(flockmates, separationDistance) * separationWeight;
        force += Cohesion(flockmates, cohesionDistance) * cohesionWeight;
        force += Alignment(flockmates, alignmentDistance) * alignmentWeight;
        force += Wander() * wanderWeight;

        // adds follow behavior force, if a target exists
        if (followTarget != null && followWeight > 0f)
        {
            Vector3 followForce = Seek(followTarget) * followWeight;
            force += followForce;
        }

        return force;
    }
}