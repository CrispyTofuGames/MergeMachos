using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _smokePS;
    [SerializeField]
    ParticleSystem _fireWorkPS;
    [SerializeField]
    AnimationCurve _fwAnimationCurve;
    [SerializeField]
    Color[] _colors;

    public void Launch()
    {
        var main = _fireWorkPS.main;
        float min = Random.Range(5f,10f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(min , min + 5f) ;
        main.startColor =new ParticleSystem.MinMaxGradient(_colors[Random.Range(0, _colors.Length)], _colors[Random.Range(0, _colors.Length)]);
        StartCoroutine(CrLaunch());
    }

    IEnumerator CrLaunch()
    {
        Vector3 origin = transform.position;
        Vector3 destiny = origin + Vector3.up * 10f;
        _smokePS.Play();
        bool stopped= false;
        for (float i = 0; i< 0.5f; i += Time.deltaTime)
        {
            yield return null;
            transform.position = Vector3.Lerp(origin, destiny,_fwAnimationCurve.Evaluate(i / 0.5f));
            if (_fwAnimationCurve.Evaluate(i / 0.5f) > 0.75f && ! stopped)
            {
                stopped = true;
                _smokePS.Stop();
            }
        }
        yield return new WaitForSeconds(0.5f);
        _fireWorkPS.Play();
    }
    void Update()
    {

    }
}
