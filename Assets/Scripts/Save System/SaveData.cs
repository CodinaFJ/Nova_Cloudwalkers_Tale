using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public List<World> worldsWithLevels = new List<World>();
    public List<bool> unlockedWorlds = new List<bool>();
    public List<bool> playedCinematics = new List<bool>();

    public Level activeLevel;
    public World activeWorld;

    public GameSaveData ()
    {
        worldsWithLevels = GameProgressManager.instance.GetWorldsWithLevels();
        unlockedWorlds = GameProgressManager.instance.GetUnlockedWorlds();
        playedCinematics = GameProgressManager.instance.GetPlayedCinematics();

        activeLevel = GameProgressManager.instance.GetActiveLevel();
        activeWorld = GameProgressManager.instance.GetActiveWorld();
    }
}

[System.Serializable]
public class ConfigurationSaveData
{
    // public string language;
    // public Resolution resolution;
    // public bool fullscreen;

    public float MusicMixerValue;    
    public float SfxMixerValue;    
    public float AmbientMixerValue;    

    public ConfigurationSaveData ()
    {
        MusicMixerValue = AudioManager.instance.MusicMixerValue;
        SfxMixerValue = AudioManager.instance.SfxMixerValue;
        AmbientMixerValue = AudioManager.instance.AmbientMixerValue;
    }
}
