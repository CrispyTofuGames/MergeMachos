using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VipRemainingTime : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _remainingText;
    [SerializeField]
    VipController _vipController;
    // Update is called once per frame
    void Update()
    {
        if (UserDataController.IsVipUser())
        {
            _remainingText.text = _vipController.GetVipTime();
        }
    }
}
