using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RawImageAspect : MonoBehaviour
{
    RectTransform _rectTransform;
    Camera _camera;
    void Start()
    {
        _camera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.y * _camera.aspect, _rectTransform.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
