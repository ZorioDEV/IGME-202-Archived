using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField] protected Vector3 velocity;
    [SerializeField] protected Vector3 acceleration;

    public float maxSpeed;
    public float maxForce;

    private Vector3 wanderTarget = Vector3.zero;

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

        // Orient agent to point towards its velocity (3D version)
        if (velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
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

    // *** WANDER ***
    public Vector3 Wander()
    {
        float wanderRadius = 1.5f;
        float wanderDistance = 2.5f;
        float wanderJitter = 2f;

        // adds random displacement to the wander target
        wanderTarget += new Vector3(
            Random.Range(-1f, 1f) * wanderJitter,
            Random.Range(-1f, 1f) * wanderJitter * 0.3f,
            Random.Range(-1f, 1f) * wanderJitter
        );

        // keeps the wander target on the wander circle
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        // projects the wander circle in front of the agent
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal);

        // seeks toward the wander target
        return Seek(targetWorld);
    }

    // *** PATH FOLLOW ***
    public Vector3 PathFollow(Transform[] waypoints, int currentWaypoint, float waypointRadius, float slowingDistance)
    {
        // returns steering force toward the current waypoint
        return Arrive(waypoints[currentWaypoint].position, slowingDistance);
    }

    // *** FLOCKING BEHAVIORS ***
    public Vector3 Separation(Agent[] neighbors, float separationDistance)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        // initializes separation checking & movement
        foreach (Agent neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.transform.position);

            // checks if the neighbor is close enough to repel
            if (neighbor != this && distance < separationDistance)
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                diff = diff.normalized / distance;
                steer += diff;
                count++;
            }
        }

        // calculates the final separation steering
        if (count > 0)
        {
            steer /= count;
            steer = steer.normalized * maxSpeed - velocity;
            steer = Vector3.ClampMagnitude(steer, maxForce);
        }

        // returns the separation force
        return steer;
    }

    public Vector3 Cohesion(Agent[] neighbors, float cohesionDistance)
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        // gathers the positions of nearby neighbors
        foreach (Agent neighbor in neighbors)
        {
            // confirms neighbor is within cohesion radius
            if (neighbor != this && Vector3.Distance(transform.position, neighbor.transform.position) < cohesionDistance)
            {
                center += neighbor.transform.position;
                count++;
            }
        }

        // calculates center of all the neighbors & seeks toward it
        if (count > 0)
        {
            center /= count;
            return Seek(center);
        }

        // returns zero if no cohesion is required
        return Vector3.zero;
    }

    public Vector3 Alignment(Agent[] neighbors, float alignmentDistance)
    {
        Vector3 avgVelocity = Vector3.zero;
        int count = 0;

        // gathers velocities of nearby neighbors
        foreach (Agent neighbor in neighbors)
        {
            // confirms neighbor is within alignment radius
            if (neighbor != this && Vector3.Distance(transform.position, neighbor.transform.position) < alignmentDistance)
            {
                avgVelocity += neighbor.velocity;
                count++;
            }
        }

        // calculates alignment force from all the neighbors & applies it
        if (count > 0)
        {
            avgVelocity /= count;
            avgVelocity = avgVelocity.normalized * maxSpeed;
            Vector3 steer = avgVelocity - velocity;

            return Vector3.ClampMagnitude(steer, maxForce);
        }

        // applies zero if no alignment is required
        return Vector3.zero;
    }
}