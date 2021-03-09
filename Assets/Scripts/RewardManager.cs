using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [SerializeField] Sprite[] _rewardSprites;
    [SerializeField] Image _rewardImage;
    [SerializeField] TextMeshProUGUI _txReward;
    [SerializeField] PanelManager _panelManager;
    [SerializeField] EconomyManager _economyManager; [SerializeField]
    SpeedUpManager _speedUpManager;
    [SerializeField] GameObject _mainPanel;
    Queue<RewardData> rewardDataQueue;
    bool canClose = false;
    bool panelIsOpen;
    VFXManager _vfxManager;
    RewardData _rewardData;
    [SerializeField] Image _characterFragmentsRewardImage;
    [SerializeField] Image _skinFragmentsRewardImage;
    [SerializeField] Image _characterFragmentFace;
    [SerializeField] TextMeshProUGUI _fragmentAmount;

    [SerializeField] CharacterLevelUpRewardImage _levelUp;
    [SerializeField] CardConfigurator _skinCardConfigurator;
    [SerializeField] TextMeshProUGUI _skinFragmentAmount;
    [SerializeField] Sprite[] _chestSprites;
    [SerializeField] LootBoxController _lootBoxController;

    private void Awake()
    {
        _vfxManager = FindObjectOfType<VFXManager>();
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
        GameEvents.Purchase.AddListener(PurchaseDinoCallBack);
        GameEvents.RewardMergeUp.AddListener(CheckDinoUp);
        rewardDataQueue = new Queue<RewardData>();
        panelIsOpen = false;
    }
    public void ShowPanel()
    {
        RewardData r = rewardDataQueue.Dequeue();
        GameEvents.PlaySFX.Invoke("Achievement");
        if (!panelIsOpen)
        {
            _mainPanel.SetActive(true);
            panelIsOpen = true;
        }
        _vfxManager.Explode();
        RefreshInfo(r);
        StartCoroutine(WaitToClose(1f));
    }
    public void ClosePanel()
    {
        if (canClose)
        {
            if (_rewardData._rewardType == RewardType.UnlockCustomization)
            {
                FindObjectOfType<DailyRewardManager>().OpenPanel();
            }
            if (rewardDataQueue.Count > 0)
            {
                ShowPanel();
            }
            else
            {
                _mainPanel.SetActive(false);
                panelIsOpen = false;
            }
        }
    }

    public int GetRemainingRewards()
    {
        if(rewardDataQueue.Count == 0)
        {
            if (panelIsOpen)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return rewardDataQueue.Count;
        }
    }
    public void CheckDinoUp(int dinoType)
    {
        if(dinoType == 5)
        {
            UnlockSpin();
        }
        if (dinoType == 6)
        {
            UnlockUpgrades();
        }
        if (dinoType == 7)
        {
            UnlockMissions();
        }
    }

    public void UnlockSpin()
    {
        rewardDataQueue.Enqueue(new RewardData(4, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void UnlockMissions()
    {
        rewardDataQueue.Enqueue(new RewardData(5, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void UnlockUpgrades()
    {
        rewardDataQueue.Enqueue(new RewardData(RewardType.UnlockCustomization, 0));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnSoftCoin(int seconds)
    {
        GameCurrency baseRewardPSec;
        baseRewardPSec = new GameCurrency(_economyManager.GetTotalEarningsPerSecond().GetIntList());
        baseRewardPSec.MultiplyCurrency(seconds);
        rewardDataQueue.Enqueue(new RewardData(0, baseRewardPSec));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnCharacterFragments(int character, int amount)
    {
        RewardData r = new RewardData(RewardType.CharacterFragments, amount);
        r._productIndex = character;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnSkinFragments(int skin, int amount)
    {
        RewardData r = new RewardData(RewardType.SkinFragments, amount);
        r._productIndex = skin;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnCellFragments(int cell, int amount)
    {
        RewardData r = new RewardData(RewardType.CustomizeCellFragments, amount);
        r._productIndex = cell;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnExpositorFragments(int expositor, int amount)
    {
        RewardData r = new RewardData(RewardType.CustomizeExpositorFragments, amount);
        r._productIndex = expositor;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnGroundFragments(int ground, int amount)
    {
        RewardData r = new RewardData(RewardType.CustomizeGroundFragments, amount);
        r._productIndex = ground;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    public void EarnGifts(int gifts)
    {
        rewardDataQueue.Enqueue(new RewardData(7, gifts));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnHardCoin(int amount)
    {
        rewardDataQueue.Enqueue(new RewardData(1, amount));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void LevelUpCharacter(int character, int level)
    {
        RewardData r = new RewardData(RewardType.CharacterLevelUp, character);
        r._productIndex = character;
        r._amount = level;
        rewardDataQueue.Enqueue(r);
        

        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnSpeedUp(int seconds)
    {
        rewardDataQueue.Enqueue(new RewardData(2, seconds));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }


    public void EarnDinoEarnings(int seconds)
    {
        rewardDataQueue.Enqueue(new RewardData(3, seconds));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    public void EarnCustomizeItem(int productIndex, int category)
    {
        rewardDataQueue.Enqueue(new RewardData(RewardType.CustomizationElement, productIndex, category));
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }
    public void EarnLootBox(int chestType, int amount)
    {
        RewardData r = new RewardData(RewardType.LootBox, amount);
        r._productIndex = chestType;
        rewardDataQueue.Enqueue(r);
        if (!panelIsOpen)
        {
            ShowPanel();
        }
    }

    IEnumerator WaitToClose(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canClose = true;
    }

    public class RewardData
    {
        public RewardType _rewardType;
        public GameCurrency _softCoinsAmount;
        public int _amount;
        public int _category;
        public int _productIndex;
        
        public RewardData(int rewardType, int amount)
        {
            _rewardType = (RewardType)rewardType;
            _amount = amount;
        }
        public RewardData(int rewardType, GameCurrency coinsAmount)
        {
            _rewardType = (RewardType)rewardType;
            _softCoinsAmount = coinsAmount;
        }
        public RewardData(int rewardType,int productIndex, int category)
        {
            _rewardType = (RewardType)rewardType;
            _productIndex = productIndex;
            _category = category;
        }
        public RewardData(RewardType rewardType, int amount)
        {
            _rewardType = rewardType;
            _amount = amount;
        }
        public RewardData(RewardType rewardType, GameCurrency coinsAmount)
        {
            _rewardType = rewardType;
            _softCoinsAmount = coinsAmount;
        }
        public RewardData(RewardType rewardType, int productIndex, int category)
        {
            _rewardType =rewardType;
            _productIndex = productIndex;
            _category = category;
        }
    }

    public void RefreshInfo(RewardData r)
    {
        _rewardData = r;
        _rewardImage.rectTransform.sizeDelta = new Vector3(300, 300, 1);
        _rewardImage.gameObject.SetActive(true);

        _characterFragmentsRewardImage.gameObject.SetActive(false);
        _skinFragmentsRewardImage.gameObject.SetActive(false);
        _levelUp.gameObject.SetActive(false);
        switch (r._rewardType)
        {
            case RewardType.SoftCoins:
                _txReward.text = r._softCoinsAmount.GetCurrentMoney();
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                _economyManager.EarnSoftCoins(r._softCoinsAmount);
                break;
            case RewardType.HardCoins:
                _txReward.text = string.Format(LocalizationController.GetValueByKey("REWARD_HARDCOINS"), r._amount);
                Sprite targetSprite = _rewardSprites[(int)r._rewardType];
                switch (r._amount)
                {
                    default:
                        targetSprite = _rewardSprites[(int)r._rewardType];
                        break;
                    case 40: 
                        targetSprite = _rewardSprites[8];
                        break;
                    case 360:
                        targetSprite = _rewardSprites[9];
                        break;
                    case 840:
                        targetSprite = _rewardSprites[10];
                        break;
                    case 1920:
                        targetSprite = _rewardSprites[11];
                        break;
                    case 5100:
                        targetSprite = _rewardSprites[12];
                        break;
                    case 11400:
                        targetSprite = _rewardSprites[13];
                        break;
                }
                _rewardImage.sprite = targetSprite;
                UserDataController.AddHardCoins(r._amount);
                break;
            case RewardType.SpeedUpTime:
                _txReward.text = string.Format(LocalizationController.GetValueByKey("REWARD_SPEEDUP"), r._amount);
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                _speedUpManager.SpeedUpCallback(r._amount);
                break;
            case RewardType.UnlockSpin:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_SPIN");
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                break;
            case RewardType.UnlockMissions:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_MISSIONS");
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                break;
            case RewardType.UnlockCustomization:
                _txReward.text = LocalizationController.GetValueByKey("UNLOCK_CUSTOMIZE");
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                break;
            case RewardType.Gifts:
                _txReward.text = LocalizationController.GetValueByKey("NEST");
                _rewardImage.sprite = _rewardSprites[(int)r._rewardType];
                FindObjectOfType<BoxManager>().RewardBox(r._amount);
                break;
            case RewardType.CustomizationElement:
                switch (r._category)
                {
                    case 0:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Cells/" + r._productIndex);
                        _txReward.text = LocalizationController.GetValueByKey("UNLOCK_CELL");
                        break;
                    case 1:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + r._productIndex);
                        _txReward.text = LocalizationController.GetValueByKey("UNLOCK_EXPOSITOR");
                        break;
                    case 2:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + r._productIndex);
                        _txReward.text = LocalizationController.GetValueByKey("UNLOCK_GROUND");
                        break;
                    case 3:
                        _rewardImage.sprite = Resources.Load<Sprite>("Sprites/Frames/" + r._productIndex);
                        _txReward.text = LocalizationController.GetValueByKey("UNLOCK_FRAME");
                        break;
                }
                _rewardImage.rectTransform.sizeDelta = new Vector3(500, 500, 1);
                break;

            case RewardType.CharacterFragments:
                
                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(true);
                _skinFragmentsRewardImage.gameObject.SetActive(false);
                _characterFragmentFace.overrideSprite =  Resources.Load<Sprite>("Sprites/FaceSprites/" + (r._productIndex*2));

                _fragmentAmount.text = "x" + r._amount;
                _txReward.text ="x" + r._amount +" " + FindObjectOfType<DayCareManager>().GetChibiName(r._productIndex * 2)+ " " + LocalizationController.GetValueByKey("LIKELY_0");
                UserDataController.AddCharacterFragments(r._productIndex,r._amount);
                GameEvents.BuyCharacterFragments.Invoke();
                break;

            case RewardType.SkinFragments:
                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(false);
                _skinFragmentsRewardImage.gameObject.SetActive(true);
                _skinCardConfigurator.InitSkin(r._productIndex);
                _skinFragmentAmount.text = "x" + r._amount;
                int character = SpecialSkinsManager._specialSkins[r._productIndex]._character;
                _txReward.text = "x" + r._amount + " " + FindObjectOfType<DayCareManager>().GetChibiName(character * 2) + " " + LocalizationController.GetValueByKey("LIKELY_2");
                UserDataController.AddExtraSkinFragments(r._productIndex, r._amount);
                GameEvents.BuySkinFragments.Invoke();
                break;

            case RewardType.CharacterLevelUp:
                _rewardImage.gameObject.SetActive(false);
                _levelUp.Init(r._productIndex, r._amount);
                _levelUp.gameObject.SetActive(true);
                _txReward.text = FindObjectOfType<DayCareManager>().GetChibiName(r._productIndex * 2) + " " + LocalizationController.GetValueByKey("LEVEL_UP_PANEL");
                UserDataController.LevelUpCharacter(r._productIndex);
                break;

            case RewardType.CustomizeCellFragments:

                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(true);
                _skinFragmentsRewardImage.gameObject.SetActive(false);
                _characterFragmentFace.overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + (r._productIndex));
                _fragmentAmount.text = "x" + r._amount;
                _txReward.text = "x" + r._amount + " " + CustomizeSkinsManager._cellSkins[r._productIndex]._name + " " + LocalizationController.GetValueByKey("LIKELY_1");
                UserDataController.AddCellFragments(r._productIndex, r._amount);
                GameEvents.BuyCustomizeSkinFragments.Invoke();
                break;
            case RewardType.CustomizeExpositorFragments:

                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(true);
                _skinFragmentsRewardImage.gameObject.SetActive(false);
                _characterFragmentFace.overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + (r._productIndex));
                _fragmentAmount.text = "x" + r._amount;
                _txReward.text = "x" + r._amount + " " + CustomizeSkinsManager._expositorSkins[r._productIndex]._name + " " + LocalizationController.GetValueByKey("LIKELY_1");
                UserDataController.AddExpositorFragments(r._productIndex, r._amount);
                GameEvents.BuyCustomizeSkinFragments.Invoke();
                break;
            case RewardType.CustomizeGroundFragments:

                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(true);
                _skinFragmentsRewardImage.gameObject.SetActive(false);
                _characterFragmentFace.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + (r._productIndex));
                _fragmentAmount.text = "x" + r._amount;
                _txReward.text = "x" + r._amount + " " + CustomizeSkinsManager._groundSkins[r._productIndex]._name + " " + LocalizationController.GetValueByKey("LIKELY_1");
                UserDataController.AddGroundFragments(r._productIndex, r._amount);
                GameEvents.BuyCustomizeSkinFragments.Invoke();
                break;

            case RewardType.LootBox:
                _rewardImage.gameObject.SetActive(false);
                _characterFragmentsRewardImage.gameObject.SetActive(true);
                _skinFragmentsRewardImage.gameObject.SetActive(false);
                _fragmentAmount.text = "x" + r._amount;
                _characterFragmentFace.sprite = _chestSprites[r._productIndex];
                _characterFragmentFace.overrideSprite= _chestSprites[r._productIndex];
                switch (r._productIndex)
                {
                    case 0:
                        _txReward.text = LocalizationController.GetValueByKey("CHESTTYPE_0");
                        break;

                    case 1:
                        _txReward.text = LocalizationController.GetValueByKey("CHESTTYPE_1");
                        break;

                    case 2:
                        _txReward.text = LocalizationController.GetValueByKey("CHESTTYPE_2");
                        break;
                    case 3:
                        _txReward.text = LocalizationController.GetValueByKey("CHESTTYPE_3");
                        break;
                }
                UserDataController.AddLootBoxes(r._productIndex, r._amount);
                _lootBoxController.CheckWarningIcon();
                break;
        }
    }

    public void MergeDinoCallBack(int dinoType)
    {
        UserDataController.AddDailyMerge();
    }
    public void PurchaseDinoCallBack(int dinoType)
    {
        UserDataController.AddDailyPurchase();
    }

    public void GiveVideoFillReward(int rewardType)
    {
        switch (rewardType)
        {
            case 1:
                EarnSoftCoin(7200);
                break;
            case 2:
                EarnSoftCoin(14400);
                break;
            case 3:
                EarnSoftCoin(21600);
                break;
        }
    }
}

