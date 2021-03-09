using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinAdviceManager : MonoBehaviour
{
    [SerializeField]
    CoinAdvice [] _adviceTextGroup;
    int _currentText = 0;

    private void Awake()
    {
        GameEvents.EarnMoney.AddListener(Play);
    }
    public void Play(GameEvents.MoneyEventData moneyEventData)
    {
        _adviceTextGroup[_currentText].Play(moneyEventData._position + new Vector3(0, 1.5f, 0), moneyEventData._money);
        _currentText++;
        _currentText = _currentText % _adviceTextGroup.Length;
    }
}