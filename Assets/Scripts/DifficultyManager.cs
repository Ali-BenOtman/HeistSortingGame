// DifficultyManager.cs
// Controls how the game gets harder - the timer cap shrinks smoothly using
// the same exponential formula as before.
//
// Driven by TOTAL ACTIONS this run (bills, obstacles, power-ups - correct or
// wrong, all of it) instead of score. Sort count can't be inflated by any
// future points system, so difficulty stays tied to actual player activity.

using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("References")]
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;

    [Header("Timer Cap Curve")]
    [Tooltip("Must match TimerSystem's Initial Max Time.")]
    public float tMaxInitial = 20f;

    [Tooltip("Floor the cap approaches but never crosses.")]
    public float tMin = 5f;

    [Tooltip("Calibrated against total actions, not score. 313 reproduces the " +
             "same curve timing as scaleFactor=8000 did against points.")]
    public float scaleFactor = 313f;

    [Header("Spawn Delay Curve (legacy readout - not currently read by BillSpawner)")]
    public float maxSpawnDelay = 0.5f;
    public float minSpawnDelay = 0.1f;

    private bool isRunning = false;
    private int lastActionCount = -1;
    private float currentSpawnDelay;
    private const int DisplayLevelSteps = 10;

    void Update()
    {
        if (!isRunning || timerSystem == null || scoreSystem == null) return;
        if (timerSystem.IsFrozen()) return;

        int actionCount = scoreSystem.GetTotalActions();
        if (actionCount == lastActionCount) return;
        lastActionCount = actionCount;

        float progress = 1f - Mathf.Exp(-actionCount / scaleFactor);

        timerSystem.SetMaxTime(tMin + (tMaxInitial - tMin) * (1f - progress));
        currentSpawnDelay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, progress);
    }

    public void StartDifficulty()
    {
        isRunning = true;
        lastActionCount = -1;
        currentSpawnDelay = maxSpawnDelay;
    }

    public void StopDifficulty()
    {
        isRunning = false;
    }

    public float GetCurrentSpawnDelay()
    {
        return currentSpawnDelay;
    }

    public int GetDifficultyLevel()
    {
        if (scoreSystem == null) return 0;
        float progress = 1f - Mathf.Exp(-scoreSystem.GetTotalActions() / scaleFactor);
        return Mathf.FloorToInt(progress * DisplayLevelSteps);
    }
}