// BillSpawner.cs
// Manages the queue of bills and spawns the next one for the player to sort
// Fake bills appear randomly between every 20-30 bills
//
// FIXED: BeginSpawning() now resets ALL per-run state (obstacle timing, repeat-
// bill tracking) every time a new run starts, not just once at scene load.
// Previously these carried over silently between runs - a partially-progressed
// obstacle counter from a quit/aborted run would push the next fake bill way
// earlier than intended in the following run.

using UnityEngine;
using System.Collections;

public class BillSpawner : MonoBehaviour
{
    [Header("Bill Setup")]
    public BillData[] allBills;
    public GameObject billDisplayPrefab;

    [Header("Obstacle Setup")]
    public ObstacleData[] allObstacles;
    private int nextObstacleInterval;

    [Header("References")]
    public ObstacleManager obstacleManager;
    public PowerUpManager powerUpManager;
    public ScoreSystem scoreSystem;
    public TimerSystem timerSystem;

    private GameObject currentBillObject;
    private BillData currentBillData;
    private int billsSinceLastObstacle = 0;
    private bool autoSortActive = false;
    private BillData lastBillData = null;
    private int sameCount = 0;

    // NEW - wire this to GameStateManager's On Game Start () event.
    public void BeginSpawning()
    {
        // Reset per-run state fresh, every run - including the very first one.
        nextObstacleInterval = Random.Range(20, 31);
        billsSinceLastObstacle = 0;
        lastBillData = null;
        sameCount = 0;

        SpawnNextBill();
    }

    public void SpawnNextBill()
    {
        if (currentBillObject != null)
            Destroy(currentBillObject);

        // Check if power up should spawn
        if (powerUpManager != null && powerUpManager.ShouldSpawnPowerUp())
        {
            SpawnPowerUp();
            return;
        }

        billsSinceLastObstacle++;

        // Randomly between 20-30 bills spawn an obstacle
        if (billsSinceLastObstacle >= nextObstacleInterval && allObstacles.Length > 0)
        {
            billsSinceLastObstacle = 0;
            nextObstacleInterval = Random.Range(20, 31);
            SpawnObstacle();
            return;
        }

        // Spawn normal bill
        obstacleManager.ClearCurrentObstacle();
        currentBillData = GetRandomBill();
        currentBillObject = Instantiate(billDisplayPrefab, Vector3.zero, Quaternion.identity);

        BillDisplay display = currentBillObject.GetComponent<BillDisplay>();
        if (display != null)
            display.SetupBill(currentBillData);

        // Auto sort if active
        if (autoSortActive)
        {
            StartCoroutine(AutoSortBill());
        }
    }

    BillData GetRandomBill()
    {
        BillData newBill;
        int attempts = 0;

        do
        {
            newBill = allBills[Random.Range(0, allBills.Length)];
            attempts++;
        }
        while (newBill == lastBillData && sameCount >= 2 && attempts < 10);

        if (newBill == lastBillData)
            sameCount++;
        else
            sameCount = 1;

        lastBillData = newBill;
        return newBill;
    }

    void SpawnObstacle()
    {
        ObstacleData obstacle = allObstacles[Random.Range(0, allObstacles.Length)];
        obstacleManager.SetCurrentObstacle(obstacle);

        currentBillObject = Instantiate(billDisplayPrefab, Vector3.zero, Quaternion.identity);

        BillDisplay display = currentBillObject.GetComponent<BillDisplay>();
        if (display != null)
            display.SetupObstacle(obstacle);

        // If auto sort is active handle obstacle automatically
        if (autoSortActive)
        {
            StartCoroutine(AutoSortBill());
        }
    }

    void SpawnPowerUp()
    {
        PowerUpData powerUp = powerUpManager.GetAndClearPendingPowerUp();
        powerUpManager.SetCurrentPowerUp(powerUp);

        currentBillObject = Instantiate(billDisplayPrefab, Vector3.zero, Quaternion.identity);
        BillDisplay display = currentBillObject.GetComponent<BillDisplay>();
        if (display != null)
            display.SetupPowerUp(powerUp);
    }

    IEnumerator AutoSortBill()
    {
        yield return new WaitForSeconds(0.5f);

        // If there is an obstacle auto tap it
        if (obstacleManager.IsCurrentObstacle())
        {
            obstacleManager.OnTapped();
            yield break;
        }

        if (currentBillData != null)
        {
            scoreSystem.OnCorrectSort(currentBillData.scorePoints);
            timerSystem.AddTime();
            SpawnNextBill();
        }
    }

    public void SetAutoSort(bool active)
    {
        autoSortActive = active;
    }

    public BillData GetCurrentBill()
    {
        return currentBillData;
    }
}