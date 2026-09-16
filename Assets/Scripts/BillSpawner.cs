// BillSpawner.cs
// Manages the queue of bills and spawns the next one for the player to sort
// Fake bills appear randomly between every 20-30 bills

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

    void Start()
    {
        nextObstacleInterval = Random.Range(20, 31);
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
            scoreSystem.OnCorrectSort((int)currentBillData.value);
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