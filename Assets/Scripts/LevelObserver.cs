using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelObserver : MonoBehaviour
{
    TextMeshProUGUI _levelText;
    [SerializeField]
    Image _expBar;
    private void Awake()
    {
        _levelText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        _levelText.text = UserDataController.GetLevel().ToString();
        _expBar.fillAmount = UserDataController.GetExperienceAmount();
    }
}