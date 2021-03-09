using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class LevelUpPanelManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _levelUpTx;
    [SerializeField]
    GameObject _levelUpPanel;
    bool canDisable;
    ExperienceManager _experienceManager;
    [SerializeField]
    Transform _rewardsPanel;
    [SerializeField]
    Sprite _gemIcon;
    [SerializeField]
    Sprite[] _chestIcons;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    GameObject _rewardsPrefab;
    PanelManager _panelManager;
    VFXManager _vFXManager;
    [SerializeField]
    RewardManager _rewardManager;
    private void Start()
    {
        _experienceManager = FindObjectOfType<ExperienceManager>();
        _panelManager = FindObjectOfType<PanelManager>();
        _vFXManager = FindObjectOfType<VFXManager>();
    }
    public void LevelUp()
    {
        StartCoroutine(ShowNewLevelInfo());
        LevelUpRewards(UserDataController.GetLevel());
    }

    IEnumerator ShowNewLevelInfo()
    {
        yield return null;
        _panelManager.RequestShowPanel(_levelUpPanel);
        StartCoroutine(WaitingForRequest());
        canDisable = false;
        int level = UserDataController.GetLevel();
        _levelUpTx.text = level.ToString();
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }
    IEnumerator WaitingForRequest()
    {
        while (!_levelUpPanel.activeSelf)
        {
            yield return null;
        }
        _vFXManager.Explode();
    }
    public void DisableMainPanel()
    {
        if (canDisable)
        {
            _panelManager.ClosePanel();
            _levelUpPanel.SetActive(false);
            _experienceManager.CloseLvlUpPanel();
        }
    }
    public void LevelUpRewards(int lvl)
    {
        switch (lvl)
        {
            case 0:
            case 1:
                break;
            case 3:
                ShowRewards(1,1,3,0,0);
                break;
            case 2:
            case 6:
                ShowRewards(1,1,0,0,0);
                break;
            case 4:
                ShowRewards(1, 0, 0, 0, 0);
                break;
            case 20:
                ShowRewards(1,0,0,2,1);
                break;
            case 5:
                ShowRewards(1, 0, 3, 0, 0);
                break;
            case 7:
            case 9:
            case 11:
            case 15:
                ShowRewards(1,0,3,0,1);
                break;
            case 8:
                ShowRewards(0,1,0,0,0);
                break;
            case 10:
                ShowRewards(0,1,0,2,1);
                break;
            case 13:
            case 17:
            case 19:
                ShowRewards(0,0,30,0,1);
                break;
            default:
                ShowRewards(0,0,100,1,1);
                break;
        }
    }

    public void ShowRewards(int cells, int expositors, int gems, int chestType, int chestAmount)
    {
        foreach (Transform t in _rewardsPanel)
        {
            Destroy(t.gameObject);
        }
        if (cells > 0)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell()), cells);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
        }
        if (expositors > 0)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor()), expositors);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
        }
        if (gems > 0)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(_gemIcon, gems);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
            _rewardManager.EarnHardCoin(gems);
        }
        if(chestAmount > 0 && UserDataController.GetBiggestDino() > 5)
        {
            GameObject nReward = Instantiate(_rewardsPrefab, _rewardsPanel.position, Quaternion.identity);
            nReward.GetComponent<RewardInstance>().SetRewards(_chestIcons[chestType], chestAmount);
            nReward.transform.SetParent(_rewardsPanel);
            nReward.transform.localScale = Vector3.one;
            _rewardManager.EarnLootBox(chestType, chestAmount);
        }
    }
}
