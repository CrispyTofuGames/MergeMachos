using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardInstance : MonoBehaviour
{
    [SerializeField]
    Image button;
    [SerializeField]
    GameObject _darkPanel, _claimedTx;
    [SerializeField]
    Sprite yellowB, grayB;

    public void UsedConfig()
    {
        _darkPanel.SetActive(true);
        _claimedTx.SetActive(true);
        button.sprite = grayB;
        button.overrideSprite = grayB;
        button.color = Color.gray;
    }
    public void SelectedConfig()
    {
        _darkPanel.SetActive(false);
        button.sprite = yellowB;
        button.overrideSprite = yellowB;
        button.color = Color.white;
    }
    public void BasicConfig()
    {
        button.sprite = yellowB;
        button.overrideSprite = yellowB;
        _darkPanel.SetActive(true);
        _claimedTx.SetActive(false);
        button.color = Color.gray;
    }
}
