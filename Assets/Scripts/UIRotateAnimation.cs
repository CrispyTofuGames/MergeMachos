using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateAnimation : MonoBehaviour
{
    AnimationCurve _animationCurve;
    [SerializeField]
    float _rotationRate;
    RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();    
    }

    void Update()
    {

        _rectTransform.Rotate(0,0,_rotationRate*Time.deltaTime); 

    }
}
