using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rewards the user when collided with the peg.
/// </summary>
public class RewardPeg : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Points being rewarded.")]
    [SerializeField] private int points = 25;

    /// <summary>
    /// Adds the reward amount when a 'Chip' object collides with the object.
    /// </summary>
    /// <param name="other">Collider of the other object.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Chip chip = other.GetComponent<Chip>();

        if (chip != null)
        {
            PlinkoManager.Instance.AddScore(points);
        }
    }
}
