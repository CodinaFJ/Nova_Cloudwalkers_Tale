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

    const string PARAMETER_MUSIC = "musicVolume";
    const string PARAMETER_SFX = "sfxVolume";
    const string PARAMETER_AMBIENT = "ambientVolume";

    public static AudioManager instance;
    private float musicMixerValue;
    public float MusicMixerValue 
    { 
        get => musicMixerValue;
        set 
        {
            musicMixerValue = value;
            SetMixerVolume(value, MixerParameter.musicVolume);
        } 
    }
    private float sfxMixerValue;
    public float SfxMixerValue
    { 
        get => sfxMixerValue;
        set 
        {
            sfxMixerValue = value;
            SetMixerVolume(value, MixerParameter.sfxVolume);
        } 
    }
    private float ambientMixerValue;
    public float AmbientMixerValue
    { 
        get => ambientMixerValue;
        set 
        {
            ambientMixerValue = value;
            SetMixerVolume(value, MixerParameter.ambientVolume);
        } 
    }

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
            PrintNotFoundWarning(name);
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
    public void PlaySound(string name, bool avoidMoreThanOne){
        if(IsPlayingSFX(name) && avoidMoreThanOne) return;
        PlaySound(name);
    }
    public void PlaySound(string name, string nameToStop){
        if(IsPlayingSFX(nameToStop)) Stop(nameToStop);
        PlaySound(name);
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
            PrintNotFoundWarning(name);
            return;
        }
        s.source.Play();
    }

    public void PlayAmbient (string name)
    {
        Sound s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            PrintNotFoundWarning(name);
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
            PrintNotFoundWarning(name);
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
            PrintNotFoundWarning(name);
            return false;
        }
        isPlaying = s.source.isPlaying;
        return isPlaying;
    }

    public bool IsPlayingSFX(string name)
    {
        bool isPlaying = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            PrintNotFoundWarning(name);
            return false;
        }
        isPlaying = Array.Exists<AudioSource>(GetComponents<AudioSource>(), x => x.clip == s.clip);
        return isPlaying;
    }

    public IEnumerator FadeInMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null) s = Array.Find(ambients, sound => sound.name == name);
        if (s == null)
        {
            PrintNotFoundWarning(name);
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
            PrintNotFoundWarning(name);
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
            PrintNotFoundWarning(name);
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
            PrintNotFoundWarning(name);
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

    private void PrintNotFoundWarning(string name)
    {
        Debug.LogWarning("Sound " + name + " not found!");
    }

    #region MixerVolumes_Sliders

    private void SetMixerVolume(float volume, MixerParameter mixerParameter)
    {
        Debug.Log("Set mixer volume for parameter: " + mixerParameter.ToString() + " " + volume);
        musicMixerGroup.audioMixer.SetFloat(mixerParameter.ToString(), Mathf.Log10(volume) * 20);
    }

    public void ChangeMixerVolumeFromSlider(float volume, MixerParameter mixerParameter)
    {
        switch (mixerParameter)
        {
            case MixerParameter.musicVolume:
            MusicMixerValue = volume;
            break;

            case MixerParameter.sfxVolume:
            SfxMixerValue = volume;
            break;

            case MixerParameter.ambientVolume:
            AmbientMixerValue = volume;
            break;
        }
    }

    public void LoadMixerVolumes(ConfigurationSaveData configurationSaveData)
    {
        this.MusicMixerValue = configurationSaveData.MusicMixerValue;
        this.SfxMixerValue = configurationSaveData.SfxMixerValue;
        this.AmbientMixerValue = configurationSaveData.AmbientMixerValue;
    }

    #endregion
}
