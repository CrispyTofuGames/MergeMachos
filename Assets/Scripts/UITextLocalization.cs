using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextLocalization : MonoBehaviour
{
    [SerializeField]
    string key;
    [SerializeField]
    bool _caps;
    void Start()
    {
        string text = LocalizationController._localizedData[key];
        if (_caps)
        {
            text = text.ToUpper();       
        }
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
