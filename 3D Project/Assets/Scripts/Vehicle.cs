using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour
{
    // Reference to Rigidbody on this GameObject
    // NOTE: We can still call methods on the Rigidbody even though 
    //   it's not using the physics system’s auto-applied physics.
    public Rigidbody rBody;

    // Part of “movement formula” we've studied thus far
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraFollowSpeed;

    // movement variables
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 acceleration;

    // Capturing user input to move vehicle along its transform
    [SerializeField] private Vector3 movementDirection;

    // Handling smallest and largest speeds
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;

    // Rates of acceleration and deceleration
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;

    // Steering the vehicle 
    [SerializeField] private float turnSpeed;    // Scalar of velocity
    [SerializeField] private Quaternion turning;	// Rotation

    // alignment settings
    [SerializeField] private float frontRaycastOffset;
    [SerializeField] private float sideRaycastOffset;
    [SerializeField] private float vehicleHeightOffset;
    [SerializeField] private float rotationSmoothSpeed;

    private Vector3 terrainNormal = Vector3.up;

    public void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    /// <summary>
    /// Updates the camera to follow the vehicle without rotating.
    /// </summary>
    private void UpdateCamera()
    {
        // calculates target position behind relative to the vehicle
        Vector3 targetPosition = transform.position - transform.forward * cameraOffset.z + Vector3.up * cameraOffset.y + transform.right * cameraOffset.x;

        // smoothly moves camera to target position
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraFollowSpeed);

        // keeps camera looking at the vehicle
        cameraTransform.LookAt(transform.position);
    }

    /// <summary>
    /// Handles the vehicle's movement logic w/ alignment.
    /// </summary>
    public void Move()
    {
        // gets terrain alignment data
        AlignToTerrain();

        // gets current position & rotation
        Vector3 nextPosition = transform.position;
        Quaternion nextRotation = transform.rotation;

        // calculates current speed for this frame
        float currentSpeed = movementDirection.z * maxSpeed;

        // handles acceleration
        if (movementDirection.z != 0)
        {
            acceleration = Vector3.zero;

            // projects acceleration onto terrain plane
            Vector3 forwardOnTerrain = Vector3.ProjectOnPlane(transform.forward, terrainNormal).normalized;
            acceleration = forwardOnTerrain * (movementDirection.z * accelerationRate);

            // adds acceleration to velocity
            velocity += acceleration * Time.deltaTime;

            // projects velocity onto terrain plane & clamp
            velocity = Vector3.ProjectOnPlane(velocity, terrainNormal);
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        // handles deceleration
        else if (velocity.magnitude != 0f)
        {
            velocity *= 1f - (decelerationRate * Time.deltaTime);

            if (velocity.magnitude < minSpeed)
            {
                velocity = Vector3.zero;
            }
        }

        // calculates rotation for steering
        float turnAmount = movementDirection.x * turnSpeed * Time.fixedDeltaTime;

        // rotates around terrain normal instead of world up
        turning = Quaternion.AngleAxis(turnAmount, terrainNormal);

        // generates a quaternion representing the vehicle's rotation
        nextRotation *= turning;

        // rotates the velocity vector around terrain normal
        velocity = turning * velocity;

        // projects velocity onto terrain plane
        velocity = Vector3.ProjectOnPlane(velocity, terrainNormal);

        // calculates next position
        nextPosition += velocity * Time.fixedDeltaTime;

        // applies terrain alignment to final position
        nextPosition = GetPositionOnTerrain(nextPosition);

        // applies terrain alignment to rotation
        Vector3 projectedForward = Vector3.ProjectOnPlane(nextRotation * Vector3.forward, terrainNormal).normalized;
        if (projectedForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedForward, terrainNormal);
            nextRotation = Quaternion.Slerp(nextRotation, targetRotation, Time.fixedDeltaTime * rotationSmoothSpeed);
        }

        // moves the rigidbody
        rBody.Move(nextPosition, nextRotation);
    }

    /// <summary>
    /// Aligns the vehicle to the terrain using three raycasts.
    /// </summary>
    private void AlignToTerrain()
    {
        // gets three raycast points: center, front, and right
        Vector3 centerPoint = transform.position;
        Vector3 frontPoint = transform.position + transform.forward * frontRaycastOffset;
        Vector3 rightPoint = transform.position + transform.right * sideRaycastOffset;

        // shoots raycasts downwards
        RaycastHit centerHit, frontHit, rightHit;
        bool centerHitSuccess = Physics.Raycast(centerPoint, Vector3.down, out centerHit, 1000f);
        bool frontHitSuccess = Physics.Raycast(frontPoint, Vector3.down, out frontHit, 1000f);
        bool rightHitSuccess = Physics.Raycast(rightPoint, Vector3.down, out rightHit, 1000f);

        if (centerHitSuccess && frontHitSuccess && rightHitSuccess)
        {
            // calculates average normal from three points
            terrainNormal = (centerHit.normal + frontHit.normal + rightHit.normal).normalized;
        }
        else
        {
            // sets default to world up, if raycasts fail (flat)
            terrainNormal = Vector3.up;
        }
    }

    /// <summary>
    /// Gets a position on the terrain by raycasting downward.
    /// </summary>
    private Vector3 GetPositionOnTerrain(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 1000f))
        {
            return hit.point + Vector3.up * vehicleHeightOffset;    // w/ optional height offset
        }
        return position;
    }

    /// <summary>
    /// Handles movement input from the player.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // See what value is being retrieved from the WASD input system
        Vector2 retrievedDirection = context.ReadValue<Vector2>();

        // The direction's Y value is being retrieved while the Vechile's moves along its Z axis
        movementDirection = new Vector3(retrievedDirection.x, 0, retrievedDirection.y);
    }

    /// <summary>
    /// Draws the visual debugging Gizmos for the vehicle & raycasts.
    /// </summary>
    private void OnDrawGizmos()
    {
        // forward vector - blue
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 4f);

        // right vector - red
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 4f);

        // up vector - green
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 4f);

        // direction vector - yellow
        // checks if the vehicle is moveing or not
        if (movementDirection != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + movementDirection.normalized * 2f);
        }

        // world center - white
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, Vector3.zero);

        // raycast positions &  paths - cyan
        Gizmos.color = Color.cyan;

        // calculates raycast start positions
        Vector3 centerStart = transform.position;
        Vector3 frontStart = transform.position + transform.forward * frontRaycastOffset;
        Vector3 rightStart = transform.position + transform.right * sideRaycastOffset;

        // raycast lines shooting down from the start points
        Gizmos.DrawLine(centerStart, centerStart + Vector3.down * 100f);
        Gizmos.DrawLine(frontStart, frontStart + Vector3.down * 100f);
        Gizmos.DrawLine(rightStart, rightStart + Vector3.down * 100f);
    }
}