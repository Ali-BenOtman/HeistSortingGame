// ObstacleManager.cs
// Handles what happens when an obstacle is tapped or swiped

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
            scoreSystem.OnWrongSort();
            scoreSystem.ResetMultiplier();
            ClearCurrentObstacle();
            onObstacleSwiped.Invoke();
            billSpawner.SpawnNextBill();
        }
    }
}