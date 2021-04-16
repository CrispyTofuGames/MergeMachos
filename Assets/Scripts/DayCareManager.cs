using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class DayCareManager : MonoBehaviour
{
    EconomyManager _economyManager;
    [SerializeField]
    Button _fastPurchase;
    [SerializeField]
    Image _fastPurchaseFace;
    [SerializeField]
    TextMeshProUGUI _fastPurchaseCost;
    [SerializeField]
    TextMeshProUGUI _fastPurchaseName;
    [SerializeField]
    GameObject _shopPanel;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    GameObject _shopButton;
    [SerializeField]
    GameObject _purchaseDinoPanelPrefab;
    [SerializeField]
    GameObject _comingSoonPanelPrefab;
    [SerializeField]
    Transform _panelParent;
    List<PurchaseDinoPanel> _dinoPanelManagers;
    bool watchedVideo = false;
    int[] gemCost = {4, 6, 8, 10, 12, 14, 16, 20, 24, 28, 32, 36, 40, 45, 50, 55, 60, 66, 72, 78, 86, 94, 102, 110, 120, 130, 140, 150, 165, 180, 200, 220, 240, 260, 280, 300, 320, 340};
    PanelManager _panelManager;
    int smallGemCost, bigGemCost;

    public enum PurchaseButtonType {SoftCoins, Hardcoins, Ad};

    string[] dinoNames = { "Shori", "Runa", "Sooru", "Dylon", "Frenkie", "Tobio", "Tulio"};

    int _fastPurchaseDinoType = 0;
    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
        _shopPanel.SetActive(false);
        _panelManager = FindObjectOfType<PanelManager>();
        GameEvents.EarnMoney.AddListener(RefreshButtons);
        if (UserDataController.GetBiggestDino() < 3)
        {
            _shopButton.SetActive(false);
        }
    }
    void Start()
    {
        _dinoPanelManagers = new List<PurchaseDinoPanel>();

        for (int i = 0; i < UserDataController.GetDinoAmount(); i++)
        {
            GameObject nPanel = Instantiate(_purchaseDinoPanelPrefab, transform.position, Quaternion.identity);
            nPanel.transform.SetParent(_panelParent);
            nPanel.transform.localScale = Vector3.one;
            PurchaseDinoPanel p = nPanel.GetComponent<PurchaseDinoPanel>();
            p.SetDinoImage(Resources.Load<Sprite>("Sprites/FaceSprites/" + i));
            p.SetDinoName(GetChibiName(i));
            int index = i;
            p.GetDinoButton().onClick.AddListener(()=>SoftCoinPurchase(index));
            p.GetGemsButton().onClick.AddListener(()=>HardCoinsPurcharse(index));

            _dinoPanelManagers.Add(p);
        }
        RefreshButtons(null);
        GameObject comingSooonPanel = Instantiate(_comingSoonPanelPrefab, transform.position, Quaternion.identity);
        comingSooonPanel.transform.SetParent(_panelParent);
        comingSooonPanel.transform.localScale = Vector3.one;
    }

    public string GetChibiName(int index)
    {
        string chibiname = "";
        if (index > 0 && index % 2 != 0)
        {
            chibiname = dinoNames[(index - 1) / 2] + " (nude)";
        }
        else
        {
            chibiname = dinoNames[index / 2];
        }
        return chibiname;
    }

    public string[] GetNamesList()
    {
        return dinoNames;
    }

    public void Close()
    {
        _shopPanel.SetActive(false);
    }
    public void Open()
    {
        RefreshButtons(null);
        _panelManager.RequestShowPanel(_shopPanel.gameObject);
    }

    public void RefreshButtons(GameEvents.MoneyEventData e)
    {
        int fastPurchaseIndex = GetFastPurchaseIndex();
        int biggestDino = UserDataController.GetBiggestDino();
        for (int i = 0; i < _dinoPanelManagers.Count; i++)
        {
            if (i == fastPurchaseIndex)
            {
                //_fastPurchase.interactable = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
                _fastPurchaseCost.text = _economyManager.GetDinoCost(i).GetCurrentMoneyConvertedTo3Chars();
                _fastPurchaseFace.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + i);
                _fastPurchaseFace.overrideSprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + i);
                _fastPurchaseName.text = GetChibiName(i);
                _fastPurchaseDinoType = i;
                
            }
            if (i > biggestDino)
            {
                _dinoPanelManagers[i].LockPanel();
            }
            else
            {
                if (biggestDino <= 3)
                {
                    _dinoPanelManagers[0].UnlockPanel(0);
                    _dinoPanelManagers[1].UnlockPanel(1);
                    _dinoPanelManagers[1].SetGemsCost(2);
                    _dinoPanelManagers[2].UnlockPanel(1);
                    _dinoPanelManagers[2].SetGemsCost(3);
                    _dinoPanelManagers[3].LockPurcharse();
                    smallGemCost = 2;
                    bigGemCost = 3;
                    if(biggestDino == 3)
                    {
                        _dinoPanelManagers[i].UnlockPanel(2);
                    }
                }
                else
                {
                    //Desbloqueo Corriente
                    if (biggestDino >= 4 && biggestDino < 8)
                    {
                        if (i > biggestDino -2)
                        {
                            _dinoPanelManagers[i].LockPurcharse(); //2 primeros bloqueados
                            _dinoPanelManagers[i].UnlockPanel(2);
                        }
                        else
                        {
                            if(i > biggestDino - 4)
                            {
                                if (i == biggestDino - 2)
                                {
                                    _dinoPanelManagers[i].UnlockPanel(1); //Hardcoins
                                    _dinoPanelManagers[i].SetGemsCost(4);
                                }
                                if (i == biggestDino - 3)
                                {
                                    _dinoPanelManagers[i].UnlockPanel(1); //Hardcoins
                                    _dinoPanelManagers[i].SetGemsCost(3);
                                }
                                smallGemCost = 3;
                                bigGemCost = 4;
                            }
                            else
                            {
                                    _dinoPanelManagers[i].UnlockPanel(0);
                            }
                        }
                    }
                    else
                    {
                        if (biggestDino >= 8)
                        {
                            if (i < biggestDino - 4)
                            {
                                _dinoPanelManagers[i].UnlockPanel(0); //Normal
                            }
                            else
                            {
                                if (i == biggestDino - 4)
                                {
                                    if (watchedVideo)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(2); //Video
                                    }
                                    else
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(0); //Normal
                                    }
                                }
                                else
                                {
                                    if (i == biggestDino - 2)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(1); //Hardcoins
                                        _dinoPanelManagers[i].SetGemsCost(gemCost[biggestDino -7]);
                                        smallGemCost = gemCost[biggestDino - 7];
                                    }
                                    if (i == biggestDino - 3)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(1); //Hardcoins
                                        _dinoPanelManagers[i].SetGemsCost(gemCost[biggestDino - 8]);
                                        bigGemCost = gemCost[biggestDino - 8];
                                    }
                                    if (i == biggestDino || i == biggestDino - 1)
                                    {
                                        _dinoPanelManagers[i].UnlockPanel(2);
                                        _dinoPanelManagers[i].LockPurcharse(); //2 primeros bloqueados
                                    }
                                }
                            }
                        }
                    }
                }
            }
            bool canPurchase = UserDataController.HaveMoney(_economyManager.GetDinoCost(i));
            bool canGemPurchase = UserDataController.HaveGems(gemCost[i]);
            _dinoPanelManagers[i].SetProfits(_economyManager.GetEarningsByType(i).GetCurrentMoneyConvertedTo3Chars());
            _dinoPanelManagers[i].SetPurchaseCost(_economyManager.GetDinoCost(i).GetCurrentMoneyConvertedTo3Chars());
            _dinoPanelManagers[i].SetPurchaseState(canPurchase);
            _dinoPanelManagers[i].SetGemState(canGemPurchase);
        }
    }

    public int GetAdPurchaseIndex()
    {
        int biggestDino = UserDataController.GetBiggestDino();
        if (biggestDino > 8)
        {
            return biggestDino - 4;
        }
        else
        {
            return -1;
        }
    }
    public int GetGemPurchaseCost(int index)
    {
        int biggestDino = UserDataController.GetBiggestDino();
        int result = -1;
        if (biggestDino > 8)
        {
            if(index == biggestDino - 2)
            {
                result =  bigGemCost;
            }
            else
            {
                if (index == biggestDino - 3)
                {
                    result = smallGemCost;
                }
            }
        }
        return result;
    }
    public int GetFastPurchaseIndex()
    {
        int biggestDino = UserDataController.GetBiggestDino();
        int fastPurchaseIndex = 0;
        if (biggestDino < 7)
        {
            fastPurchaseIndex = biggestDino - 4;
        }
        else
        {
            if(biggestDino < 9)
            {
                fastPurchaseIndex = biggestDino - 5;
            }
            else
            {
                if (biggestDino < 11)
                {
                    fastPurchaseIndex = biggestDino - 6;
                }
                else
                {
                    fastPurchaseIndex = biggestDino - 7;
                }
            }
        }

        return Mathf.Max(fastPurchaseIndex, 0);
    }
    public void SoftCoinPurchase(int dinoType)
    {
        if (CurrentSceneManager._canPurchase)
        {
            if (UserDataController.GetEmptyCells() > 0)
            {
                if (_economyManager.SpendSoftCoins(_economyManager.GetDinoCost(dinoType)))
                {
                    _mainGameSceneController.PurchaseDino(dinoType);
                    RefreshButtons(null);
                }
            }
            else
            {
                GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOEMPTYCELLS"));
            }
        }
    }
    public void HardCoinsPurcharse(int dinoType)
    {
        if (UserDataController.GetEmptyCells() > 0)
        {
            if (_economyManager.SpendHardCoins(_dinoPanelManagers[dinoType].GetGemCost()))
            {
                _mainGameSceneController.PurchaseDino(dinoType);
                RefreshButtons(null);
            }
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOEMPTYCELLS"));
        }
    }

    public void WatchVideoCallback()
    {
        watchedVideo = true;
        _mainGameSceneController.PurchaseDino(GetAdPurchaseIndex());
        RefreshButtons(null);
    }

    public void FastPurchase()
    {
        SoftCoinPurchase(_fastPurchaseDinoType);
    }

}


