// DifficultyManager.cs
// Controls how the game gets harder as SCORE increases (not time elapsed).
//
// CHANGED from the original:
// - Driven by score, not a real-time interval timer - fixes the bug where this
//   script AND TimerSystem.AddTime() were both independently shrinking the same
//   cap on two different clocks.
// - Everything (timer cap, spawn delay, display level) is now a pure function of
//   ONE normalized progress value derived from score - a single source of truth
//   instead of several drifting variables.
// - Pauses itself while TimerSystem is frozen, so the Time Freeze power-up can no
//   longer be undercut by difficulty quietly climbing underneath it.
// - Public API (StartDifficulty, StopDifficulty, GetCurrentSpawnDelay,
//   GetDifficultyLevel) is unchanged, so any existing Inspector event wiring
//   (e.g. GameStateManager.onGameStart) keeps working without edits.

using UnityEngine;
using UnityEngine.Serialization;

public class DifficultyManager : MonoBehaviour
{
    [Header("References")]
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;

    [Header("Timer Cap Curve")]
    [Tooltip("Must match TimerSystem's initialMaxTime.")]
    public float tMaxInitial = 30f;

    [FormerlySerializedAs("minTimerCap")]
    [Tooltip("Floor the cap approaches but never crosses.")]
    public float tMin = 12f;

    [Tooltip("Smaller = cap shrinks sooner/harder. Tune against your average score pace - no direct equivalent existed in the old script, so this needs a fresh value.")]
    public float scaleFactor = 4000f;

    [Header("Spawn Delay Curve (legacy readout - not currently read by BillSpawner)")]
    public float maxSpawnDelay = 0.5f;
    public float minSpawnDelay = 0.1f;

    private bool isRunning = false;
    private int lastScore = -1;
    private float currentSpawnDelay;
    private const int DisplayLevelSteps = 10;

    void Update()
    {
        if (!isRunning || timerSystem == null || scoreSystem == null) return;
        if (timerSystem.IsFrozen()) return; // difficulty pauses while Time Freeze is active

        int score = scoreSystem.GetScore();
        if (score == lastScore) return;
        lastScore = score;

        float progress = 1f - Mathf.Exp(-score / scaleFactor); // 0 -> 1 as score climbs

        timerSystem.SetMaxTime(tMin + (tMaxInitial - tMin) * (1f - progress));
        currentSpawnDelay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, progress);
    }

    public void StartDifficulty()
    {
        isRunning = true;
        lastScore = -1;
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
        float progress = 1f - Mathf.Exp(-scoreSystem.GetScore() / scaleFactor);
        return Mathf.FloorToInt(progress * DisplayLevelSteps);
    }
}