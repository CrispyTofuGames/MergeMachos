using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootBoxRewardInstance : MonoBehaviour
{
    [SerializeField] GameObject _ticket;
    [SerializeField] TextMeshProUGUI _amount, _ticketAmount;
    [SerializeField] Image _rewardImage;
    [SerializeField] Sprite _coinImage, _gemImage, _customizeImage, _skinImage;
    [SerializeField] Image _bgImage;
    [SerializeField] Sprite[] _bgImages;
    [SerializeField] Material _default, _special;
    public void Init(LootBoxController.LootReward lr)
    {
        _ticket.SetActive(false);
        _rewardImage.color = Color.white;
        _bgImage.sprite = _bgImages[0];
        _bgImage.material = _default;

        switch (lr._lootRewardType)
        {
            case LootBoxController.lootRewardTypes.character:
                _rewardImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + lr._productIndex * 2);
                _amount.text = "";
                _bgImage.sprite = _bgImages[1];
                _ticket.SetActive(true);
                _ticketAmount.text = "x" + lr._amount;
                break;
            case LootBoxController.lootRewardTypes.customize:
                
                switch (lr._productCategory)
                {
                    case 0:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Cells/" + (lr._productIndex)); ;
                        break;

                    case 1:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + (lr._productIndex)); ;
                        break;

                    case 2:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + (lr._productIndex)); ;
                        break;
                }
                _amount.text = "";
                _bgImage.sprite = _bgImages[2];
                _ticket.SetActive(true);
                _ticketAmount.text = "x" + lr._amount;
                break;
            case LootBoxController.lootRewardTypes.skin:
                _rewardImage.color = Color.black;
                _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Skins/" + lr._productIndex);
                _amount.text = "";
                _bgImage.sprite = _bgImages[3];
                _bgImage.material = _special;
                _ticket.SetActive(true);
                _ticketAmount.text = "x" + lr._amount;
                break;
            case LootBoxController.lootRewardTypes.softCoins:
                _rewardImage.sprite = _coinImage;
                _amount.text = "x" + lr._amount + "s";
                break;
            case LootBoxController.lootRewardTypes.hardCoins:
                _rewardImage.sprite = _gemImage;
                _amount.text = "x" + lr._amount;
                break;
        }
    }
}
