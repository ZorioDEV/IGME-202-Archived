using UnityEngine;

public class PathFollowerAgent : Agent
{
    public Transform[] waypoints;
    public GameObject[] hazards;
    public int currentWaypoint = 0;
    public float fleeRadius;
    public float waypointRadius;
    public float slowingDistance;

    public override Vector3 CalcSteeringForce()
    {
        Vector3 force = Vector3.zero;

        // checks whether the agent reaches the current waypoint & advances to the next
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < waypointRadius)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        // checks for nearby hazards
        GameObject closestHazard = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject hazard in hazards)
        {
            float distance = Vector3.Distance(transform.position, hazard.transform.position);
            if (distance < fleeRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closestHazard = hazard;
            }
        }

        // applies forces with weights
        if (closestHazard != null)
        {
            // flees from hazard with high priority
            force += Flee(closestHazard) * 2f;

            // follows path, but with lower priority
            force += PathFollow(waypoints, currentWaypoint, waypointRadius, slowingDistance) * 0.5f;
        }
        else
        {
            // follows path normally
            force += PathFollow(waypoints, currentWaypoint, waypointRadius, slowingDistance);
        }

        return force;
    }
}
