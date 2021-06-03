using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Credit to Brackeys for original/base implementation: https://www.youtube.com/watch?v=6OT43pvUyfY
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixerGroup musicMixerGroup;
    private float musicVolume;

    public Sound[] sounds;

    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    Dictionary<string, Sound> soundLibrary;



    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        soundLibrary = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioGroup;
            soundLibrary.Add(s.name, s);
        }
    }

    private void Start()
    {
        Play("Park Music");
    }

    public void Play(string name)
    {
      //  Sound s = Array.Find(sounds, sound => sound.name == name);

        if (!soundLibrary.ContainsKey(name))
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        soundLibrary[name].source.Play();
        //  if (s == null)
        //  {
        //      Debug.LogWarning("Sound: " + name + " not found!");
        //      return;
        //  }
        //
        //  s.source.Play();
    }

    public void ForceMuteMusic()
    {
        musicMixerGroup.audioMixer.GetFloat("Music", out musicVolume);
        musicMixerGroup.audioMixer.SetFloat("Music", -80f);
    }

    public void RestoreMusic()
    {
        musicMixerGroup.audioMixer.SetFloat("Music", musicVolume);
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public AudioMixerGroup audioGroup;
}

