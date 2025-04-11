using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AudioManager : BehaviourSingleton<AudioManager>
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private List<AudioSource> TreeAudioList;
    [SerializeField] private List<AudioSource> RockAudioList;
    [SerializeField] private List<AudioSource> UnitAudioList;
    [SerializeField] private List<AudioSource> EnemyAudioList;
    [SerializeField] private List<AudioSource> BGMList;

    public void SetBGMVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("SFXVolume", volume);
    }

    public void PlayResourceAudio(ResourceType type, bool isDone)
    {
        if (type == ResourceType.Tree)
        {
            if (isDone) TreeAudioList[1].Play();
            else TreeAudioList[0].Play();
        }
        else if(type == ResourceType.Rock)
        {
            if (isDone) RockAudioList[1].Play();
            else RockAudioList[0].Play();
        }
    }
}
