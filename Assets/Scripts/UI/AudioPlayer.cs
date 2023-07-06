using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource sfxSource = null;

    [Header("audio clips")]

    [SerializeField] private AudioClip playingBGMusic = null;
    [SerializeField] private AudioClip stopWheelBGMusic = null;
    [SerializeField] private AudioClip hitSingleTokenSFX = null;
    [SerializeField] private AudioClip correctStreak = null;
    [SerializeField] private AudioClip wrongStreak = null;
    [SerializeField] private AudioClip levelUp = null;
    [SerializeField] private AudioClip playerSnapshot = null;
    [SerializeField] private AudioClip rivalSnapshot = null;

    [Header("settings")]
    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private float sfxVolume = 1.0f;

    private void OnEnable()
    {
        EventManager.OnTakeSnapshot += PlaySnapshot;
    }

    private void OnDisable()
    {
        EventManager.OnTakeSnapshot -= PlaySnapshot;
    }

    private void Awake()
    {
        if (musicSource == null && !this.gameObject.TryGetComponent(out musicSource))
        {
            Debug.LogError("no audio source found in Audio Manager");
        }
        else
        {
            musicSource.clip = null;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null && !this.gameObject.TryGetComponent(out sfxSource))
        {
            Debug.LogError("no audio source found in Audio Manager");
        }
        else
        {
            sfxSource.clip = null;
            sfxSource.playOnAwake = false;
        }
    }



    private void PlaySnapshot(bool isPlayer)
    {
        PlaySFX(isPlayer ? playerSnapshot : rivalSnapshot);

    }



    private void PlaySFX(AudioClip _clip)
    {
        if (sfxSource)
        {
            sfxSource.Stop();
            sfxSource.volume = sfxVolume;
            if (_clip)
            {
                sfxSource.clip = _clip;
                sfxSource.Play();
            }
        }
    }
    private void PlaMusic(AudioClip _clip)
    {
        if (musicSource)
        {
            musicSource.Stop();
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            if (_clip)
            {
                musicSource.clip = _clip;
                musicSource.Play();
            }
        }
    }

}
