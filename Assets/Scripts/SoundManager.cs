using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SoundManager : MonoBehaviour
{
    // The UIScreen Ids currently used in the game
    public const string MainScreenMusicId = "menu";
    public const string TutorialMusicId = "tutorial";
    public const string PHCMusicId = "phc";
    public const string FMIMusicId = "fmi";
    public const string RozlytrekMusicId = "rozlytrek";
    public const string ButtonClickId = "selection";

    public static float fadeDuration = 2f;
    public static float fadeLevel = 1f;

    public static SoundManager instance;
    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.loop = sound.loop;
            if (sound.source.playOnAwake)
            {
                sound.source.Play();
            }
        }
    }

    public void PlayFade(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == soundName);
        if (!s.source.isPlaying)
        {
            StopLoopings();
            StartCoroutine(FadeIn(s.source, fadeDuration));
            //s.source.Play();
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == soundName);
        s.source.Play();
    }

    public void StopLoopings()
    {
        StopAllCoroutines();

        foreach (var sound in sounds)
        {
            if (sound.loop && sound.source.isPlaying)
            {
                //Debug.Log("stopping: " + sound.source.clip.name);
                sound.source.Stop();
            }
        }
    }

    public void MuteAll()
    {
        foreach (var sound in sounds)
        {
            if (sound.loop && sound.source.isPlaying)
            {

                sound.source.volume = 0f;
            }
        }
    }

    public void UnmuteAll()
    {
        foreach (var sound in sounds)
        {
            if (sound.loop && sound.source.isPlaying)
            {

                sound.source.volume = 1f;
            }
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < fadeLevel)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    public void ToggleMusic(bool flag)
    {
        if (!flag)
        {
            StopAllCoroutines();
            MuteAll();
            //StopLoopings();
            fadeDuration = 0f;
            fadeLevel = 0;
        }
        else if (flag)
        {
            fadeLevel = 1;
            fadeDuration = 2f;
            UnmuteAll();
        }

        //Debug.Log("flag: " + flag);
    }
}
