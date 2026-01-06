using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Universal controller/handler for click-and-drag slicing of SliceObject GOs.
/// </summary>
public class SliceController : MonoBehaviour
{
    private Vector3 previousPosition;
    private bool isDragging = false;

    /// <summary>
    /// Handles swipe/drag/slice logic.
    /// </summary>
    private void Update()
    {
        // starts drag & collects the mouse's initial position
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousPosition = GetMouseWorldPosition();
        }

        // handles the logic during the drag
        if (isDragging && Input.GetMouseButton(0))
        {
            // gets the current mouse's position
            Vector3 currentPosition = GetMouseWorldPosition();

            // finds all the sliceable GOs in the scene
            SliceObject[] allSliceableObjects = FindObjectsOfType<SliceObject>();

            // loops through all the SliceObject GOs
            foreach (SliceObject sliceable in allSliceableObjects)
            {
                // checks the collision between the drag's line (created from prev & current
                // mouse position) and SliceObject's circle (created from it's radius)
                if (LineCircleCollision(previousPosition, currentPosition, sliceable.transform.position, sliceable.colliderRadius))
                {
                    sliceable.OnSliced();
                }
            }

            previousPosition = currentPosition;
        }

        // ends drag & updates bool for isDragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    /// <summary>
    /// Checks if a line segment intersects with a circle.
    /// </summary>
    private bool LineCircleCollision(Vector3 lineStart, Vector3 lineEnd, Vector3 circleCenter, float circleRadius)
    {
        // calculates the vector from lineStart to circleCenter
        // & gets the relative line from the lineStart to the circleCenter
        Vector3 startToCenter = circleCenter - lineStart;

        // calculates the vector from lineStart to lineEnd
        // & gets the total drag line's length and direction
        Vector3 startToEnd = lineEnd - lineStart;

        // uses dot product to project circleCenter onto the drag line
        // & finds how far along the line circleCenter projects
        // & returns the distance of projection along the drag line
        float projection = Vector3.Dot(startToCenter, startToEnd.normalized);

        // finds the closest point on the line to the circleCenter
        Vector3 closestPoint;
        // if the projection is behind the start of the line, the closest point would be lineStart
        if (projection <= 0)
        {
            closestPoint = lineStart;
        }
        // if the projection goes past the end of the line, the closest point would be lineEnd
        else if (projection >= startToEnd.magnitude)
        {
            closestPoint = lineEnd;
        }
        // else the closest point is somewhere on the line between start & end
        // therefore, move from lineStart in the line's direction by the projection distance
        else
        {
            closestPoint = lineStart + startToEnd.normalized * projection;
        }

        // calculates the distance between the closestPoint on the line & the circleCenter
        float distance = Vector3.Distance(closestPoint, circleCenter);

        // checks if the distance is small enough to be in the circle's radius of the GO
        if (distance <= circleRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Converts mouse position to world coordinates.
    /// </summary>
    private Vector3 GetMouseWorldPosition()
    {
        // converts screen point to world point
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f; // defualts z-value to 0 ALWAYS

        return worldPosition;
    }
}
