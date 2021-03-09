using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PackageShopManager : MonoBehaviour
{
    [SerializeField]
    Image _background, _cell, _auxCell, _expositor, _auxExpositor;
    [SerializeField]
    Image[] _points;
    [SerializeField]
    RectTransform _pack, _pack2;
    [SerializeField]
    GameObject _purchaseButton;
    [SerializeField]
    TextMeshProUGUI _packageName;
    int _packIndex = 0;
    ShopManager _shopManager;
    float _transitionTime = 0.4f;
    bool _auxPack = false;
    Vector3 initialPos;
    bool _canSwap = true;
    void Start()
    {
        RefreshSprites();
        _background.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + (_packIndex + 1));
        _shopManager = FindObjectOfType<ShopManager>();
        initialPos = _pack.anchoredPosition;
        _packageName.text = LocalizationController.GetValueByKey("PACKAGE_0");
    }

    public void SwapPackage(int amount)
    {
        if (_canSwap)
        {
            _packIndex += amount;
            _packIndex %= 4;
            if (_packIndex < 0)
            {
                _packIndex = 3;
            }
            _packageName.text = LocalizationController.GetValueByKey("PACKAGE_" + _packIndex);
            if (amount == 1)
            {
                _canSwap = false;
                StartCoroutine(Move(true));
                StartCoroutine(GroundSwap());
            }
            if (amount == -1)
            {
                _canSwap = false;
                StartCoroutine(Move(false));
                StartCoroutine(GroundSwap());
            }
        }

    }
    void RefreshSprites()
    {
        if (_auxPack)
        {
            _cell.sprite = Resources.Load<Sprite>("Sprites/Cells/" + (_packIndex + 1));
            _expositor.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + (_packIndex + 1));
        }
        else
        {
            _auxCell.sprite = Resources.Load<Sprite>("Sprites/Cells/" + (_packIndex + 1));
            _auxExpositor.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + (_packIndex + 1));
        }
        foreach (Image i in _points)
        {
            i.color = Color.white;
        }
        _points[_packIndex].color = Color.black;
        if (UserDataController.GetCellsSkins()[_packIndex + 1] || UserDataController.GetExpositorsSkins()[_packIndex + 1] || UserDataController.GetGroundSkins()[_packIndex + 1])
        {
            _purchaseButton.SetActive(false);
        }
        else
        {
            _purchaseButton.SetActive(true);
        }
    }

    public IEnumerator GroundSwap()
    {
        float semi = _transitionTime / 2f;
        for (float i = 0; i < semi; i+= Time.deltaTime)
        {
            _background.color = Color.Lerp(Color.white, Color.clear, i / semi);
            yield return null;
        }
        _background.color =Color.clear;
        _background.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + (_packIndex + 1));
        for (float i = 0; i < semi; i += Time.deltaTime)
        {
            _background.color = Color.Lerp(Color.clear, Color.white, i / semi);
            yield return null;
        }
        _background.color = Color.white;
        _canSwap = true;
    }
    public IEnumerator Move(bool right)
    {
        Vector3 distance = new Vector3(1000, 0, 0);
        if (!right)
        {
            distance = new Vector3(-1000, 0, 0);
        }
        Vector3 targetPos = initialPos - distance;
        Vector3 initialPos2 = initialPos + distance;
        RefreshSprites();
        if (_auxPack)
        {
            _pack2.anchoredPosition = initialPos;
            for (float i = 0; i < _transitionTime; i += Time.deltaTime)
            {
                _pack.anchoredPosition = Vector3.Lerp(initialPos2, initialPos, i / _transitionTime);
                _pack2.anchoredPosition = Vector3.Lerp(initialPos, targetPos, i / _transitionTime);
                yield return null;
            }
            _pack.anchoredPosition = initialPos;
            _pack2.anchoredPosition = targetPos;
        }
        else
        {
            _pack2.anchoredPosition = initialPos + distance;
            for (float i = 0; i < _transitionTime; i += Time.deltaTime)
            {
                _pack.anchoredPosition = Vector3.Lerp(initialPos, targetPos, i / _transitionTime);
                _pack2.anchoredPosition = Vector3.Lerp(initialPos2, initialPos, i / _transitionTime);
                yield return null;
            }
            _pack.anchoredPosition = targetPos;
            _pack2.anchoredPosition = initialPos;
        }
        _auxPack = !_auxPack;
    }

    public void Purchase()
    {
        if (_shopManager.PackOffer(_packIndex + 1))
        {
            RefreshSprites();
            _purchaseButton.SetActive(false);
        }
    }
}
