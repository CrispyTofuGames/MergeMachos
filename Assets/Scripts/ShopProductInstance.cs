using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopProductInstance : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _rewardTx;
    [SerializeField]
    TextMeshProUGUI _realCostTx;
    int _index = 0;
    bool _isGem = false;
    ShopManager _shopManager;

    public void Init(string reward, float realCost, int index, bool isGem, ShopManager s)
    {
        _rewardTx.text = reward;
        _realCostTx.text = realCost.ToString();
        _index = index;
        _isGem = isGem;
        _shopManager = s;
    }
    public void Purchase()
    {
        _shopManager.Purchase(_isGem, _index);
    }

}
