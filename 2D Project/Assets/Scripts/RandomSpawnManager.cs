using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Prefab references to RandomShapes.")]
    public GameObject[] shapePrefabs;

    [Header("Colors")]
    [Tooltip("Color options for random selection.")]
    public Color[] availableColors;

    [Header("Scale")]
    [Tooltip("Min & max scale for spawned objects.")]
    public float minScale;
    public float maxScale;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float[] prefabProbabilities = { 0.35f, 0.25f, 0.15f, 0.15f, 0.10f };
    private float[] colorProbabilities = { 0.5f, 0.3f, 0.2f };

    /// <summary>
    /// Selects a RandomShapes prefab based on the probabilities.
    /// </summary>
    private GameObject PickRandomObject()
    {
        float randomValue = Random.value;
        float cumulative = 0f;

        // loops through prefabProbabilities, adding them up until they are above the randomValue
        for (int i = 0; i < prefabProbabilities.Length; i++)
        {
            cumulative += prefabProbabilities[i];
            if (randomValue <= cumulative)
            {
                return shapePrefabs[i];
            }
        }

        // this will never be called, but satisfies "all posibilties"
        return null;
    }

    /// <summary>
    /// Selects a random color based on the probabilities.
    /// </summary>
    private Color PickRandomColor()
    {
        float randomValue = Random.value;
        float cumulative = 0f;

        // loops through colorProbabilities, adding them up until they are above the randomValue
        for (int i = 0; i < colorProbabilities.Length; i++)
        {
            cumulative += colorProbabilities[i];
            if (randomValue <= cumulative)
            {
                return availableColors[i];
            }
        }

        // this will never be called, but satisfies "all posibilties"
        return Color.white;
    }

    /// <summary>
    /// Generates a random rotation between 0-360 degrees.
    /// </summary>
    private Quaternion PickRandomRotation()
    {
        float randomAngle = Random.Range(0f, 360f);
        return Quaternion.Euler(0f, 0f, randomAngle);
    }

    /// <summary>
    /// Generates a random scale.
    /// </summary>
    private Vector3 PickRandomScale()
    {
        float randomScale = Random.Range(minScale, maxScale);
        return new Vector3(randomScale, randomScale, 0f);
    }

    /// <summary>
    /// Spawns a RandomShapes prefab at the specified position.
    /// </summary>
    public void Spawn(Vector3 position)
    {
        // gets random properties
        GameObject selectedPrefab = PickRandomObject();
        Color selectedColor = PickRandomColor();
        Quaternion selectedRotation = PickRandomRotation();
        Vector3 selectedScale = PickRandomScale();

        // instantiates the object
        GameObject spawningObject = Instantiate(selectedPrefab, position, selectedRotation);
        spawningObject.transform.localScale = selectedScale;

        // sets the color
        SpriteRenderer spriteRenderer = spawningObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = selectedColor;

        // stores reference
        spawnedObjects.Add(spawningObject);
    }

    /// <summary>
    /// Destroys all spawnedObjects & clears the list.
    /// </summary>
    public void Despawn()
    {
        foreach (GameObject randomObject in spawnedObjects)
        {
            Destroy(randomObject);
        }

        spawnedObjects.Clear();
    }

    #region --- TESTING ---
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Spawn(Vector3.zero);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Despawn();
        }
    } 
    #endregion
}
