using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HardCoinsObserver : MonoBehaviour
{
    TextMeshProUGUI _hardCoinsText;
    private void Start()
    {
        _hardCoinsText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (UserDataController._checked)
        {
            _hardCoinsText.text = UserDataController._currentUserData._hardCoins.ToString();
        }
    }
}