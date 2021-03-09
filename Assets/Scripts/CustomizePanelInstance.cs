using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizePanelInstance : MonoBehaviour
{
    [SerializeField]
    GameObject _leftSignal;
    [SerializeField]
    GameObject _rightSignal;
    [SerializeField]
    RectTransform _products;

    void Update()
    {
        if(_products.childCount > 3)
        {
            if (_products.anchoredPosition.x < 0)
            {
                _leftSignal.SetActive(true);
            }
            else
            {
                _leftSignal.SetActive(false);
            }
            if (_products.anchoredPosition.x > -300 * (_products.childCount - 3))
            {
                _rightSignal.SetActive(true);
            }
            else
            {
                _rightSignal.SetActive(false);
            }
        }
    }

    public RectTransform GetProductsParent()
    {
        return _products;
    }
}
