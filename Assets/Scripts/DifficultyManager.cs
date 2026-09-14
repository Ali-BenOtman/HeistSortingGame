// DifficultyManager.cs
// Controls how the game gets harder over time

using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Difficulty Settings")]
    public float difficultyIncreaseInterval = 10f;
    public float minTimerCap = 5f;
    public float timerCapReduceAmount = 0.5f;
    public float maxSpawnDelay = 0.5f;
    public float minSpawnDelay = 0.1f;
    public float spawnDelayReduceAmount = 0.05f;

    private float timeSinceLastIncrease = 0f;
    private int difficultyLevel = 0;
    private float currentSpawnDelay = 0.5f;

    [Header("References")]
    public TimerSystem timerSystem;

    private bool isRunning = false;

    void Update()
    {
        if (!isRunning) return;

        timeSinceLastIncrease += Time.deltaTime;

        if (timeSinceLastIncrease >= difficultyIncreaseInterval)
        {
            timeSinceLastIncrease = 0f;
            IncreaseDifficulty();
        }
    }

    public void StartDifficulty()
    {
        isRunning = true;
        difficultyLevel = 0;
        timeSinceLastIncrease = 0f;
        currentSpawnDelay = maxSpawnDelay;
    }

    public void StopDifficulty()
    {
        isRunning = false;
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;
        Debug.Log("Difficulty Level: " + difficultyLevel);

        // Shrink the timer cap
        if (timerSystem != null)
        {
            timerSystem.maxTimeCap = Mathf.Max(
                minTimerCap,
                timerSystem.maxTimeCap - timerCapReduceAmount
            );
        }

        // Increase spawn speed
        currentSpawnDelay = Mathf.Max(
            minSpawnDelay,
            currentSpawnDelay - spawnDelayReduceAmount
        );
    }

    public float GetCurrentSpawnDelay()
    {
        return currentSpawnDelay;
    }

    public int GetDifficultyLevel()
    {
        return difficultyLevel;
    }
}