using UnityEngine;

public class MouseHighlight : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The color when mouse button is pressed down.")]
    public Color mouseDownColor;
    [Tooltip("The Color when mouse button is not pressed.")]
    public Color mouseUpColor;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // sets the defualt color as mouseUpColor
        spriteRenderer.color = mouseUpColor;
    }

    /// <summary>
    /// Updates the color based on mouse button state.
    /// </summary>
    private void Update()
    {
        // changes color if left click is held down
        if (Input.GetMouseButton(0))
        {
            spriteRenderer.color = mouseDownColor;
        }
        // changes color back to default if left click is released (not held down)
        else
        {
            spriteRenderer.color = mouseUpColor;
        }
    }
}
