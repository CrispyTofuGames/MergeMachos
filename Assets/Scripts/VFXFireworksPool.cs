using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXFireworksPool : MonoBehaviour
{
    [SerializeField]
    Firework[] _fireworks;
    int _currentFireWork;
    Vector2 xPos;
    Vector2 yPos;
    bool _partying;
    float _elapsedTime;
    float _nextFW;
    [SerializeField]
    ParticleSystem _confetti;
    public void StartTheParty()
    {
        _partying = true;
        _confetti.Play();
    }
    public void StopTheParty()
    {
        _partying = false;
        _confetti.Stop();
    }
    private void Start()
    {
        xPos = new Vector2(-5f, 5f);
        yPos = new Vector2(-20f, 0f);
        _nextFW = Random.Range(1f, 5f);
    }
    public void Launch()
    {
        _fireworks[_currentFireWork].transform.position = new Vector3(Random.Range(xPos.x, xPos.y), Random.Range(yPos.x, yPos.y), 0f);
        _fireworks[_currentFireWork].Launch();
        _currentFireWork++;
        _currentFireWork = _currentFireWork % _fireworks.Length;

    }
    private void Update()
    {
        if (_partying)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _nextFW)
            {
                _elapsedTime = 0f;
                _nextFW = Random.Range(1f, 3f);
                Launch();
            }
        }


    }
}
