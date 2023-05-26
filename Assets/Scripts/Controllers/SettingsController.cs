using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer effectsMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Toggle easyToggle;
    [SerializeField] private Toggle normalToggle;
    [SerializeField] private Toggle hardToggle;
    void Start()
    {
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficulty(Toggle toggle)
    {
    }
    
    public void SetMusicVolume(float desiredVolume)
    {
        musicMixer.SetFloat("MasterVolume", Mathf.Log10(desiredVolume) * 20);
    }
    
    public void SetEffectsVolume(float desiredVolume)
    {
        effectsMixer.SetFloat("MasterVolume", Mathf.Log10(desiredVolume) * 20);
    }

    public void SaveSettings()
    {
        musicMixer.GetFloat("MasterVolume", out var musicVolume);
        PlayerPrefs.SetFloat("mVolume", musicVolume);
        PlayerPrefs.SetFloat("mSlider", musicSlider.value);

        effectsMixer.GetFloat("MasterVolume", out var effectsVolume);
        PlayerPrefs.SetFloat("eVolume", effectsVolume);
        PlayerPrefs.SetFloat("eSlider", effectsSlider.value);
    }

    private void LoadSettings()
    {
        musicMixer.SetFloat("MasterVolume", PlayerPrefs.HasKey("mVolume") 
            ? PlayerPrefs.GetFloat("mVolume") 
            : 0.001f);
        musicSlider.value = PlayerPrefs.GetFloat("mSlider");
        effectsSlider.value = PlayerPrefs.GetFloat("eSlider");
    }
}
