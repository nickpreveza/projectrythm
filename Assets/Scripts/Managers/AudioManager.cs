using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
/// <summary>
/// Four built-in functions. Play, Stop, Pause and Unpause. Requires the string of the target sound clip for all.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public List<AudioSource> audioSourceList = new List<AudioSource>();

    //audio adjuster variables
    public static AudioSource levelMusicSource;
    float updateStep = 0.001f;
    int sampleDataLength = 2048;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    float sizeFactor = 1;

    float minSize = 0;
    float maxSize = 1000;

    public static float targetMissSoundVolume;
    public float targetMissSoundAdjuster;
    public static float alrightHitSoundVolume;
    public float alrightHitSoundAdjuster;
    public static float perfectHitSoundVolume;
    public float perfectHitSoundAdjuster;
    public static float explosionSoundVolume;
    public float explosionSoundAdjuster;
    public static float crumblingSoundVolume;
    public float crumblingSoundAdjuster;
    public static float meteorGrabSoundVolume;
    public float meteorGrabSoundAdjuster;
    public static float meteorWhooshSoundVolume;
    public float meteorWhooshSoundAdjuster;

    public static AudioManager Instance;
    public AudioMixer masterMixer;


    public float time_before_starting;
    public float max_volume;
    public float rate_of_fading_per_frame = 0.0001f;
    public static string Starting_Music;
   public float time_of_music;
    public Sound[] sounds;
    bool turned_on = false;
    void Awake()
    {
        clipSampleData = new float[sampleDataLength];

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        GameObject audioSources = new GameObject { name = "Audio Sources" };
        audioSources.transform.SetParent(transform);
        foreach (var s in sounds)
        {
            s.source = audioSources.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.mute = s.mute;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;

        }

        // DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        Play(Starting_Music);
    }


    public void StopLayers()
    {

        Stop(Starting_Music);
        Stop("layer_two");
        Stop("layer_three");
        Stop("layer_four");
        Stop("layer_five");

        Stop("pauseTheme");
        levelMusicSource.Stop();
    }
    public void PauseLayers()
    {
        Pause(Starting_Music);
        Pause("layer_two");
        Pause("layer_three");
        Pause("layer_four");
        Pause("layer_five");

        Play("pauseTheme");
        levelMusicSource.Pause();
    }

    public void UnpauseLayers()
    {
        levelMusicSource.Play();
        switch (ScoreManager.Instance.currentLayer)
        {
            case 1:
                Play(Starting_Music);
                break;
            case 2:
                Play(Starting_Music);
                Play("layer_two");
                break;
            case 3:
                Play(Starting_Music);
                Play("layer_two");
                Play("layer_three");

                break;
            case 4:
                Play(Starting_Music);
                Play("layer_two");
                Play("layer_three");
                Play("layer_four");
                break;
            case 5:
                Play(Starting_Music);
                Play("layer_two");
                Play("layer_three");
                Play("layer_four");
                Play("layer_five");
                break;
        }

        Stop("pauseTheme");
    }

    private void Update()
    {
        if (GameManager.Instance.gameHasStarted)
        {
            clipLoudness = GetCurrentTrackLoudness();

           
        }
        else
        {
            clipLoudness = 0.7f;
        }

        targetMissSoundVolume = clipLoudness * targetMissSoundAdjuster;
        alrightHitSoundVolume = clipLoudness * alrightHitSoundAdjuster;
        perfectHitSoundVolume = clipLoudness * perfectHitSoundAdjuster;
        explosionSoundVolume = clipLoudness * explosionSoundAdjuster;
        crumblingSoundVolume = clipLoudness * crumblingSoundAdjuster;
        meteorGrabSoundVolume = clipLoudness * meteorGrabSoundAdjuster;
        meteorWhooshSoundVolume = clipLoudness * meteorWhooshSoundAdjuster;


        if (GameManager.Instance.gameHasStarted)
        {
            time_of_music = Time(Starting_Music);
            if(time_of_music >= Length(Starting_Music))
            {
                GameManager.Instance.EndRound();

                
     
            }
            //Play("layer_two");
            //Play("layer_three");
            //Play("layer_four");
            //Play("layer_five");
            //turned_on = true;
        }


        if (ScoreManager.Instance != null && !GameManager.Instance.is_paused)
        {
            switch (ScoreManager.Instance.currentLayer)
            {
                case 1:
                    Fade_out("layer_two");
                    Fade_out("layer_three");
                    Fade_out("layer_four");
                    break;
                case 2:
                    Fade_in("layer_two");
                    Fade_out("layer_three");
                    Fade_out("layer_four");
                    break;
                case 3:
                    Fade_in("layer_two");
                    Fade_in("layer_three");
                    Fade_out("layer_four");
                    break;
                case 4:
                    Fade_in("layer_two");
                    Fade_in("layer_three");
                    Fade_in("layer_four");
                    break;
                case 5:

                    Fade_in("layer_two");
                    Fade_in("layer_three");
                    Fade_in("layer_four");
                    break;
            }
        }

    }


    /// <summary>
    /// <paramref name="soundName"/> is case sensitive!
    /// </summary>
    /// <param name="soundName"></param>
    /// 


    public float Length(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be stopped."); return 0;
        }
        return s.clip.length;
    }
    public float Time(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be stopped."); return 0; 
        }
        return s.source.time;
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be played."); return;
        }

        if (s.clipVariants.Length > 1)
        {
            s.clip = s.clipVariants[UnityEngine.Random.Range(0, s.clipVariants.Length)];
            s.source.clip = s.clip;
        }

       
        s.source.Play();
    }

    public void Play(string soundName, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be played."); return;
        }

        if (s.clipVariants.Length > 1)
        {
            s.clip = s.clipVariants[UnityEngine.Random.Range(0, s.clipVariants.Length)];
            s.source.clip = s.clip;
        }

        s.source.pitch = pitch;
        s.source.Play();
    }
    public void Play(string soundName, float pitch, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be played."); return;
        }

        if (s.clipVariants.Length > 1)
        {
            s.clip = s.clipVariants[UnityEngine.Random.Range(0, s.clipVariants.Length)];
            s.source.clip = s.clip;
        }

        s.source.pitch = pitch;
        s.source.volume = volume;
        s.source.Play();
    }

    public void PlayRandomExplosionAudioSource(float pitch, float volume)
    {
        for (int i = 0; i < audioSourceList.Capacity; i++)
        {
            if (!audioSourceList[i].isPlaying)
            {
                audioSourceList[i].pitch = pitch;
                audioSourceList[i].volume = volume;
                audioSourceList[i].Play();
                break;
            }
        }
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be stopped."); return;
        }
        s.source.Stop();
    }

    public void Fade_in(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be stopped."); return;
        }
        if(s.source.volume<max_volume)
        {

        s.source.volume += rate_of_fading_per_frame;
        }
    }
    public void Fade_out(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be stopped."); return;
        }
        if (s.source.volume > 0)
        {

            s.source.volume -= rate_of_fading_per_frame;
        }
    }

    public void Pause(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be paused."); return;
        }
        s.source.Pause();
    }

    public void Unpause(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log($"The sound called ''{soundName}'' is not found and cannot be unpaused."); return;
        }
        s.source.UnPause();
    }

    public static float RandomPitch(float minPitch, float maxPitch)
    {
        return UnityEngine.Random.Range(minPitch, maxPitch);
    }

    public float GetCurrentTrackLoudness()
    {
        currentUpdateTime += UnityEngine.Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            if (levelMusicSource == null)
            {
                return 0;
            }
            levelMusicSource.clip.GetData(clipSampleData, levelMusicSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);
        }
        if (clipLoudness <= 0.01f)
        {
            clipLoudness = 0.01f;
        }
        return clipLoudness;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioClip[] clipVariants;
    public AudioMixerGroup mixerGroup;
    [Range(0, 1)] public float volume = 0.5f;
    [Range(0, 3)] public float pitch = 1f;
    public bool loop = false;
    public bool mute = false;
    public bool playOnAwake = false;
    [HideInInspector] public AudioSource source;
}

