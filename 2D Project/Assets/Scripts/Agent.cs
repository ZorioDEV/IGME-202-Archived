using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected Vector3 velocity;
    [SerializeField] protected Vector3 acceleration;

    public float maxSpeed;
    public float maxForce;

    void Start()
    {
        
    }

    public abstract Vector3 CalcSteeringForce();

    /// <summary>
    /// Updates the agent's position based on calculated steering forces.
    /// </summary>
    void Update()
    {
        // Start "fresh" every frame with no previous acceleration
        acceleration = Vector3.zero;

        // Calculate the steering force
        Vector3 steeringForce = CalcSteeringForce();

        // Scale it to a max force (more or less is applied to agent)
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        // Basic "movement" for agents
        acceleration += steeringForce;
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        // Orient agent to point towards its velocity
        if (velocity.magnitude != 0)
        {
            transform.up = velocity.normalized;
        }
    }

    // *** SEEK ***
    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    public Vector3 Seek(Vector3 targetVector)
    {
        // Calculate the desired velocity
        Vector3 desiredVelocity = targetVector - transform.position;

        // Scale to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the steering force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Return the steering force
        return steeringForce;
    }

    // *** FLEE ***
    public Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }

    public Vector3 Flee(Vector3 targetVector)
    {
        // Calculate the desired velocity
        Vector3 desiredVelocity = transform.position - targetVector;

        // Scale to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the steering force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Return the steering force
        return steeringForce;
    }

    // *** ARRIVE ***
    public Vector3 Arrive(GameObject target, float slowingDistance)
    {
        return Arrive(target.transform.position, slowingDistance);
    }

    public Vector3 Arrive(Vector3 targetVector, float slowingDistance)
    {
        // Calculate the desired velocity
        Vector3 desiredVelocity = targetVector - transform.position;
        float distance = desiredVelocity.magnitude;

        // Scale to max speed or slower, if within slowing distance
        if (distance < slowingDistance)
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed * ((distance / slowingDistance) / 1.25f);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }

        // Calculate the steering force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Set to zero if we're very close to target
        if (distance < 0.01f)
        {
            velocity = Vector3.zero;
            steeringForce = Vector3.zero;
        }

        // Return the steering force
        return steeringForce;
    }
}
