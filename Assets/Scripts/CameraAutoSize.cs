using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoSize : MonoBehaviour
{
    Camera _camera;

    public void SetWidth(float targetWidth)
    {
        _camera = GetComponent<Camera>();
        _camera.orthographicSize = (targetWidth / _camera.aspect) / 2f;
    }
}
