using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectionManager : MonoBehaviour
{
    string music;
    string ambient;

    void Start()
    {

        music = "World" + LevelInfo.instance.GetLevelWorldNumber();
        ambient = "Ambient" + LevelInfo.instance.GetLevelWorldNumber();
        
        foreach ( Sound sound in AudioManager.instance.musics)
        {
            if(sound.name != music)
            {
                if(AudioManager.instance.IsPlaying(sound.name))StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
            }
        }

        foreach ( Sound sound in AudioManager.instance.ambients)
        {
            if(sound.name != ambient)
            {
                if(AudioManager.instance.IsPlaying(sound.name))StartCoroutine(AudioManager.instance.FadeOutMusic(sound.name));
            }
        }

        if(!AudioManager.instance.IsPlaying(music))StartCoroutine(AudioManager.instance.FadeInMusic(music));
        if(!AudioManager.instance.IsPlaying(ambient))StartCoroutine(AudioManager.instance.FadeInMusic(ambient));
        if(AudioManager.instance.IsPlaying("Main Theme"))StartCoroutine(AudioManager.instance.FadeOutMusic("Main Theme"));
    }

    public void FadeOutLevelMusic()
    {
        StartCoroutine(AudioManager.instance.FadeOutMusic(music));
        StartCoroutine(AudioManager.instance.FadeOutMusic(ambient));
    }
}


