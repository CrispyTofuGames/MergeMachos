using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoftCoinsObserver : MonoBehaviour
{
    TextMeshProUGUI _softCoinText;

    private void Start()
    {
        _softCoinText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (UserDataController._checked)
        {
            _softCoinText.text = new GameCurrency(UserDataController._currentUserData._softCoins).GetCurrentMoney();
        }
    }
}
