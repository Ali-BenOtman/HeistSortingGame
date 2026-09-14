// ScoreSystem.cs
// Tracks score and multiplier based on correct/wrong sorts
// Advanced: wrong sort drops multiplier one level, speed bonus, multiplier decay

using UnityEngine;
using UnityEngine.Events;

public class ScoreSystem : MonoBehaviour
{
    private int score = 0;
    private int multiplier = 1;
    private int streak = 0;
    private float lastSortTime = 0f;
    private float timeSinceLastSort = 0f;
    private bool doubleMultiplierActive = false;

    [Header("Multiplier Settings")]
    public int maxMultiplier = 8;
    public int streakPerLevel = 3;
    public float speedBonusWindow = 1.5f;

    [Header("Decay Settings")]
    public float decayStartDelay = 3f;
    public float decayInterval = 1f;
    private float decayTimer = 0f;
    private bool isPlaying = false;

    public UnityEvent<int> onScoreChanged;
    public UnityEvent<int> onMultiplierChanged;

    void Update()
    {
        if (!isPlaying) return;

        timeSinceLastSort += Time.deltaTime;

        if (timeSinceLastSort > decayStartDelay)
        {
            decayTimer += Time.deltaTime;

            if (decayTimer >= decayInterval)
            {
                decayTimer = 0f;
                DecayMultiplier();
            }
        }
    }

    void DecayMultiplier()
    {
        if (multiplier > 1)
        {
            multiplier = Mathf.Max(1, multiplier - 1);
            streak = Mathf.Max(0, streak - streakPerLevel);
            Debug.Log("Multiplier decaying: x" + multiplier);
            onMultiplierChanged.Invoke(doubleMultiplierActive ? multiplier * 2 : multiplier);
        }
    }

    public void OnCorrectSort(int billValue)
    {
        timeSinceLastSort = 0f;
        decayTimer = 0f;
        streak++;

        int newMultiplier = Mathf.Min(streak / streakPerLevel + 1, maxMultiplier);
        if (newMultiplier != multiplier)
        {
            multiplier = newMultiplier;
            onMultiplierChanged.Invoke(doubleMultiplierActive ? multiplier * 2 : multiplier);
        }

        int pointsEarned = doubleMultiplierActive ? billValue * multiplier * 2 : billValue * multiplier;
        if (Time.time - lastSortTime < speedBonusWindow && lastSortTime != 0f)
        {
            pointsEarned = Mathf.RoundToInt(pointsEarned * 1.5f);
            Debug.Log("Speed bonus!");
        }

        score += pointsEarned;
        lastSortTime = Time.time;
        onScoreChanged.Invoke(score);
    }

    public void OnWrongSort()
    {
        streak = Mathf.Max(0, streak - streakPerLevel);
        multiplier = Mathf.Max(1, multiplier - 1);
        timeSinceLastSort = 0f;
        decayTimer = 0f;
        lastSortTime = 0f;
        onMultiplierChanged.Invoke(doubleMultiplierActive ? multiplier * 2 : multiplier);
    }

    public void ResetScore()
    {
        score = 0;
        multiplier = 1;
        streak = 0;
        lastSortTime = 0f;
        timeSinceLastSort = 0f;
        decayTimer = 0f;
        isPlaying = false;
        doubleMultiplierActive = false;
        onMultiplierChanged.Invoke(multiplier);
        onScoreChanged.Invoke(score);
    }

    public void StartPlaying()
    {
        isPlaying = true;
        timeSinceLastSort = 0f;
        decayTimer = 0f;
    }

    public void StopPlaying()
    {
        isPlaying = false;
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        streak = 0;
        lastSortTime = 0f;
        onMultiplierChanged.Invoke(multiplier);
    }

    public void SetDoubleMultiplier(bool active)
    {
        doubleMultiplierActive = active;
        onMultiplierChanged.Invoke(active ? multiplier * 2 : multiplier);
    }

    public int GetScore() { return score; }
    public int GetMultiplier() { return multiplier; }
}