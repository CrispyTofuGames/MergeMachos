using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopModeManager : MonoBehaviour
{
    [SerializeField] GameObject[]_panels;
    [SerializeField] RectTransform _selectedBg;
    bool _moving = false;
    int _currentIndex = 0;
    [SerializeField] Image[] _shopImages, _blooms;
    [SerializeField] Material _grayMat, _standard;
    [SerializeField] RectTransform _gemPanel, _coinPanel;
    void Start()
    {
        _selectedBg.anchoredPosition = _gemPanel.anchoredPosition;
        for(int i = 0; i<_panels.Length; i++)
        {
            _panels[i].SetActive(false);
            _shopImages[i].material = _grayMat;
        }
        _panels[0].SetActive(true);
        _shopImages[0].material = _standard;
    }

    public void SelectPanel(int index)
    {
        if(_currentIndex != index && !_moving)
        {
            _currentIndex = index;
            StartCoroutine(TransitionTo());
            _moving = true;
        }
    }

    IEnumerator TransitionTo()
    {
        Vector2 currentPos;
        Vector2 targetPos;

        if (_currentIndex == 0)
        {
            currentPos = _coinPanel.anchoredPosition;
            targetPos = _gemPanel.anchoredPosition;
        }
        else
        {
            currentPos = _gemPanel.anchoredPosition;
            targetPos = _coinPanel.anchoredPosition;
        }

        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(false);
            _shopImages[i].material = _grayMat;
        }
        _panels[_currentIndex].SetActive(true);
        _shopImages[_currentIndex].material = _standard;

        for (float i = 0; i<0.25f; i+= Time.deltaTime)
        {
            _selectedBg.anchoredPosition = Vector2.Lerp(currentPos, targetPos, i / 0.25f);
            yield return null;
        }
        _selectedBg.anchoredPosition = targetPos;
        _moving = false;
    }
}
