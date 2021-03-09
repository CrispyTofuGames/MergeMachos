using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootBoxController : MonoBehaviour
{
    PanelManager _panelManager;
    VFXManager _vFXManager;
    [SerializeField] GameObject _lootBoxMainPanel;
    CameraShake _cameraShake;
    [SerializeField] GameObject[] _mainUIItems;
    [SerializeField] GameObject _selectorPanel, _rewardPanel, _infoPanel;
    [SerializeField] GameObject _inGamePanel;
    [SerializeField] SpriteRenderer _chest;
    [SerializeField] Sprite[] _closedChests, _openedChests;
    [SerializeField] GameObject _openParticles;
    [SerializeField] TextMeshProUGUI _chestType;
    [SerializeField] Image _horizontalPanel;
    [SerializeField] Sprite[] _panelSprites;
    [SerializeField] Button _openChestButton, _continueButton;
    [SerializeField] Image[] _chestButtons;
    [SerializeField] RectTransform _rewardBg;
    [SerializeField] Transform _chestTr;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Transform[] _particlesTr;
    [SerializeField] Color _c1, _c2;
    [SerializeField] SpriteRenderer _background;
    [SerializeField] TextMeshProUGUI _unlockedTx;
    [SerializeField] GameObject _rewardsNull;
    [SerializeField] RectTransform[] _rewardImages;
    [SerializeField] LootBoxRewardInstance[] _rewardInstances;
    [SerializeField] GameObject _warningIcon;
    [SerializeField] GridLayoutGroup _rewardsGrid;
    [SerializeField] GameObject _goShopButton, _shopMainPanel;

    int _rewardsCount = 4;
    int _currentChestIndex = 0; 
    bool _showedRewards, _showingReward;
    int _currentRewardIndex = 0;
    bool _showingInfo = false;
    float _rewardDuration = 0.25f;

    List<LootReward> rewardsToGive;
    // Start is called before the first frame update
    void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        _vFXManager = FindObjectOfType<VFXManager>();
        _cameraShake = FindObjectOfType<CameraShake>();
        CheckWarningIcon();
    }

    public void OpenLootBoxPanel()
    {
        _showingInfo = false;
        _infoPanel.SetActive(false);
        _vFXManager.Stop();
        _vFXManager.StopLootFx();

        foreach (GameObject g in _mainUIItems)
        {
            g.SetActive(false);
        }
        _currentChestIndex = 0;
        SelectChest(0);
        _lootBoxMainPanel.SetActive(true);
        _inGamePanel.SetActive(true);
    }

    public void CheckWarningIcon()
    {
        if (UserDataController.GetTotalChestAmount() > 0)
        {
            _warningIcon.SetActive(true);
        }
        else
        {
            _warningIcon.SetActive(false);
        }
    }

    public void OpenChest()
    {
        _showedRewards = false;
        _vFXManager.Stop();
        _vFXManager.StopLootFx();
        GameEvents.PlaySFX.Invoke("Lootbox_open");
        StartCoroutine(OpenChestCr());
    }

    public void CloseLootBox()
    {
        _inGamePanel.SetActive(false);
        _lootBoxMainPanel.SetActive(false);
        foreach (GameObject g in _mainUIItems)
        {
            g.SetActive(true);
        }
    }

    public void SelectChest(int chestIndex)
    {
        _currentChestIndex = chestIndex;
        if(UserDataController.GetChestAmount(chestIndex) > 0) //Aqui preguntamos cuantos cofres tiene de ese tipo
        {
            _openChestButton.interactable = true;
            _goShopButton.SetActive(false);
        }
        else
        {
            _openChestButton.interactable = false;
            _goShopButton.SetActive(true);
        }
        foreach (Image i in _chestButtons)
        {
            i.color = new Color(0.7f, 0.7f, 0.7f,1f);
        }
        _chestButtons[_currentChestIndex * 2].color = Color.white;
        _chestButtons[(_currentChestIndex * 2) +1].color = Color.white;
        _chestType.text = LocalizationController.GetValueByKey("CHESTTYPE_"+ _currentChestIndex);
        _chest.sprite = _closedChests[_currentChestIndex];
        _openParticles.SetActive(false);
        _horizontalPanel.sprite = _panelSprites[_currentChestIndex];
    }

    IEnumerator OpenChestCr()
    {
        _continueButton.interactable = false;
        _particlesTr[_currentChestIndex].localPosition = Vector3.zero;
        _selectorPanel.SetActive(false);
        _rewardBg.gameObject.SetActive(false);
        _rewardPanel.SetActive(true);
        _currentRewardIndex = 0;
        yield return StartCoroutine(ChestOpenMovement());
        CreateRewards(_currentChestIndex); //AQUI CREAMOS LAS RECOMPENSAS EN LAS INSTANCIAS
        StartCoroutine(RewardAnim());
        while (!_showedRewards)
        {
            yield return null;
        }
        _vFXManager.Stop();
        _vFXManager.StopLootFx();
        yield return StartCoroutine(ChestBackMovement()); //AQUI ESPERAMOS A QUE EL COFRE SE CIERRE 
        UserDataController.OpenChest(_currentChestIndex);
        RewardManager rm = FindObjectOfType<RewardManager>();
        for(int i = 0; i< rewardsToGive.Count; i++)
        {
            switch (rewardsToGive[i]._lootRewardType)
            {
                case lootRewardTypes.character:
                    rm.EarnCharacterFragments(rewardsToGive[i]._productIndex, rewardsToGive[i]._amount);
                    break;
                case lootRewardTypes.skin:
                    rm.EarnSkinFragments(rewardsToGive[i]._productIndex, rewardsToGive[i]._amount);
                    break;
                case lootRewardTypes.softCoins:
                    rm.EarnSoftCoin(rewardsToGive[i]._amount);
                    break;
                case lootRewardTypes.hardCoins:
                    rm.EarnHardCoin(rewardsToGive[i]._amount);
                    break;
                case lootRewardTypes.customize:
                    switch (rewardsToGive[i]._productCategory)
                    {
                        case 0:
                            rm.EarnCellFragments(rewardsToGive[i]._productIndex, rewardsToGive[i]._amount);
                            break;

                        case 1:
                            rm.EarnExpositorFragments(rewardsToGive[i]._productIndex, rewardsToGive[i]._amount);
                            break;

                        case 2:
                            rm.EarnGroundFragments(rewardsToGive[i]._productIndex, rewardsToGive[i]._amount);
                            break;
                    }
                    break;
            }
        }
        CheckWarningIcon();
        _selectorPanel.SetActive(true);
        _rewardPanel.SetActive(false);
        SelectChest(_currentChestIndex);
    }

    public void InfoButton()
    {
        _showingInfo = !_showingInfo;
        _infoPanel.SetActive(_showingInfo);
    }

    public void ContinueButton()
    {
        _showedRewards = true;
    }

    IEnumerator RewardAnim()
    {
        Vector3 startPos = new Vector3(0, -1200f, 0);
        Vector3 targetPos = new Vector3(0, -600f, 0);
        Vector3 particlesTargetPos = new Vector3(0, 7.5f, 0);
        Vector2 startSize = new Vector2(100,100);
        Vector2 targetSize = new Vector2(700,700);
        _rewardBg.gameObject.SetActive(true);
        _continueButton.interactable = false;
        _rewardBg.anchoredPosition = startPos;
        _unlockedTx.color = new Color(1,1,1,0);
        _rewardsNull.transform.localScale = Vector3.zero;
        
        foreach(RectTransform r in _rewardImages)
        {
            r.sizeDelta = Vector2.zero;
        }
        for (int i = 0; i < _rewardInstances.Length; i++)
        {
            _rewardInstances[i].gameObject.SetActive(false);
        }
        StartCoroutine(ShowReward());

        for (float i = 0f; i< _rewardDuration; i+= Time.deltaTime)
        {
            _rewardBg.anchoredPosition = Vector3.Lerp(startPos, targetPos, _animationCurve.Evaluate(i/_rewardDuration));
            _particlesTr[_currentChestIndex].localPosition = Vector3.Lerp(Vector3.zero, particlesTargetPos, _animationCurve.Evaluate(i / _rewardDuration));
            _rewardsNull.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animationCurve.Evaluate(i / _rewardDuration));
            _rewardBg.sizeDelta = Vector2.Lerp(startSize, targetSize, _animationCurve.Evaluate(i /_rewardDuration));
            _unlockedTx.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, _animationCurve.Evaluate(i / _rewardDuration));
            yield return null;
        }

        _unlockedTx.color = Color.white;
        _rewardsNull.transform.localScale = Vector3.one;
        _particlesTr[_currentChestIndex].localPosition = particlesTargetPos;
        _rewardBg.anchoredPosition = targetPos;
    }

    IEnumerator ChestOpenMovement()
    {
        Vector3 topPos = new Vector3(0,-2.5f, 0);
        Vector3 botPos = new Vector3(0,-5f, 0);
        float dur = 0.25f;
        _cameraShake.ShakeCamera(0.1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _vFXManager.PlayLootBoxFX(_currentChestIndex);
        _chest.sprite = _openedChests[_currentChestIndex];
        _openParticles.SetActive(true);
        _chestTr.localPosition = topPos;
        _background.color = _c1;
        for (float i = 0; i<dur; i+= Time.deltaTime)
        {
            _chestTr.localPosition = Vector3.Lerp(topPos, botPos, _animationCurve.Evaluate(i / dur));
            _background.color = Color.Lerp(_c1, _c2, _animationCurve.Evaluate(i / dur));
            yield return null;
        }
        _chestTr.localPosition = botPos;
        _background.color = _c2;
    }

    IEnumerator ChestBackMovement()
    {
        Vector3 topPos = new Vector3(0, -2.5f, 0);
        Vector3 botPos = new Vector3(0, -5f, 0);
        float dur = 0.25f;
        _chestTr.localPosition = botPos;
        _background.color = _c2;
        _unlockedTx.color = Color.white;
        _rewardsNull.transform.localScale = Vector3.one;

        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            _chestTr.localPosition = Vector3.Lerp(botPos, topPos, _animationCurve.Evaluate(i / dur));
            _background.color = Color.Lerp(_c2, _c1, _animationCurve.Evaluate(i / dur));
            _rewardsNull.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, _animationCurve.Evaluate(i / dur));
            _unlockedTx.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), _animationCurve.Evaluate(i / dur));
            yield return null;
        }

        _rewardsNull.transform.localScale = Vector3.zero;
        _unlockedTx.color = new Color(1, 1, 1, 0);
        _background.color = _c1;
        _chestTr.localPosition = topPos;
    }

    IEnumerator ShowReward()
    {
        float dur = 0.15f;
        float fastDur = 0.05f;
        Vector2 big = new Vector2(200, 200);
        Vector2 normal = new Vector2(175, 175);

        _rewardsGrid.cellSize = Vector2.zero;

        for(int i = 0; i<_rewardsCount; i++)
        {
            _rewardInstances[i].gameObject.SetActive(true);
        }

        for (float i = 0; i < dur; i += Time.deltaTime)
        {        
            _rewardsGrid.cellSize = Vector2.Lerp(Vector2.zero, big, _animationCurve.Evaluate(i / dur));
            yield return null;
        }
        _rewardsGrid.cellSize = big;
        for (float i = 0; i < fastDur; i += Time.deltaTime)
        {
            _rewardsGrid.cellSize = Vector2.Lerp(big, normal, _animationCurve.Evaluate(i / fastDur));
            yield return null;
        }
        _rewardsGrid.cellSize = normal;
        _continueButton.interactable = true;
    }
    public enum lootRewardTypes {character, skin, customize, softCoins, hardCoins};
    void CreateRewards(int chestType)
    {
        int rewardFragments = 0;
        int numberOfRewards = 4 * (chestType + 1);
        _rewardsCount = numberOfRewards;
        List<int> fragmentsByTypes = new List<int>() {};
        for(int i = 0; i< numberOfRewards; i++)
        {
            fragmentsByTypes.Add(1);
        }
        List<lootRewardTypes> rewardTypes = new List<lootRewardTypes>() { lootRewardTypes.character, lootRewardTypes.skin, lootRewardTypes.customize, lootRewardTypes.softCoins, lootRewardTypes.hardCoins };
        List<lootRewardTypes> finalRewards = new List<lootRewardTypes>();
        for(int i = 0; i< numberOfRewards; i++)
        {
            int rand = Random.Range(0, rewardTypes.Count);
            finalRewards.Add(rewardTypes[rand]);
            if(rewardTypes[rand] == lootRewardTypes.hardCoins)
            {
                rewardTypes.Remove(lootRewardTypes.hardCoins);
            }
            else
            {
                if (rewardTypes[rand] == lootRewardTypes.softCoins)
                {
                    rewardTypes.Remove(lootRewardTypes.softCoins);
                }
            }

        }
        switch (chestType) //Calcula los fragmentos segun el tipo de cofre
        {
            case 0:
                rewardFragments = Random.Range(4, 9);
                break;
            case 1:
                rewardFragments = Random.Range(10, 18);
                break;
            case 2:
                rewardFragments = Random.Range(22, 38);
                break;
            case 3:
                rewardFragments = Random.Range(50, 82);
                break;
        }


        for (int i = 0; i< 50; i++) //Aletoriza el orden de las rewardTypes
        {
            int index1 = Random.Range(0,finalRewards.Count), index2 = Random.Range(0, finalRewards.Count);
            lootRewardTypes auxReward = finalRewards[index1];
            finalRewards[index1] = finalRewards[index2];
            finalRewards[index2] = auxReward;
        }

        for(int i = numberOfRewards; i<rewardFragments; i++) //Reparte los fragmentos
        {
            float n = Random.value;
            fragmentsByTypes[Random.Range(0, fragmentsByTypes.Count)]++;
        }
        rewardsToGive = new List<LootReward>();

        for (int i = 0; i< numberOfRewards; i++)
        {
            switch (finalRewards[i])
            {
                case lootRewardTypes.character:
                    List<int> possibleCharacters = UserDataController.GetNonMaxedCharacters();
                    LootReward lr = new LootReward(lootRewardTypes.character, 0, possibleCharacters[Random.Range(0, possibleCharacters.Count)],fragmentsByTypes[i]);
                    rewardsToGive.Add(lr);
                    _rewardInstances[i].Init(lr);
                    break;
                case lootRewardTypes.customize:
                    List<int> possibleCells = UserDataController.GetNonMaxedCells();
                    List<int> possibleExpositors = UserDataController.GetNonMaxedExpositors();
                    List<int> possibleGrounds = UserDataController.GetNonMaxedGrounds();
                    int rewardCategory = Random.Range(0, 3);
                    for (int j = 0; j< 10; j++)
                    {
                        bool available = true;
                        switch (rewardCategory)
                        {
                            case 0:
                                if (possibleCells.Count == 0)
                                {
                                    available = false;
                                }
                                break;

                            case 1:
                                if (possibleExpositors.Count == 0)
                                {
                                    available = false;
                                }
                                break;

                            case 2:
                                if (possibleGrounds.Count == 0)
                                {
                                    available = false;
                                }
                                break;
                        }
                        if (!available)
                        {
                            rewardCategory = Random.Range(0, 3);
                        }
                    }
                    lr = null;
                    switch (rewardCategory)
                    {
                        case 0:
                            lr = new LootReward(lootRewardTypes.customize, rewardCategory, possibleCells[Random.Range(0, possibleCells.Count)], fragmentsByTypes[i]);
                            break;

                        case 1:
                            lr = new LootReward(lootRewardTypes.customize, rewardCategory, possibleExpositors[Random.Range(0, possibleExpositors.Count)], fragmentsByTypes[i]);
                            break;

                        case 2:
                            lr = new LootReward(lootRewardTypes.customize, rewardCategory, possibleGrounds[Random.Range(0, possibleGrounds.Count)], fragmentsByTypes[i]);
                            break;
                    }

                    _rewardInstances[i].Init(lr);
                    rewardsToGive.Add(lr);
                    break;
                case lootRewardTypes.skin:
                    List<int> possibleSkins = UserDataController.GetNonMaxedSkins();

                    lr = new LootReward(lootRewardTypes.skin, 0,possibleSkins[Random.Range(0, possibleSkins.Count)], fragmentsByTypes[i]);
                    _rewardInstances[i].Init(lr);
                    rewardsToGive.Add(lr);
                    break;
                case lootRewardTypes.softCoins:
                    lr = new LootReward(lootRewardTypes.softCoins, 0, 0, fragmentsByTypes[i]*60);
                    _rewardInstances[i].Init(lr);
                    rewardsToGive.Add(lr);
                    break;
                case lootRewardTypes.hardCoins:
                    int totalHardCoins = 0;
                    for(int q = 0; q < fragmentsByTypes[i]; q++)
                    {
                        totalHardCoins += Random.Range(3, 6);
                    }
                    lr = new LootReward(lootRewardTypes.hardCoins, 0, 0, totalHardCoins);
                    _rewardInstances[i].Init(lr); //DAMOS LOS FRAGMENTOS QUE HAYAN TOCADO x LO QUE VALUEMOS EN GEMAS (HACEMOS RANDOM 2-5)??
                    rewardsToGive.Add(lr);
                    break;
            }
        }
    }
    public class LootReward
    {
        public lootRewardTypes _lootRewardType;
        public int _productCategory;
        public int _productIndex;
        public int _amount;

        public LootReward(lootRewardTypes lrt, int productCategory, int productIndex, int amount)
        {
            _lootRewardType = lrt;
            _productCategory = productCategory;
            _productIndex = productIndex;
            _amount = amount;
        }
    }

    public void GoShop()
    {
        CloseLootBox();
        _panelManager.RequestShowPanel(_shopMainPanel);
    }
}

