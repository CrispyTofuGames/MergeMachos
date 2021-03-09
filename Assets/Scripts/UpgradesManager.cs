using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    int _discount = 0;
    int _extraEarnings = 0;
    int _extraTouristSpeed = 0;
    int _extraPassiveEarnings = 0;
    int _gemDiscount = 0;
    private void Start()
    {
        CheckVip();
    }

    public int GetDiscount() 
    {
        return _discount;
    }
    public int GetExtraEarnings()
    {
        return _extraEarnings;
    }
    public int GetExtraTouristSpeed()
    {
        return _extraTouristSpeed;
    }
    public int GetExtraPassiveEarnings()
    {
        return _extraPassiveEarnings;
    }
    public int GetGemDiscount()
    {
        return _gemDiscount;
    }

    public void CheckVip()
    {
        if (UserDataController.IsVipUser())
        {
            _discount = 10;
            _extraEarnings = 10;
            _extraPassiveEarnings = 10;
        }
    }
}
