using System.Collections;
using UnityEngine;

/// <summary>
/// Handles ALL spawning & launching of SliceObject GOs.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    public float minSpawnDelay;     // 0.5f - half a second of delay
    public float maxSpawnDelay;     // 2f - two seconds of delay
    public float spawnXRange;       // 8f - +/- unity units
    public float spawnY;            // -6f - unity units

    [Header("Launch")]
    public float minForce;      // 9f - AddForce on rigidbody
    public float maxForce;      // 12f - AddForce on rigidbody
    public float torque;        // 1f - +/- AddTorque on rigidbody

    [Header("Bomb")]
    [Range(0, 1)] public float bombSpawnChance;     // 0.2f - 1/5th chance

    private bool shouldSpawn = true;

    /// <summary>
    /// Begins the spanwing.
    /// </summary>
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Handles the time delay between spawning.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnRoutine()
    {
        while (shouldSpawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            // checks if the game is over or not & spawns random object
            if (FruitNinjaManager.Instance.IsGameActive())
            {
                SpawnObject();
            }
        }
    }

    /// <summary>
    /// Handles which objects (Fruits or Bombs) are going to be spawned randomly.
    /// </summary>
    void SpawnObject()
    {
        // generates random position along bottom of the screen
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnY, 0);

        GameObject objectToSpawn;

        // decides whether to spawn bomb or fruit
        if (Random.value < bombSpawnChance && bombPrefab != null)
        {
            // assigns object to bomb & plays bomb throw sound effect
            objectToSpawn = bombPrefab;
            FruitNinjaManager.Instance.PlayBombThrowSound();
        }
        else
        {
            // assigns object to one of the fruits randomly & plays fruit throw sound effect
            objectToSpawn = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            FruitNinjaManager.Instance.PlayFruitThrowSound();
        }

        // instantiates the random object & launches it into the scene with random force & torque
        GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // gets the rigidbody 
        Rigidbody2D rBody = newObject.GetComponent<Rigidbody2D>();

        // generates random upward force
        float totalForce = Random.Range(minForce, maxForce);
        rBody.AddForce(Vector2.up * totalForce, ForceMode2D.Impulse);       // ForceMode2D.Impulse allows for instantaneous application & mass-dependency

        // generates rotation torque
        float totalTorque = Random.Range(-torque, torque);
        rBody.AddTorque(totalTorque, ForceMode2D.Impulse);      // ForceMode2D.Impulse allows for instantaneous application & mass-dependency
    }

    /// <summary>
    /// Stops the spawning process when game ends.
    /// </summary>
    public void StopSpawning()
    {
        shouldSpawn = false;

        // ensures all other coroutines aren't executing anymore
        StopAllCoroutines();
    }
}