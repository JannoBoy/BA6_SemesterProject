using UnityEngine;
[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;
    public bool playOnAwake;
    public bool loop;

    public AudioSource source;
}
