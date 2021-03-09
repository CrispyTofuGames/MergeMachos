using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardInstance : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _numberOfReward;
    [SerializeField]
    Image _rewardImage;

    public void SetRewards(Sprite rewardType, int numberOfThisReward)
    {
        _rewardImage.sprite = rewardType;
        _rewardImage.overrideSprite = rewardType;
        _numberOfReward.text = "+ " + numberOfThisReward.ToString();
    }
}
