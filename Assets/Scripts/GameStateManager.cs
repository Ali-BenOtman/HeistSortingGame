// GameStateManager.cs
// Controls the game states - Idle, Playing, Game Over

using UnityEngine;
using UnityEngine.Events;

public enum GameState { Idle, Playing, GameOver }

public class GameStateManager : MonoBehaviour
{
    public GameState currentState = GameState.Idle;

    [Header("References")]
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;

    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onGameOver;

    public void StartGame()
    {
        if (currentState == GameState.Playing) return;
        

        currentState = GameState.Playing;
        scoreSystem.ResetScore();
        timerSystem.StartTimer();
        onGameStart.Invoke();
        Debug.Log("Game Started!");
    }

    public void TriggerGameOver()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.GameOver;
        onGameOver.Invoke();
        Debug.Log("Game Over!");
    }

    public bool IsPlaying()
    {
        return currentState == GameState.Playing;
    }
}