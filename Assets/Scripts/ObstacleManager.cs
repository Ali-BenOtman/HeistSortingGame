// ObstacleManager.cs
// Handles what happens when an obstacle is tapped or swiped
//
// CHANGED: OnTapped() now counts toward the difficulty driver via
// scoreSystem.CountAction() - correctly tapping a fake bill is still a real
// action taken in the run, same as sorting a bill. OnSwiped() already counted
// automatically as a side effect of calling OnWrongSort() - no change needed there.

using UnityEngine;
using UnityEngine.Events;

public class ObstacleManager : MonoBehaviour
{
    [Header("References")]
    public ScoreSystem scoreSystem;
    public BillSpawner billSpawner;
    public TimerSystem timerSystem;

    private bool currentIsObstacle = false;
    private ObstacleData currentObstacle = null;

    [Header("Events")]
    public UnityEvent onObstacleTapped;
    public UnityEvent onObstacleSwiped;

    public void SetCurrentObstacle(ObstacleData obstacle)
    {
        currentObstacle = obstacle;
        currentIsObstacle = true;
    }

    public void ClearCurrentObstacle()
    {
        currentObstacle = null;
        currentIsObstacle = false;
    }

    public bool IsCurrentObstacle()
    {
        return currentIsObstacle;
    }

    public void OnTapped()
    {
        if (!currentIsObstacle) return;

        if (currentObstacle.obstacleType == ObstacleType.Fake)
        {
            Debug.Log("Fake bill tapped correctly!");
            if (timerSystem != null)
                timerSystem.AddTime();
            if (scoreSystem != null)
                scoreSystem.CountAction(); // NEW
            ClearCurrentObstacle();
            onObstacleTapped.Invoke();
            billSpawner.SpawnNextBill();
        }
    }

    public void OnSwiped()
    {
        if (!currentIsObstacle) return;

        if (currentObstacle.obstacleType == ObstacleType.Fake)
        {
            Debug.Log("Fake bill swiped - full multiplier reset!");
            scoreSystem.OnWrongSort(); // already counts toward totalActions
            scoreSystem.ResetMultiplier();
            ClearCurrentObstacle();
            onObstacleSwiped.Invoke();
            billSpawner.SpawnNextBill();
        }
    }
}