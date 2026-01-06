using System;
using UnityEngine;

public class LoopingBall_SpawnManager : MonoBehaviour
{
    [Header("GO References")]
    [Tooltip("Prefab of the ball to instantiate.")]
    public GameObject ballPrefab;
    [Tooltip("Current instance of the ball prefab.")]
    public GameObject ballInstance;

    [Header("Settings")]
    [Tooltip("Position where the ball will be spawned.")]
    public Vector3 spawnPosition;

    [Header("Script References")]
    [Tooltip("Reference to the FollowBall script.")]
    public FollowBall followBallScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // spawns the first ball
        SpawnBall();
    }


    // Update is called once per frame
    void Update()
    {
        // ensures the ball GO doesn't exist or has been destroyed
        if (ballInstance == null)
        {
            SpawnBall();
        }
    }

    /// <summary>
    /// Instantiates a new instance of the ball prefab GO at spawn position. 
    /// </summary>
    public void SpawnBall()
    {
        // creates a new ball at spawn position
        ballInstance = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // updates the FollowBall script to follow the new ball GO
        followBallScript.objectToFollow = ballInstance;
    }
}
