using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField] bool shakeAvailable;
    float duration;
    float magnitude;
    float amplitude;
    float stopwatch = 0;
    float a_decrease_per_second;
    float f_decrease_per_second;
    bool on = false;

    CinemachineVirtualCamera vcam;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(on)
        {
            if(stopwatch < duration)
            {

                stopwatch += Time.deltaTime;
                if (vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain > 0)
                {

                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain -= f_decrease_per_second * Time.deltaTime;
                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain -= a_decrease_per_second * Time.deltaTime;
                }
                else
                {

                    vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
                }

            }
            else
            {
                vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
                on = false;
            }
        }
    }

    public void Shake(float _duration,float _magnitude,float _amplitude)
    {
        if (!shakeAvailable)
        {
            return;
        }
        duration = _duration;
        magnitude = _magnitude;
        amplitude = _amplitude;
        on = true;
        stopwatch = 0;
        gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = _magnitude;
        gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        f_decrease_per_second = magnitude / duration;
        a_decrease_per_second = amplitude / duration;
    }
}
