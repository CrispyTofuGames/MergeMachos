using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _confettiRain;
    [SerializeField] ParticleSystem _confettiExplosion;
    [SerializeField] ParticleSystem _sparks;
    [SerializeField] ParticleSystem _implosion;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] ParticleSystem _capsuleIn;
    [SerializeField] ParticleSystem _godRays;
    [SerializeField] ParticleSystem _dustIn;
    [SerializeField] ParticleSystem _dustOut;
    [SerializeField] ParticleSystem _haloSquares;
    [SerializeField] ParticleSystem _haloExplosion;
    [SerializeField] ParticleSystem _normalHaloBack;
    [SerializeField] ParticleSystem[] _chestVFXs;
    [SerializeField] Image _mergeLeft;
    [SerializeField] Image _mergeRight;
    [SerializeField] Image _finalMerge;

    public void Stop()
    {
        _confettiExplosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _sparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _haloSquares.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _dustOut.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _confettiRain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _haloExplosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _normalHaloBack.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    public void Explode()
    {
        Stop();
        _confettiExplosion.Play();
        _sparks.Play();
        _haloSquares.Play();
        _dustOut.Play();
        _confettiRain.Play();
        _haloExplosion.Play();
        _normalHaloBack.Play();
    }

    public void PlayLootBoxFX(int chestIndex)
    {
        StopLootFx();
        _chestVFXs[chestIndex].Play();
        Explode();
    }

    public void StopLootFx()
    {
        foreach (ParticleSystem p in _chestVFXs)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void PlayMergeAnimation()
    {
        Stop();
        StartCoroutine(CrPlayMergeAnimation());
    }
    IEnumerator CrPlayMergeAnimation()
    {
        _godRays.Play();
        _capsuleIn.Play();
        _dustIn.Play();
        yield return new WaitForSeconds(2f);
        _implosion.Play();
        yield return new WaitForSeconds(0.5f);
        _explosion.Play();
        yield return new WaitForSeconds(2f);
        _explosion.Stop();
        _explosion.Stop();
        _dustIn.Stop();
        _capsuleIn.Stop();
        _godRays.Stop();
        Explode();
    }
}
