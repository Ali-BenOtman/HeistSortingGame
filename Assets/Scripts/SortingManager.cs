// SortingManager.cs
using UnityEngine;
using UnityEngine.Events;

public class SortingManager : MonoBehaviour
{
    [Header("References")]
    public BillSpawner billSpawner;
    public ScoreSystem scoreSystem;
    public GameStateManager gameStateManager;
    public ObstacleManager obstacleManager;
    public PowerUpManager powerUpManager;

    [Header("Events")]
    public UnityEvent onCorrectSort;
    public UnityEvent onWrongSort;

    public void OnSwipeReceived(SwipeDirection direction)
    {
        if (!gameStateManager.IsPlaying())
            return;

        // If current is obstacle handle it differently
        if (obstacleManager.IsCurrentObstacle())
        {
            obstacleManager.OnSwiped();
            return;
        }

        // If current is power up just skip it
        if (powerUpManager.IsCurrentPowerUp())
        {
            powerUpManager.OnPowerUpSwiped();
            billSpawner.SpawnNextBill();
            return;
        }

        BillData currentBill = billSpawner.GetCurrentBill();

        if (currentBill == null)
            return;

        if (direction == currentBill.correctDirection)
        {
            Debug.Log("Correct sort! Bill: $" + (int)currentBill.value);
            scoreSystem.OnCorrectSort((int)currentBill.value);
            onCorrectSort.Invoke();
        }
        else
        {
            Debug.Log("Wrong sort!");
            scoreSystem.OnWrongSort();
            onWrongSort.Invoke();
        }

        billSpawner.SpawnNextBill();
    }
}