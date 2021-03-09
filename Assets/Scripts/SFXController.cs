using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SFXController : MonoBehaviour
{
    [SerializeField]
    List<AudioSource> _audioSources;
    int _currentAudioSource;
    private void Awake()
    {
        GameEvents.PlaySFX.AddListener(Play);    
    }

    public void Play(string audioClip)
    {
        _audioSources[_currentAudioSource].clip = Resources.Load<AudioClip>("Audio/" + audioClip);
        _audioSources[_currentAudioSource].Play();
        _currentAudioSource++;
        _currentAudioSource = _currentAudioSource % _audioSources.Count;
    }
}
