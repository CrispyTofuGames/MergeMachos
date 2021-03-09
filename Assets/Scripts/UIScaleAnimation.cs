using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIScaleAnimation : MonoBehaviour
{
    [SerializeField]
    AnimationCurve _animationCurve;
    RectTransform _rectTransform;
    [SerializeField]
    float _time;
    float _elapsedTime;
    [SerializeField]
    float _scaleAmount;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        _elapsedTime +=Time.deltaTime;
        if (_elapsedTime > _time)
        {
            _elapsedTime = 0f;
            
        }
        _rectTransform.localScale = Vector3.Lerp(Vector3.one * (1 - _scaleAmount), Vector3.one * (1 + _scaleAmount), _animationCurve.Evaluate(_elapsedTime / _time));
    }
}
