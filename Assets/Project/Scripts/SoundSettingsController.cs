using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundSettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Toggle _soundMuteToggle;

    private void OnEnable()
    {
        _soundMuteToggle.onValueChanged.AddListener(SetSoundMuted);
    }

    private void OnDisable()
    {
        _soundMuteToggle.onValueChanged.RemoveAllListeners();
    }

    private void Start()
    {
        _soundMuteToggle.isOn = SoundSettings.IsSoundMuted;
    }

    private void SetSoundMuted(bool state)
    {
        SoundSettings.SetSoundMuted(state);
        _audioMixer.SetFloat("SoundVol", state? -80f : 0);
    }
}
