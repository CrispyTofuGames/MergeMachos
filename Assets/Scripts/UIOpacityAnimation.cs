using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIOpacityAnimation : MonoBehaviour
{
    [SerializeField]
    AnimationCurve _animationCurve;
    Image _image;
    [SerializeField]
    float _time;
    float _elapsedTime;
    Color startColor, targetColor;
    void Start()
    {
        _image = GetComponent<Image>();
        targetColor = _image.color;
        startColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    void Update()
    {
        _elapsedTime +=Time.deltaTime;
        if (_elapsedTime > _time)
        {
            _elapsedTime = 0f;
        }
        _image.color = Color.Lerp(startColor, targetColor, _animationCurve.Evaluate(_elapsedTime / _time));
    }
}
