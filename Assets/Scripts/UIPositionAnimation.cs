using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionAnimation : MonoBehaviour
{
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    float _time;
    float _elapsedTime;
    [SerializeField]
    float _instensity;

    Vector3 _initialPos;
    void Start()
    {
        _initialPos = transform.localPosition;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _time)
        {
            _elapsedTime = 0f;

        }

        transform.localPosition = _initialPos + new Vector3(_initialPos.x, _animationCurve.Evaluate(_elapsedTime / _time)*_instensity, 1);
    }
}
