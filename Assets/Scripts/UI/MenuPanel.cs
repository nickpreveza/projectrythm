using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class MenuPanel : UIPanel
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider audioSlider;
    [SerializeField] Slider musicSlider;

    public AudioSource level1MusicSource;
    public AudioSource level2MusicSource;

    private void Start()
    {

    }

    public override void Setup()
    {
        base.Setup();
    }

    public override void Activate()
    {
        base.Activate();

    }

    public override void Disable()
    {
        base.Disable();
    }

    public void StartGame(int index)
    {
        switch (index)
        {
            case 0:
                //tutorial;
               // GameManager.Instance.StartGame(index);
                break;
            case 1:
                AudioManager.Starting_Music = "layer_one";
                AudioManager.levelMusicSource = level1MusicSource;
                AudioManager.levelMusicSource.Play();
                GameManager.resultThemeString = "easyResultMusic";
                GameManager.Instance.StartGame(index);
                break;
            case 2:
                AudioManager.Starting_Music = "hardMusic";
                AudioManager.levelMusicSource = level2MusicSource;
                AudioManager.levelMusicSource.Play();
                GameManager.resultThemeString = "hardResultMusic";
                GameManager.Instance.StartGame(index);
             
                //hard
                break;
        }
    }

    public void OpenCredits()
    {
        UIManager.Instance.OpenCreditsPanel();
    }

    public void OpenTutorial()
    {
        UIManager.Instance.OpenOnboarding();
    }


    void UpdateSettings()
    {
        if (AudioManager.Instance != null)
        {
            float masterVolume;
            AudioManager.Instance.masterMixer.GetFloat("master", out masterVolume);
            masterSlider.value = masterVolume;

            float audioVolume;
            AudioManager.Instance.masterMixer.GetFloat("audio", out audioVolume);
            audioSlider.value = audioVolume;

            float musicVolume;
            AudioManager.Instance.masterMixer.GetFloat("music", out musicVolume);
            musicSlider.value = musicVolume;
        }

    }

    public void Exit()
    {
        //UIManager.Instance.popup.Exit();
    }

    public void OpenSettings()
    {

    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance.masterMixer.SetFloat("master", volume);
    }

    public void SetAudiovolume(float volume)
    {
        AudioManager.Instance.masterMixer.SetFloat("audio", volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.masterMixer.SetFloat("music", volume);
    }
}
