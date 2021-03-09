using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuyFragmentsController : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] Image _productImage;
    [SerializeField] TextMeshProUGUI _fragmentsAmountText;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] Button _buyButton;
    FragmentType _type;
    int _fragmentAmount;
    int _productIndex;
    public void ShowInfo(FragmentType type,int index, int maxAmount)
    {
        _slider.maxValue = maxAmount;
        _productImage.color = Color.white;
        _type = type;
        _productIndex = index;
        switch (type)
        {
            case FragmentType.CellFragments:
                _productImage.sprite = Resources.Load<Sprite>("Sprites/Cells/" + index);
                break;

            case FragmentType.ExpositorFragments:
                _productImage.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + index);
                break;

            case FragmentType.GroundFragments:
                _productImage.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + index);
                break;

            case FragmentType.SkinFragments:
                _productImage.color = Color.black;
                _productImage.sprite = Resources.Load<Sprite>("Sprites/Skins/" + index);
                break;

            case FragmentType.CharacterFragments:
                _productImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (index*2));
                break;

        }
        _mainPanel.SetActive(true);
        _slider.minValue = 1;
        _slider.value = 1;
        RefreshFragmentsAmount();
    }

    public void RefreshFragmentsAmount()
    {
        _fragmentAmount = (int)_slider.value;
        _fragmentsAmountText.text = "x"+_fragmentAmount;
        
        int totalPrice = _fragmentAmount * 5;
        _priceText.text= totalPrice.ToString();

        if (totalPrice <= UserDataController.GetHardCoins())
        {
            _buyButton.interactable = true;
        }
        else
        {
            _buyButton.interactable = false;
        }
    }

    public void Buy()
    {
        int totalPrice = _fragmentAmount * 5;
        UserDataController.SpendHardCoins(totalPrice);
        switch (_type)
        {
            case FragmentType.CellFragments:
                FindObjectOfType<RewardManager>().EarnCellFragments(_productIndex,_fragmentAmount);
                break;

            case FragmentType.ExpositorFragments:
                FindObjectOfType<RewardManager>().EarnExpositorFragments(_productIndex, _fragmentAmount);
                break;

            case FragmentType.GroundFragments:
                FindObjectOfType<RewardManager>().EarnGroundFragments(_productIndex, _fragmentAmount);
                break;

            case FragmentType.SkinFragments:
                _productImage.color = Color.black;
                FindObjectOfType<RewardManager>().EarnSkinFragments(_productIndex, _fragmentAmount);
                break;

            case FragmentType.CharacterFragments:
                FindObjectOfType<RewardManager>().EarnCharacterFragments(_productIndex, _fragmentAmount);
                break;
        }
        Close();
    }
    public void Close()
    {
        _mainPanel.SetActive(false);
    }
}
