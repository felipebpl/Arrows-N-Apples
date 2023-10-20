using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] Sounds;

    private string _musicPlaying; 
    
    // Start is called before the first frame update
    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.ignoreListenerPause = s.ignoreListenerPause;
        }   
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        
        s.source.Play();
    }

    public void PlayClipAtGameObject(string name, GameObject gameObject, bool loop, float minDist, float maxDist)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.loop = loop;
        s.source.spatialBlend = 1;
        s.source.rolloffMode = AudioRolloffMode.Linear;
        s.source.minDistance = minDist;
        s.source.maxDistance = maxDist;
        s.source.Play();
    }
    
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.PlayOneShot(s.clip);
    }

    public void PlaySong(string name)
    {
        if (_musicPlaying != "")
        {
            if (IsSoundPlaying(_musicPlaying) && !_musicPlaying.Equals(name)) //if song is playing and it's not the same song
            {
                Stop(_musicPlaying);
            }  
        }

        _musicPlaying = name;
        Play(name);
    }
    
    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void PlayRandomBetweenSounds(params string[] names)
    {
        if (names.Length == 0)
        { return;}

        Play(names[Random.Range(0, names.Length)]);
    }
    
    public void PlayOneShotRandomBetweenSounds(string[] names)
    {
        PlayOneShot(names[Random.Range(0, names.Length)]);
    }
    
    public bool IsSoundPlaying(string soundName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);

        if (s == null)
        {
            return false;
        }

        return s.source.isPlaying;
    }

    public void SetSoundVolume(string soundName, float volume)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);

        s.source.volume = volume;
    }

    public float GetAudioLength(string soundName)
    {
        Sound s = Array.Find(Sounds, sound => sound.name == soundName);

        return s.source.clip.length;
    }

    public void PlaySoundAfterAnother(string firstSoundName, string secondSoundName, float pauseDuration)
    {
        Play(firstSoundName);
        StartCoroutine(PlaySoundAfterSeconds(secondSoundName, pauseDuration));
    }
    public void PlaySoundAfterAnotherArray(float pauseDuration, params string[] soundNames)
    {
        StartCoroutine(PlaySoundAfterSeconds(pauseDuration, soundNames));
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator PlaySoundAfterSeconds(string soundName, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instance.Play(soundName);
    }

    public static IEnumerator PlaySoundAfterSeconds(float delay, string[] soundName)
    {
        float d = Instance.GetAudioLength(soundName[0]);
        Instance.Play(soundName[0]);

        for (int i = 1; i < soundName.Length; i++)
        {
            yield return new WaitForSeconds(d + delay);
            Instance.Play(soundName[i]);
            d = Instance.GetAudioLength(soundName[i]);
        }
    }
}
