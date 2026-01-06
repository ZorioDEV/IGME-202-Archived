using UnityEngine;

/// <summary>
/// Bucket reward system logic.
/// </summary>
public class Bucket : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Points being rewarded.")]
    [SerializeField] private int points;

    /// <summary>
    /// Adds the reward amount when a 'Chip' object collides with the object, then destorys the chip.
    /// </summary>
    /// <param name="other">Collider of the other object.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Chip chip = other.GetComponent<Chip>();

        if (chip != null)
        {
            PlinkoManager.Instance.AddScore(points);

            Destroy(other.gameObject);
        }
    }
}
