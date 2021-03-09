using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static List<int> experienceCost = new List<int>() {0, 4, 11, 27, 50, 109, 217, 394, 665, 1015, 1415, 1904, 2552, 3394, 4480, 4817, 5869, 6384, 7630, 9156, 10987, 13184, 15821,18985,22783,27339,32807,39369,47242,56691,68023,81627,97952,117542,141051,169261,203114,243736}; //TO DO AÑADIR NIVELES
    LevelUpPanelManager _levelUpPanelManager;
    bool waitingLvlUpPanel = false;

    void Start()
    {
        _levelUpPanelManager = FindObjectOfType<LevelUpPanelManager>();
        if (!UserDataController.GetCheckedExperience())
        {
            UserDataController.SetCheckedExperience();
            if (UserDataController.GetLevel() >= 19)
            {
                UserDataController.SetLevel(19);
                UserDataController.SetExperience(9000);
            }

        }
        GameEvents.MergeDino.AddListener(MergeDinoCallBack);
    }


    public void MergeDinoCallBack(int dinoType)
    {
        int preLevel = UserDataController.GetLevel();
        UserDataController.AddExperiencePoints(dinoType);
        int postLevel = UserDataController.GetLevel();
        if (postLevel > preLevel)
        {
            _levelUpPanelManager.LevelUp();
        }
    }
    public void AddExperience(int expAmount)
    {
        int preLevel = UserDataController.GetLevel();
        UserDataController.AddExperiencePoints(expAmount);
        int postLevel = UserDataController.GetLevel();
        if (postLevel > preLevel)
        {
            _levelUpPanelManager.LevelUp();
        }
    }
    public void CloseLvlUpPanel()
    {
        waitingLvlUpPanel = false;
    }
}
