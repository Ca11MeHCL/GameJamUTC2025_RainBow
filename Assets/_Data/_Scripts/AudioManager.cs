using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton instance

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip MusicSound;
    public AudioClip PopSound;
    public AudioClip CollectSound;
    public AudioClip SpawnColorSound;
    public AudioClip AttackSound;
    public AudioClip CollectEnergySound;
    public AudioClip ButtonClickSound;

    private bool isMute = false; // Track mute state
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void Start()
    {
        musicSource.clip = MusicSound;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayPopSound()
    {
        sfxSource.PlayOneShot(PopSound);
    }
    public void PlayAttackSound()
    {
        //sfxSource.volume = 0f;
        
        sfxSource.PlayOneShot(AttackSound);
        StartCoroutine(ResetVolumeAfterPlay());
    }

    private IEnumerator ResetVolumeAfterPlay()
    {
        yield return new WaitForSeconds(AttackSound.length);
        sfxSource.volume = 1f; 
    }
    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(ButtonClickSound);
    }

    public void Mute()
    {
        isMute = !isMute;
        
        musicSource.mute = isMute;
        sfxSource.mute = isMute;
        
    }
}