// GameStateManager.cs
// Controls the game states - Idle, Playing, GameOver
//
// NEW: QuitToMenu() - a deliberate exit mid-run, distinct from a real Game Over.
// Stops the same gameplay systems (score saved, difficulty/power-ups stopped)
// via its own onQuitToMenu event, but does NOT show GameOverPanel - that event
// routes to MainMenuManager instead, kept separate from onGameOver so a
// voluntary quit never triggers the "you lost" screen.

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
    public UnityEvent onQuitToMenu; // NEW

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

    // NEW
    public void QuitToMenu()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.Idle;
        onQuitToMenu.Invoke();
        Debug.Log("Quit to Menu!");
    }

    public bool IsPlaying()
    {
        return currentState == GameState.Playing;
    }
}