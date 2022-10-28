using System.Collections;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] musics;
    public Sound[] ambients;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup ambientMixerGroup;
    [SerializeField] float fadeInDuration = 2f;
    [SerializeField] float fadeInDurationSFX = 0.1f;
    [SerializeField] float fadeOutDuration = 1f;
    [SerializeField] float fadeOutDurationSFX = 0.3f;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        /*foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = sfxMixerGroup;
        }*/

        foreach (Sound s in musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = musicMixerGroup;
        }

        foreach (Sound s in ambients)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = ambientMixerGroup;
        }
    }

    public void PlaySound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        //Prevents spamming same SFX, would create performance problems
        AudioSource[] identicalSources = Array.FindAll<AudioSource>(GetComponents<AudioSource>(), x => x.clip == s.clip);
        if(identicalSources.Length > 3) Destroy(identicalSources[0]);

        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.outputAudioMixerGroup = sfxMixerGroup;

        s.source.Play();
        
        StartCoroutine(DestroyOnFinishedClip(s.source));
    }

    IEnumerator DestroyOnFinishedClip(AudioSource source)
    {
        bool isPlaying = true;
        while(isPlaying)
        {
            if(source == null) yield break;
            else isPlaying = source.isPlaying;
            yield return null;
        }

        try{Destroy(source);}
        catch{}
        
        yield break;
    }

    public void PlayMusic (string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayAmbient (string name)
    {
        Sound s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public bool Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) s = Array.Find(musics, sound => sound.name == name);
        if (s == null) s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Clip " + name + " not found!");
            return false;
        }
        try{
            s.source.Stop();
            return true;
        }
        catch{
            return  false;
        }
    }

    public bool IsPlaying(string name)
    {
        bool isPlaying = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) s = Array.Find(musics, sound => sound.name == name);
        if (s == null) s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return false;
        }
        isPlaying = s.source.isPlaying;
        return isPlaying;
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(AudioOptionsManager.sfxVolume) * 20);
        ambientMixerGroup.audioMixer.SetFloat("ambientVolume", Mathf.Log10(AudioOptionsManager.sfxVolume) * 20);
    }

    public IEnumerator FadeInMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null) s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            yield break;
        }
        s.source.Play();
			s.source.volume = 0f;
			while (s.source.volume < s.volume) 
            {
				s.source.volume += Time.deltaTime / fadeInDuration;
				yield return null;
            }
    }

    public IEnumerator FadeOutMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null) s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            yield break;
        }
        float startVolume = s.source.volume;
		while (s.source.volume > 0) {
			s.source.volume -= startVolume * Time.deltaTime / fadeOutDuration;
			yield return null;
		}
		s.source.Stop();
    }

    public IEnumerator FadeInSFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            yield break;
        }
        s.source.Play();
			s.source.volume = 0f;
			while (s.source.volume < s.volume) 
            {
				s.source.volume += Time.deltaTime / fadeInDurationSFX;
				yield return null;
            }
    }

    public IEnumerator FadeOutSFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(Array.FindAll<AudioSource>(GetComponents<AudioSource>(), x => x.clip == s.clip).Length > 1){
            AudioSource[] sourcesPlaying = Array.FindAll<AudioSource>(GetComponents<AudioSource>(), x => x.clip == s.clip);
            for(int i = 0; i < sourcesPlaying.Length - 1; i++){
                sourcesPlaying[i].Stop();
            }
        } 
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            yield break;
        }
        else if(s.source == null) yield break;
        float startVolume = s.source.volume;
        float elapsedTime = 0;
        while (s.source.volume > 0) {
            elapsedTime += Time.deltaTime;
            s.source.volume -= startVolume * Time.deltaTime / fadeOutDurationSFX; //Mathf.Lerp(startVolume, 0, elapsedTime/fadeOutDurationSFX);
            yield return null;
        }
        s.source.Stop();
        
    }
}
