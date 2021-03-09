using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeBigPhotoInstance : MonoBehaviour
{
    [SerializeField]
    CustomizeController _customizeController;
    [SerializeField]
    Image _background;
    [SerializeField]
    Image[] _seats;
    [SerializeField]
    Image[] _tables;
    [SerializeField]
    Image _frameImage;

    public void LoadConfig(int category, int productIndex)
    {
        //_hardCoinsText.text = _customizeController.GetHardCoinsProductCost(_category, _productIndex).ToString();
        if (category == 0)
        {
            for (int i = 0; i < _seats.Length; i++)
            {
                _seats[i].sprite = Resources.Load<Sprite>("Sprites/Cells/" + productIndex);
                _seats[i].overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + productIndex);
            }
        }
        else if (category == 1)
        {
            for (int i = 0; i < _tables.Length; i++)
            {
                _tables[i].sprite = Resources.Load<Sprite>("Sprites/Expositors/" + productIndex);
                _tables[i].overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + productIndex);
            }
        }
        else if (category == 2)
        {
            _background.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + productIndex);
            _background.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + productIndex);
        }
        else if (category == 3)
        {
            _frameImage.sprite = Resources.Load<Sprite>("Sprites/Frames/" + productIndex);
            _frameImage.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + productIndex);
        }
    }
}
