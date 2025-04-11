using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AudioManager : BehaviourSingleton<AudioManager>
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _bgmMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private List<AudioClip> TreeAudioList;
    [SerializeField] private List<AudioClip> RockAudioList;
    [SerializeField] private List<AudioClip> UnitAudioList;
    [SerializeField] private List<AudioClip> EnemyAudioList;
    [SerializeField] private List<AudioClip> BGMList;

    [Header("Audio Pool")]
    public int poolSize = 20;
    private List<AudioSource> audioSourceList = new List<AudioSource>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(source);
        }
    }

    public void SetBGMVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("BGM", volume);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue <= 0.001f ? 0.001f : sliderValue) * 20;
        _mixer.SetFloat("SFX", volume);
    }

    public void PlayIdleBGM()
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.resource = BGMList[0];
        audioSource.outputAudioMixerGroup = _bgmMixerGroup;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayResourceAudio(ResourceType type, bool isDone)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;
        if (type == ResourceType.Tree)
        {
            if (isDone) audioSource.resource = TreeAudioList[1];
            else audioSource.resource = TreeAudioList[0];
        }
        else if(type == ResourceType.Rock)
        {
            if (isDone) audioSource.resource = RockAudioList[1];
            else audioSource.resource = RockAudioList[0];
        }
        audioSource.Play();
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach(AudioSource source in audioSourceList)
        {
            if (!source.isPlaying) return source;
        }
        return null;
    }
}
