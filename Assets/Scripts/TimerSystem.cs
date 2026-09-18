// TimerSystem.cs
// Countdown timer - adds time on correct sort, subtracts on wrong, ends game at zero.
//
// CHANGED from the original:
// - startingTime + maxTimeCap merged into one initialMaxTime, so they can never
//   mismatch (the old version started currentTime at 30 with a cap of 15, which
//   meant the very first correct sort of every run slashed the timer in half).
// - AddTime() no longer shrinks the cap itself. Cap-shrinking is now driven
//   entirely by DifficultyManager via SetMaxTime(), based on score - having two
//   separate systems shrink the same cap on two different clocks was the root
//   cause of the difficulty escalating faster than intended.
// - isFrozen is now separate from isRunning, so unfreezing after a game-over
//   can no longer accidentally revive an already-ended timer.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TimerSystem : MonoBehaviour
{
    [Header("Timer Settings")]
    [FormerlySerializedAs("startingTime")]
    [FormerlySerializedAs("maxTimeCap")]
    [Tooltip("Starting time AND starting max cap, kept as one value so they can't mismatch.")]
    public float initialMaxTime = 30f;

    public float correctSortBonus = 2f;
    public float wrongSortPenalty = 8f;

    private float currentTime;
    private float currentMaxCap;
    private bool isRunning = false;
    private bool isFrozen = false;

    [Header("Events")]
    public UnityEvent onTimerEnd;
    public UnityEvent<float> onTimeChanged; // normalized 0-1, same signature as before

    void Update()
    {
        if (!isRunning || isFrozen) return;

        currentTime -= Time.deltaTime;
        NotifyChanged();

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            onTimerEnd.Invoke();
        }
    }

    public void StartTimer()
    {
        currentMaxCap = initialMaxTime;
        currentTime = initialMaxTime;
        isRunning = true;
        isFrozen = false;
        NotifyChanged();
    }

    public void AddTime()
    {
        currentTime = Mathf.Min(currentTime + correctSortBonus, currentMaxCap);
        NotifyChanged();
    }

    public void SubtractTime()
    {
        currentTime = Mathf.Max(0, currentTime - wrongSortPenalty);
        NotifyChanged();
    }

    /// Called by DifficultyManager - the ONLY thing that should shrink the cap now.
    public void SetMaxTime(float newMaxCap)
    {
        currentMaxCap = newMaxCap;
        if (currentTime > currentMaxCap)
            currentTime = currentMaxCap; // clamp immediately, don't let overflow vanish silently later
        NotifyChanged();
    }

    public void FreezeTimer(bool frozen)
    {
        isFrozen = frozen;
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetCurrentMaxCap()
    {
        return currentMaxCap;
    }

    void NotifyChanged()
    {
        if (currentMaxCap > 0)
            onTimeChanged.Invoke(currentTime / currentMaxCap);
    }
}