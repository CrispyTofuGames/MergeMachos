using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] _particleSystems;
    int _currentParticleSystem;

    private void Awake()
    {
        GameEvents.EarnMoney.AddListener(PlayParticles);
    }
    public void PlayParticles(GameEvents.MoneyEventData moneyEventData)
    {
        _particleSystems[_currentParticleSystem].transform.position = moneyEventData._position - new Vector3(0, 0.2f, 0);
        _particleSystems[_currentParticleSystem].Play();
        _currentParticleSystem++;
        _currentParticleSystem = _currentParticleSystem % _particleSystems.Length;
    }

}
