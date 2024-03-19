using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    AudioSource audioSource;
    public float power = 2, launchDelay;
    bool isPlaying;
    Transform mp;
    [SerializeField] float shake_duration = 0.2f;
    [SerializeField] float shake_magnitude = 5f;
    [SerializeField] float shake_amplitude = 1.5f;
    int meteorHits;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 1f * Time.deltaTime, 0.15f, Space.Self);
        audioSource.volume = AudioManager.meteorWhooshSoundVolume;

        //Destroy meteors after 3 buildings
        if(meteorHits >= 3)
        {
            DestroyWithCoolness();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            AudioManager.Instance.Play("explosion", AudioManager.RandomPitch(0.7f, 1.3f), AudioManager.explosionSoundVolume);
            CameraShake.instance.Shake(shake_duration, shake_magnitude,shake_amplitude);
            collision.gameObject.GetComponent<BuildingObject>().DestroyStructure();

            meteorHits++;
        }

        if (collision.CompareTag("Planet"))
        {
            DestroyWithCoolness();
        }
        
    }

    public void DestroyWithCoolness()
    {
        GameObject destroyEffect = Instantiate(GameManager.Instance.destructionParticleEffect, this.transform.position, Quaternion.identity);
        CameraShake.instance.Shake(shake_duration, shake_magnitude, shake_amplitude);
        destroyEffect.transform.parent = null;
        Destroy(this.gameObject);
    }

    public void PlayMeteorWhoosh()
    {
        if (!isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
            isPlaying = true;
        }
    
    }

    public void ThrowMeteor(Transform mousePos)
    {
        mp = mousePos;
        PlayMeteorWhoosh();
        Invoke("Launch", launchDelay);
    }

    public void Launch()
    {
            var direction = (mp.transform.position - transform.position);

            GetComponent<Rigidbody2D>().AddForce(direction * (power * 50));
    }
}
