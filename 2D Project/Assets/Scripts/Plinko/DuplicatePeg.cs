using UnityEngine;

/// <summary>
/// Duplicates chip when collided with the peg.
/// </summary>
public class DuplicatePeg : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Total distance the peg will move from starting position.")]
    [SerializeField] private float moveDistance = 1f;
    [Tooltip("Speed of the peg.")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Prefab")]
    [Tooltip("Reference to the Chip prefab.")]
    [SerializeField] private GameObject ballPrefab;
    [Tooltip("Cooldown for the duplication.")]
    [SerializeField] private float duplicationCooldown = 0.5f;

    private Vector3 startPosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private bool movingRight;
    private bool canDuplicate = true;
    private float lastDuplicationTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startPosition = transform.position;

        leftPosition = startPosition + Vector3.left * moveDistance;
        rightPosition = startPosition + Vector3.right * moveDistance;

        movingRight = true;

        lastDuplicationTime = -duplicationCooldown;
    }

    /// <summary>
    /// Moves the peg back-and-forth in a looping pattern & has cooldown for duplication.
    /// </summary>
    private void Update()
    {
        MovePeg();

        // checks if cooldown period has passed
        if (!canDuplicate && Time.time >= lastDuplicationTime + duplicationCooldown)
        {
            canDuplicate = true;
        }
    }

    /// <summary>
    /// Duplicates the Chip prefab on collision with a cooldown.
    /// </summary>
    /// <param name="other">Collider of the other object.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        Chip ball = other.gameObject.GetComponent<Chip>();

        if (ball != null && canDuplicate)
        {
            // spawns from the peg
            Vector3 spawnPosition = transform.position;
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

            // starts cooldown
            canDuplicate = false;
            lastDuplicationTime = Time.time;
        }
    }

    /// <summary>
    /// Moves the peg back-and-forth with a set distance.
    /// </summary>
    private void MovePeg()
    {
        // sets the target position
        Vector3 targetPosition;
        if (movingRight)
        {
            targetPosition = rightPosition;
        }
        else
        {
            targetPosition = leftPosition;
        }

        // alters position to move towards target position
        transform.position = Vector3.MoveTowards(
                                    transform.position,
                                    targetPosition,
                                    moveSpeed * Time.deltaTime      
                             );

        // turns the peg to the other target position once reaching the current target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }
}
