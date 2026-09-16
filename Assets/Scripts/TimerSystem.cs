// TimerSystem.cs
// Countdown timer - adds time on correct sort, subtracts on wrong, ends game at zero

using UnityEngine;
using UnityEngine.Events;

public class TimerSystem : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startingTime = 30f;
    public float correctSortBonus = 2f;
    public float wrongSortPenalty = 3f;
    public float maxTimeCap = 15f;
    public float minTimeCap = 5f;
    public float timeCapShrinkRate = 0.1f;

    private float currentTime;
    private float currentMaxCap;
    private bool isRunning = false;

    [Header("Events")]
    public UnityEvent onTimerEnd;
    public UnityEvent<float> onTimeChanged;

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        onTimeChanged.Invoke(currentTime / currentMaxCap);

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            onTimerEnd.Invoke();
        }
    }

    public void StartTimer()
    {
        currentTime = startingTime;
        currentMaxCap = maxTimeCap;
        isRunning = true;
    }

    public void AddTime()
    {
        currentMaxCap = Mathf.Max(minTimeCap, currentMaxCap - timeCapShrinkRate);
        currentTime = Mathf.Min(currentTime + correctSortBonus, currentMaxCap);
    }

    public void SubtractTime()
    {
        currentTime = Mathf.Max(0, currentTime - wrongSortPenalty);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
    public void FreezeTimer(bool frozen)
{
    isRunning = !frozen;
}
}