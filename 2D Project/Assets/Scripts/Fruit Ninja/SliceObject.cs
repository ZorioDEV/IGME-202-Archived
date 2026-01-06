using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

/// <summary>
/// Any GO that interacts with SliceController & can be sliced.
/// </summary>
public class SliceObject : MonoBehaviour
{
    [Header("DO NOT TOUCH")]
    [Tooltip("The radius of the SliceObject (all are calcuated as circles).")]
    public float colliderRadius;    // half of a Unity scale unit
    [Tooltip("The SpriteRender of the SliceObject.")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Whether this object is currently in the process of being destroyed.")]
    public bool isBeingDestroyed = false;

    [Header("Settings")]
    [Tooltip("Duration before destruction in seconds.")]
    public float destructionDelay;  // 0.5f - half of a second

    [Tooltip("Whether this object is fruit (aka. beneficial to the player) or bomb.")]
    public bool isBeneficial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        colliderRadius = gameObject.transform.localScale.x / 2;     // assuming x- and y-scale values are equal
    }

    /// <summary>
    /// When the GO is sliced, it turns red & gets destroys after a delay.
    /// </summary>
    public virtual void OnSliced()
    {
        // ensures that the same GO isn't triggering the Destruction more than once
        if (isBeingDestroyed) return;

        // begins to destroy the object
        isBeingDestroyed = true;
        StartCoroutine(Destruction());
    }

    /// <summary>
    /// Handles the destruction sequence of the GO.
    /// </summary>
    public virtual IEnumerator Destruction()
    {
        // creates the delay
        yield return new WaitForSeconds(destructionDelay);

        // destroys the GO
        Destroy(gameObject);
    }
}
