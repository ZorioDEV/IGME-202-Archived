using UnityEngine;

public class WanderingAgent : Agent
{
    public float wanderWeight = 1f;

    void Start()
    {
        // randomizes initial rotation so agents face different directions
        float randomYRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomYRotation, 0);

        // randomizes initial velocity
        velocity = Random.onUnitSphere * maxSpeed * 0.1f;
        velocity.y = 0; // keeps it horizontal
    }

    public override Vector3 CalcSteeringForce()
    {
        Vector3 wanderingForce = Wander() * wanderWeight;
        return wanderingForce;
    }
}
