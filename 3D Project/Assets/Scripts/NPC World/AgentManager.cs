using UnityEngine;
using System.Collections;

public class AgentManager : MonoBehaviour
{
    public GameObject wanderingAgentPrefab;
    public Transform broom;
    public Vector3 spawnOffset;
    public int spawnCount = 5;
    public float spawnCooldown = 0.5f;

    [Header("Broom Swing Settings")]
    public float forwardSwingAngle = 45f;
    public float backwardSwingAngle = -45f;
    public float swingDuration = 0.3f;
    public AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float lastSpawnTime = 0f;
    private Coroutine swingCoroutine;
    private Quaternion initialBroomRotation;

    void Start()
    {
        initialBroomRotation = broom.localRotation;
    }

    void Update()
    {
        // checks if user has clicked & if the cooldown has passed
        if (Input.GetMouseButton(0) && Time.time > lastSpawnTime + spawnCooldown)
        {
            SpawnWanderers();
            lastSpawnTime = Time.time;

            // starts broom swing
            if (swingCoroutine == null)
            {
                swingCoroutine = StartCoroutine(SwingBroom());
            }
        }
    }

    void SpawnWanderers()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(wanderingAgentPrefab, broom.position + spawnOffset, Quaternion.identity);
        }
    }

    IEnumerator SwingBroom()
    {
        // swings forward
        float timer = 0f;
        Quaternion forwardRot = initialBroomRotation * Quaternion.Euler(0, 0, forwardSwingAngle);

        while (timer < swingDuration)
        {
            timer += Time.deltaTime;
            float swingTime = swingCurve.Evaluate(timer / swingDuration);
            broom.localRotation = Quaternion.Slerp(initialBroomRotation, forwardRot, swingTime);
            yield return null;
        }

        // swings backward
        timer = 0f;
        Quaternion backwardRot = initialBroomRotation * Quaternion.Euler(0, 0, backwardSwingAngle);

        while (timer < swingDuration)
        {
            timer += Time.deltaTime;
            float swingTime = swingCurve.Evaluate(timer / swingDuration);
            broom.localRotation = Quaternion.Slerp(forwardRot, backwardRot, swingTime);
            yield return null;
        }

        // returns to initial position
        timer = 0f;

        while (timer < swingDuration)
        {
            timer += Time.deltaTime;
            float swingTime = swingCurve.Evaluate(timer / swingDuration);
            broom.localRotation = Quaternion.Slerp(backwardRot, initialBroomRotation, swingTime);
            yield return null;
        }

        // ensures it's back to exact initial rotation & ends coroutine
        broom.localRotation = initialBroomRotation;
        swingCoroutine = null;
    }
}