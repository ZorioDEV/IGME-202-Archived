using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    public Terrain terrain;
    public float strength = 1f;
    public float timeStep = 0.01f;


    void Start()
    {
        SetTerrainHeight();
    }

    void Update()
    {
        
    }

    public void SetTerrainHeight()
    {
        int heightmapRes = terrain.terrainData.heightmapResolution;
        float[,] heightData = new float[heightmapRes, heightmapRes];
        float xCoord = 0f;
        float yCoord = 0f;

        for (int x = 0; x < heightmapRes; x++)
        {
            for (int y = 0; y < heightmapRes; y++)
            {
                heightData[x, y] = Mathf.PerlinNoise(xCoord, yCoord) * strength;

                xCoord += timeStep;
            }

            xCoord = 0f;
            yCoord += timeStep;
        }

        terrain.terrainData.SetHeights(0, 0, heightData);
    }
}
