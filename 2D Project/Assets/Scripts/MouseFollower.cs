using System;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("Vector of the current mouse's position.")]
    public Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        MoveToMouse();
    }

    /// <summary>
    /// Makes the position of the GO the same as the mouse's position.
    /// </summary>
    public void MoveToMouse()
    {
        // gets mouse position
        mousePosition = Input.mousePosition;

        // converts screen point to world point
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // defualts z-value to 0 ALWAYS

        // sets the GO's position to the mouse position
        gameObject.transform.position = worldPosition;
    }
}
