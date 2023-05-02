using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private MixerParameter mixerParameter;

    void Start()
    {
        float audioMixerValue;
        audioMixerGroup.audioMixer.GetFloat(mixerParameter.ToString(),out audioMixerValue);
        slider.value = Mathf.Pow(10, audioMixerValue/20);

        slider.onValueChanged.AddListener((v) => {
            AudioManager.instance.SetMixerVolume(v, mixerParameter);
            audioMixerGroup.audioMixer.SetFloat(mixerParameter.ToString(), Mathf.Log10(v) * 20);
        });
    }
}

public enum MixerParameter
{
    musicVolume,
    sfxVolume,
    ambientVolume,
    masterVolume
}
