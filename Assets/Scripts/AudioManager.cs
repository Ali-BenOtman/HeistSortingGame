// AudioManager.cs
// Central audio control. No sound clips wired in yet - this is plumbing so the
// Settings mute toggle has something real to control, and so future correct/
// wrong/game-over sounds have a ready home via PlaySFX().
//
// Self-configuring: adds its own AudioSource components if none are assigned,
// so setup is just "create an empty GameObject, attach this script."

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private const string MutedPrefKey = "AudioMuted";

    [Header("Sources (auto-created if left empty)")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private bool isMuted;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        isMuted = PlayerPrefs.GetInt(MutedPrefKey, 0) == 1;
        ApplyMuteState();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void SetMuted(bool muted)
    {
        isMuted = muted;
        PlayerPrefs.SetInt(MutedPrefKey, muted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMuteState();
    }

    public void ToggleMuted()
    {
        SetMuted(!isMuted);
    }

    void ApplyMuteState()
    {
        if (sfxSource != null) sfxSource.mute = isMuted;
        if (musicSource != null) musicSource.mute = isMuted;
    }

    /// Ready for future hookup - e.g. AudioManager.Instance.PlaySFX(correctSortClip);
    /// No clips exist yet, so nothing calls this today - that's intentional.
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
}