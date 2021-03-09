using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutakuOptionsActivator : MonoBehaviour
{
    [SerializeField]
    GameObject _nutakuOptions;

    private void OnEnable()
    {
        _nutakuOptions.SetActive(true);
    }
    private void OnDisable()
    {
        _nutakuOptions.SetActive(false);
    }
}
