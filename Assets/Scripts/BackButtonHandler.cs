// BackButtonHandler.cs
// Handles the Android back button uniformly across every screen. Unity maps
// Android's back gesture to the Escape key automatically, so this also means
// the Escape key on a keyboard can be used to test this live in the Editor -
// unlike OnApplicationPause, this one doesn't need a real device to verify.
//
// Priority order: close whatever's on top first, then step back one level
// at a time, matching standard back-button conventions.

using UnityEngine;
using UnityEngine.InputSystem;

public class BackButtonHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject settingsPanel;
    public GameObject pausePanel;
    public GameStateManager gameStateManager;
    public SettingsManager settingsManager;
    public PauseManager pauseManager;

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
            return;

        // Settings open (from either Main Menu or Pause) - close it, same as its own Close button.
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsManager.CloseSettings();
            return;
        }

        // Pause panel open - resume, same as its own Resume button. NOT quit -
        // back should step back one level, not jump all the way out of the run.
        if (pausePanel != null && pausePanel.activeSelf)
        {
            pauseManager.ClosePause();
            return;
        }

        // Actively playing, nothing open - open Pause, same as tapping PauseButton.
        if (gameStateManager != null && gameStateManager.IsPlaying())
        {
            pauseManager.OpenPause();
            return;
        }

        // Game Over screen - deliberately does nothing. Restarting should only
        // happen via an explicit tap on Play Again, never an ambiguous back-press.
        if (gameStateManager != null && gameStateManager.currentState == GameState.GameOver)
        {
            return;
        }

        // Main Menu - nothing else open, nowhere further back to go. Exit the app.
        Application.Quit();

#if UNITY_EDITOR
        // Application.Quit() does nothing in the Editor - this makes testing
        // it here actually show something happening instead of silence.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}