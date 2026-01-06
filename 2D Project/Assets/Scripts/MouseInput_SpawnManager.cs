using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput_SpawnManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the PF_TrianglePointer prefab.")]
    public GameObject trianglePrefab;

    [Header("Values")]
    [Tooltip("List of all the prefab instances.")]
    public List<GameObject> triangleInstanceList = new List<GameObject>();
    [Tooltip("Vector of the prefab's spawning position.")]
    public Vector3 spawnPosition;

    // Update is called once per frame
    void Update()
    {
        // checks for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTriangle();
        }
    }

    /// <summary>
    /// Instantiates a new instance of the PF_TrianglePointer prefab at the mouse's position.
    /// </summary>
    private void SpawnTriangle()
    {
        // gets mouse position
        spawnPosition = Input.mousePosition;

        // converts screen point to world point
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(spawnPosition);
        worldPosition.z = 0f; // defualts z-value to 0 ALWAYS

        // creates prefab at the mouse position (also, spawn position)
        GameObject newTriangle = Instantiate(trianglePrefab, worldPosition, Quaternion.identity);

        // adds the new prefab to the list
        triangleInstanceList.Add(newTriangle);
    }
}
