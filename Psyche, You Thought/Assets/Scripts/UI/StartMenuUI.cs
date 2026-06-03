using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StartMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsBackButton;

    [Header("Master Volume")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI masterValueLabel;

    [Header("Sound Effects Volume")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValueLabel;

    [Header("Music Volume")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValueLabel;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Button creditsBackButton;

    private Settings _settings;

    private void Start()
    {
        _settings = Settings.Load();

        startButton.onClick.AddListener(OnStartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        settingsButton.onClick.AddListener(ToggleSettings);
        settingsBackButton.onClick.AddListener(ToggleSettings);
        creditsButton.onClick.AddListener(ToggleCredits);
        creditsBackButton.onClick.AddListener(ToggleCredits);

        InitSlider(masterSlider, _settings.masterVolume, OnMasterChanged);
        InitSlider(sfxSlider, _settings.sfxVolume, OnSfxChanged);
        InitSlider(musicSlider, _settings.musicVolume, OnMusicChanged);

        // Load previous values into AudioManager
        _settings.SetMasterVolume(_settings.masterVolume);
        _settings.SetSfxVolume(_settings.sfxVolume);
        _settings.SetMusicVolume(_settings.musicVolume);

        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        RefreshValueLabels();
    }

    // ── Button Handlers ───────────────────────────────────────────────────────

    private void OnStartClicked()
    {
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.clickButtonSFX);
        _settings.Save();
        GameManager.Instance?.StartGame();
    }

    private void OnQuitClicked()
    {
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.clickButtonSFX);
        Application.Quit();
    }

    private void ToggleSettings()
    {
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.clickButtonSFX);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    private void ToggleCredits()
    {
        AudioManager.Instance?.PlaySfx(AudioManager.Instance?.clickButtonSFX);
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }


    // ── Slider Handlers ───────────────────────────────────────────────────────

    private void OnMasterChanged(float value)
    {
        _settings.SetMasterVolume(value);
        SetLabel(masterValueLabel, value);
    }

    private void OnSfxChanged(float value)
    {
        _settings.SetSfxVolume(value);
        SetLabel(sfxValueLabel, value);
    }

    private void OnMusicChanged(float value)
    {
        _settings.SetMusicVolume(value);
        SetLabel(musicValueLabel, value);
    }

    /// <summary>Configure a slider's range, starting value, and change listener.</summary>
    private static void InitSlider(Slider slider, float initialValue,
                                   UnityEngine.Events.UnityAction<float> onChange)
    {
        if (slider == null) return;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
        slider.value = initialValue;
        slider.onValueChanged.AddListener(onChange);
    }

    private static void SetLabel(TextMeshProUGUI label, float value)
    {
        if (label != null)
            label.text = $"{Mathf.RoundToInt(value * 100f)}";
    }

    private void RefreshValueLabels()
    {
        SetLabel(masterValueLabel, _settings.masterVolume);
        SetLabel(sfxValueLabel,    _settings.sfxVolume);
        SetLabel(musicValueLabel,  _settings.musicVolume);
    }
}