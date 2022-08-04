using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    //[SerializeField] private TextMeshProUGUI _sliderText;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private string _mixerParameter;

    void Start()
    {
        float audioMixerValue;
        _audioMixerGroup.audioMixer.GetFloat(_mixerParameter,out audioMixerValue);
        _slider.value = Mathf.Pow(10, audioMixerValue/20);

        _slider.onValueChanged.AddListener((v) => {
            //_sliderText.text = ((int)(v*100)).ToString("0");
            _audioMixerGroup.audioMixer.SetFloat(_mixerParameter, Mathf.Log10(v) * 20);
        });
    }

}
