using UnityEngine;

public class DragController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the RandomSpawnManager.")]
    public RandomSpawnManager spawnManager;

    [Header("Settings")]
    [Tooltip("Distance interval the mouse must travel to spawn the next object.")]
    public float spawnDistanceThreshold;

    private Vector3 previousMousePosition;
    private float currentDragDistance;
    private bool mouseIsDragging = false;

    /// <summary>
    /// Initializes the previous mouse position.
    /// </summary>
    private void Start()
    {
        previousMousePosition = GetMouseWorldPosition();
    }

    /// <summary>
    /// Handles the spawning conditions with mouse clicks & dragging.
    /// </summary>
    private void Update()
    {
        // when left mouse button pressed
        if (Input.GetMouseButtonDown(0))
        {
            mouseIsDragging = true;
            currentDragDistance = 0f;
            Vector3 clickPosition = GetMouseWorldPosition();
            previousMousePosition = clickPosition;

            // spawns first object immediately at click position
            spawnManager.Spawn(clickPosition);
        }

        // when left mouse button released
        if (Input.GetMouseButtonUp(0))
        {
            mouseIsDragging = false;
            currentDragDistance = 0f;
        }

        if (mouseIsDragging)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            
            // calculates distance moved since last check
            float distanceMoved = Vector3.Distance(previousMousePosition, currentMousePosition);
            currentDragDistance += distanceMoved;

            // updates previous position
            previousMousePosition = currentMousePosition;

            // spawns if mouse moved enough
            if (currentDragDistance >= spawnDistanceThreshold)
            {
                spawnManager.Spawn(currentMousePosition);

                // resets distance, but keeps current position as new starting point
                currentDragDistance = 0f;
            }
        }
    }

    /// <summary>
    /// Converts mouse position to world coordinates
    /// </summary>
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1f;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
