using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestAmountListener : MonoBehaviour
{
    TextMeshProUGUI _text;
    [SerializeField] int _chestType;
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = UserDataController.GetChestAmount(_chestType).ToString();
    }
}
