// SettingsManager.cs
// Opens/closes the Settings panel and connects the mute toggle to AudioManager.
//
// CHANGED: OpenSettings() now forces SettingsPanel to the front (SetAsLastSibling)
// every time it opens. Settings can now be reached from multiple places (Main
// Menu, Pause) that don't share the same Hierarchy order, so without this,
// whichever panel/button happened to sit later in sibling order could sit ON
// TOP of Settings for clicks instead of the other way around.

using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    public GameObject settingsPanel;
    public Toggle muteToggle;

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            settingsPanel.transform.SetAsLastSibling(); // NEW
        }

        if (muteToggle != null && AudioManager.Instance != null)
            muteToggle.isOn = AudioManager.Instance.IsMuted();
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnMuteToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMuted(isOn);
    }
}