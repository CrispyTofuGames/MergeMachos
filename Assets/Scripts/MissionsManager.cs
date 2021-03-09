using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Transform dailyMissionsNull;
    [SerializeField]
    Transform achievementsNull;
    [SerializeField]
    GameObject missionPrefab;
    [SerializeField]
    SOAchievement[] _sOAchievements;
    PanelManager _panelManager;
    [SerializeField]
    Sprite softCoinsIcon, hardCoinsIcon;
    [SerializeField]
    GameObject _warningIcon;
    [SerializeField]
    DailyMissionInstance[] _dailyMissionInstances;
    List<AchievementInstance> _achievementInstances = new List<AchievementInstance>();
    public void OpenMissions()
    {
        _panelManager.RequestShowPanel(_mainPanel);
    }
    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        StartCoroutine(WaitCr());
        GameEvents.MergeDino.AddListener(CallChecking);
        GameEvents.Purchase.AddListener(CallChecking);
    }
    public void CallChecking(int n)
    {
        CheckWarningState();
    }

    public void CloseMissions()
    {
        _mainPanel.SetActive(false);
    }
    IEnumerator WaitCr()
    {
        yield return null;
        CreateAchievements();
    }
    public void CreateAchievements()
    {
        for(int i = 0; i< 10; i++)
        {
            if (!UserDataController.GetClaimedAchievement(i))
            {
                GameObject missionInstance = Instantiate(missionPrefab, achievementsNull.transform.parent);
                string achievementTitle = string.Format(LocalizationController.GetValueByKey("ACHIEVEMENT_TITLE"), (_sOAchievements[i].dinoLevel + 1));
                missionInstance.GetComponent<AchievementInstance>().SetMissionInstance(i, achievementTitle, UserDataController.GetObtainedDinosByDinotype(_sOAchievements[i].dinoLevel), _sOAchievements[i].amount, hardCoinsIcon, _sOAchievements[i].rewardAmount, _sOAchievements[i].dinoLevel, this);
                _achievementInstances.Add(missionInstance.GetComponent<AchievementInstance>());
            }      
        }
        for (int i = 0; i < _dailyMissionInstances.Length; i++)
        {
            _dailyMissionInstances[i].Refresh();
        }
        CheckWarningState();
    }

    public void CheckWarningState()
    {
        bool state = false;
        for(int i = 0; i < _achievementInstances.Count; i++)
        {
            _achievementInstances[i].Refresh();
        }
        for(int i = 0; i<UserDataController.GetAchievementsToClaim().Length; i++)
        {
            if (!UserDataController.GetClaimedAchievement(i))
            {
                if (UserDataController.GetAchievementsToClaim()[i])
                {
                    state = true;
                }
            }
        }
        for(int i = 0; i<_dailyMissionInstances.Length; i++)
        {
            if (_dailyMissionInstances[i].GetState())
            {
                state = true;
            }
        }
        _warningIcon.SetActive(state);
    }
}
