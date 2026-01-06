using UnityEngine;

/// <summary>
/// Used in class on 11/24 to raise the forklift to the correct height of the Terrain.
/// </summary>
public class ForkliftTerrainHeight : MonoBehaviour
{
    // Terrain data --> Terrain
    public Terrain terrainObj;
    private TerrainData heightmapData;

    // Forklift
    public GameObject forkliftModel;

    // Control script height function
    public bool raycastOn;


    void Start()
    {
        // Use GetHeight method of the terrainData object
        if (!raycastOn)
        {
            // Set the terrain data as a field of the class (shortcut)
            heightmapData = terrainObj.terrainData;

            // Set the height of the forklift
            Vector3 forkliftPosition = forkliftModel.transform.position;        // World units

            // Get the height of terrain at its position
            float heightOnTerrain =
                heightmapData.GetHeight((int)forkliftPosition.x, (int)forkliftPosition.z);

            // Set the y value of the forklift to the height of the terrain
            forkliftModel.transform.position =
                new Vector3(forkliftPosition.x, heightOnTerrain, forkliftPosition.z);
        }
        // Raycast!
        else
        {
            // RaycastHit out object (set via the Raycast)
            RaycastHit hit;

            // Set the height of the forklift
            Vector3 forkliftPosition = forkliftModel.transform.position;        // World units

            // Set the height above the Terrain object
            forkliftPosition.y = 610;                                           // heightmapData.size.y

            // Raycast DOWN from the vehicle's position (floating above terrain) down to Terrain
            Physics.Raycast(forkliftPosition, -forkliftModel.transform.up, out hit);

            // Get the height of the exact spot where the ray hit the Terrain
            float heightOnTerrain = hit.point.y;

            // Set the y value of the forklift to the height of the terrain
            forkliftModel.transform.position =
                new Vector3(forkliftPosition.x, heightOnTerrain, forkliftPosition.z);
        }
    }

    void Update()
    {

    }
}