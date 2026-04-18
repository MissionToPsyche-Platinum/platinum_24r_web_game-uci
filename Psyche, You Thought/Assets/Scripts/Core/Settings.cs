using UnityEngine;

[System.Serializable]
public class Settings
{
    public float masterVolume = 0.8f;
    public float sfxVolume = 0.8f;
    public float musicVolume = 0.8f;

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        AudioManager.Instance?.SetMasterVolume(masterVolume);
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        AudioManager.Instance?.SetSfxVolume(sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        AudioManager.Instance?.SetMusicVolume(musicVolume);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public static Settings Load()
    {
        return new Settings
        {
            masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.8f),
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.8f),
            musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.8f),
        };
    }
}