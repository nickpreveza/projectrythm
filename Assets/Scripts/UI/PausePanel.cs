using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PausePanel : UIPanel
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider audioSlider;
    [SerializeField] Slider musicSlider;

    private void Start()
    {

    }

    public override void Setup()
    {
        base.Setup();
    }

    public override void Activate()
    {
        DepthOfField dof;
        if (GameManager.Instance.globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            dof.active = true;
        }
        

        base.Activate();

    }

    public override void Disable()
    {
        DepthOfField dof;
        if (GameManager.Instance.globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            dof.active = false;
        }
        base.Disable();
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

    public void Resume()
    {
        GameManager.Instance.SetPause = false;
    }

    public void Restart()
    {
        //someone do this
    }
    public void Exit()
    {
        SaveManager.Instance.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
