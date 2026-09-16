// PowerUpManager.cs
// Handles spawning and activating power ups

using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    [Header("Power Up Settings")]
    public PowerUpData[] allPowerUps;
    public float spawnInterval = 20f;

    [Header("References")]
    public BillSpawner billSpawner;
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;
    public ObstacleManager obstacleManager;

    private bool isRunning = false;
    private float timer = 0f;
    private bool powerUpPending = false;
    private PowerUpData currentPowerUp = null;
    private PowerUpData lastPowerUp = null;
    private bool firstPowerUp = true;
    private float nextSpawnInterval = 45f;

    void Update()
    {
        if (!isRunning) return;

        timer += Time.deltaTime;

        if (timer >= nextSpawnInterval)
        {
            timer = 0f;
            powerUpPending = true;
            firstPowerUp = false;
            nextSpawnInterval = Random.Range(30f, 36f);
        }
    }

    public void StartPowerUps()
    {
        isRunning = true;
        timer = 0f;
        powerUpPending = false;
        firstPowerUp = true;
        nextSpawnInterval = 45f;
    }

    public void StopPowerUps()
    {
        isRunning = false;
        powerUpPending = false;
    }

    public bool ShouldSpawnPowerUp()
    {
        if (obstacleManager.IsCurrentObstacle()) return false;
        return powerUpPending;
    }

    public PowerUpData GetAndClearPendingPowerUp()
    {
        powerUpPending = false;
        currentPowerUp = GetRandomPowerUp();
        return currentPowerUp;
    }

    PowerUpData GetRandomPowerUp()
    {
        PowerUpData newPowerUp;
        int attempts = 0;

        do
        {
            newPowerUp = allPowerUps[Random.Range(0, allPowerUps.Length)];
            attempts++;
        }
        while (newPowerUp == lastPowerUp && attempts < 10);

        lastPowerUp = newPowerUp;
        return newPowerUp;
    }

    public void SetCurrentPowerUp(PowerUpData powerUp)
    {
        currentPowerUp = powerUp;
    }

    public bool IsCurrentPowerUp()
    {
        return currentPowerUp != null;
    }

    public void OnPowerUpTapped()
    {
        if (currentPowerUp == null) return;

        Debug.Log("Power up activated: " + currentPowerUp.powerUpType);

        // Add time bonus for all power ups except Time Freeze
        if (timerSystem != null && currentPowerUp.powerUpType != PowerUpType.TimeFreeze)
            timerSystem.AddTime();

        switch (currentPowerUp.powerUpType)
        {
            case PowerUpType.TimeFreeze:
                StartCoroutine(TimeFreeze(currentPowerUp.duration));
                break;
            case PowerUpType.DoubleMultiplier:
                StartCoroutine(DoubleMultiplier(currentPowerUp.duration));
                break;
            case PowerUpType.AutoSort:
                StartCoroutine(AutoSort(currentPowerUp.duration));
                break;
        }

        currentPowerUp = null;
        billSpawner.SpawnNextBill();
    }

    public void OnPowerUpSwiped()
    {
        currentPowerUp = null;
    }

    IEnumerator TimeFreeze(float duration)
    {
        timerSystem.FreezeTimer(true);
        Debug.Log("Time Freeze activated!");
        yield return new WaitForSeconds(duration);
        timerSystem.FreezeTimer(false);
        timerSystem.AddTime();
        Debug.Log("Time Freeze ended!");
    }

    IEnumerator DoubleMultiplier(float duration)
    {
        scoreSystem.SetDoubleMultiplier(true);
        Debug.Log("Double Multiplier activated!");
        yield return new WaitForSeconds(duration);
        scoreSystem.SetDoubleMultiplier(false);
        Debug.Log("Double Multiplier ended!");
    }

    IEnumerator AutoSort(float duration)
    {
        billSpawner.SetAutoSort(true);
        Debug.Log("Auto Sort activated!");
        yield return new WaitForSeconds(duration);
        billSpawner.SetAutoSort(false);
        Debug.Log("Auto Sort ended!");
    }
}