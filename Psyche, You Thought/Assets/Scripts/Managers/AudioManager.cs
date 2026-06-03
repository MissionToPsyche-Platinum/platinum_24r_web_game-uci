using System;
using UnityEngine;

/// <summary>
/// Persistent audio manager. 
/// Master volume scales both sources multiplicatively.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private float _masterVolume = 1f;
    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;
    [Header("Music")]
    [SerializeField] public AudioClip startMenuMusic;
    [SerializeField] public AudioClip mainGameMusic;
    [SerializeField] public AudioClip endSceneMusic;
    [SerializeField] public AudioClip flappyBirdMusic;
    [SerializeField] public AudioClip matchCardsMusic;
    [SerializeField] public AudioClip rhythmQTEMusic;
    [SerializeField] public AudioClip rollingDiceMusic;
    [SerializeField] public AudioClip spaceInvadersMusic;
    [SerializeField] public AudioClip whackAMoleMusic;

    [Header("Core SFX")]
    [SerializeField] public AudioClip clickButtonSFX;
    // [SerializeField] public AudioClip selectSFX;
    [SerializeField] public AudioClip cardDealSFX;
    [SerializeField] public AudioClip nextRoundSFX;
    [SerializeField] public AudioClip storyOpenSFX;
    [SerializeField] public AudioClip popUpSFX;

    [SerializeField] public AudioClip alienGreetSFX;
    [SerializeField] public AudioClip alienLeaveSFX;

    [Header("Flappy Bird SFX")]
    [SerializeField] public AudioClip flapSFX;

    [SerializeField] public AudioClip fallSFX;

    [Header("Match Cards SFX")]
    [SerializeField] public AudioClip cardFlipSFX;
    [SerializeField] public AudioClip matchMadeSFX;
    [SerializeField] public AudioClip lifeLostSFX;

    [Header("Rhythm QTE SFX")]
    [SerializeField] public AudioClip notePressedSFX;

    [Header("Rolling Dice SFX")]
    [SerializeField] public AudioClip diceRollSFX;
    [SerializeField] public AudioClip dealDamageSFX;

    [Header("Space Invaders SFX")]

    [SerializeField] public AudioClip shootSFX;
    [SerializeField] public AudioClip hitAlienSFX;

    [Header("Whack A Mole SFX")]
    [SerializeField] public AudioClip alienHideSFX;
    [SerializeField] public AudioClip whackAlienSFX;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnPhaseChanged += HandlePhase;
        GameManager.OnJudgingOpen  += HandleJudgingOpen;
        GameManager.OnJudgingClose += HandleJudgingClose;
        GameManager.OnScoreAwarded += HandleScoreAwarded;
    }

    private void OnDisable()
    {
        GameManager.OnPhaseChanged -= HandlePhase;
        GameManager.OnJudgingOpen  -= HandleJudgingOpen;
        GameManager.OnJudgingClose -= HandleJudgingClose;
        GameManager.OnScoreAwarded -= HandleScoreAwarded;
    }

     private void HandlePhase(RoundPhase phase)
    {
        if (phase == RoundPhase.DealCards)
            PlaySfx(cardDealSFX);

        if (phase == RoundPhase.RunMinigame)
            StopMusic();
    }

    private void HandleJudgingOpen(PromptCard prompt)
    {
        PlaySfx(alienGreetSFX);
    }

    private void HandleJudgingClose()
    {
        PlaySfx(alienLeaveSFX);
    }

    private void HandleScoreAwarded(Player winner, int newScore)
    {
        PlaySfx(nextRoundSFX);
    }

    public void SetMasterVolume(float value)
    {
        _masterVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetSfxVolume(float value)
    {
        _sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = _masterVolume * _musicVolume;
        if (sfxSource != null) sfxSource.volume = _masterVolume * _sfxVolume;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        if (musicSource.clip == clip) return; // already playing this track
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip, _masterVolume * _sfxVolume);
    }
}