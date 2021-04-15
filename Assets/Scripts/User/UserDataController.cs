using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Analytics;
using System.Text;
public class UserDataController : MonoBehaviour
{

    public static UserData _currentUserData;
    public static string _fileName = "CurrentUserData.json";
    public static bool _checked;
    public static int lastSaveTime = -1;
    public static void Initialize()
    {
        _currentUserData = new UserData();
        SaveToFile();
        _checked = true;
    }
    public static DateTime GetLastSave()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._lastUpdatedTime));
    }
    public static List<int> GetNonMaxedCharacters()
    {
        List<int> canMaximizeCharacters = new List<int> ();
        for(int i =0; i< _currentUserData._characterLevel.Length; i++)
        {
            if (_currentUserData._biggestDino/2>i &&  _currentUserData._characterLevel[i] < 3 && GetRemainingCharacterFragments(i)>0)
            {
                canMaximizeCharacters.Add(i);
            }
        }
        if (canMaximizeCharacters.Count == 0)
        {
            canMaximizeCharacters.Add(0);
        }
        return canMaximizeCharacters;
    }
    public static List<int> GetNonMaxedSkins()
    {
        List<int> canMaximizeSkins = new List<int>();
        for (int i = 0; i < _currentUserData._extraSkinsUnlocked.Length; i++)
        {
            if (!_currentUserData._extraSkinsUnlocked[i]  && GetRemainingSkinFragments(i) > 0)
            {
                canMaximizeSkins.Add(i);
            }
        }
        if (canMaximizeSkins.Count == 0)
        {
            canMaximizeSkins.Add(0);
        }
        return canMaximizeSkins;
    }
    public static List<int> GetNonMaxedCells()
    {
        List<int> canMaximizeCells = new List<int>();
        for (int i = 0; i < _currentUserData._cellSkins.Length; i++)
        {
            if (!_currentUserData._cellSkins[i] && GetRemainingCellFragments(i) > 0)
            {
                canMaximizeCells.Add(i);
            }
        }
        if (canMaximizeCells.Count == 0)
        {
            canMaximizeCells.Add(0);
        }
        return canMaximizeCells;
    }
    public static List<int> GetNonMaxedExpositors()
    {
        List<int> canMaximizeExpositors = new List<int>();
        for (int i = 0; i < _currentUserData._expositorSkins.Length; i++)
        {
            if (!_currentUserData._expositorSkins[i] && GetRemainingExpositorFragments(i) > 0)
            {
                canMaximizeExpositors.Add(i);
            }
        }
        if (canMaximizeExpositors.Count == 0)
        {
            canMaximizeExpositors.Add(0);
        }
        return canMaximizeExpositors;
    }
    public static void OpenChest(int chestType)
    {
        _currentUserData._lootBoxes[chestType]--;
        SaveToFile();
    }
    public static List<int> GetNonMaxedGrounds()
    {
        List<int> canMaximizeGrounds = new List<int>();
        for (int i = 0; i < _currentUserData._groundSkins.Length; i++)
        {
            if (!_currentUserData._groundSkins[i] && GetRemainingGroundFragments(i) > 0)
            {
                canMaximizeGrounds.Add(i);
            }
        }
        if (canMaximizeGrounds.Count == 0)
        {
            canMaximizeGrounds.Add(0);
        }
        return canMaximizeGrounds;
    }
    public static DateTime GetLastPlayedDay()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._dailyRewardCheck));
    }
    public static DateTime GetVipExpireDate()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._vipExpireTime));
    }
    public static int GetSecondsSinceLastSave()
    {
        return lastSaveTime;
    }
    public static int GetDinoAmount()
    {
        return _currentUserData._dinosaurs.Length;
    }
    public static int GetPlayerAvatar()
    {
        return _currentUserData._playerAvatar;
    }
    public static void SetPlayerAvatar(int avatarIndex)
    {
        _currentUserData._playerAvatar = avatarIndex;
        SaveToFile();
    }
    public static int GetCurrentCell()
    {
        return _currentUserData._currentCell;
    }
    public static void SetCurrentCell(int cellIndex)
    {
        _currentUserData._currentCell = cellIndex;
        SaveToFile();
    }
    public static void SetCurrentExpositor(int expositorIndex)
    {
        _currentUserData._currentExpositor = expositorIndex;
        SaveToFile();
    }
    public static int GetCurrentExpositor()
    {
        return _currentUserData._currentExpositor;
    }
    public static int GetChibiSkinsNumber()
    {
        return _currentUserData._skins.Length;
    }
    public static void InitializeUser()
    {
        _currentUserData = new UserData();
        SaveToFile();
        _checked = true;
    }

    public static void ChangeName(string username)
    {
        _currentUserData._username = username;
        SaveToFile();
    }
    public static void AddWatchedVideo()
    {
        _currentUserData._currentRewardVideos++;
        if(_currentUserData._currentRewardVideos >= 7)
        {
            _currentUserData._currentRewardVideos = 1;
        }
    }
    public static int GetWatchedVideos()
    {
        return _currentUserData._currentRewardVideos;
    }
    public static void SetUserData()
    {
        if (_currentUserData == null)
        {
            Initialize();
        }

        SaveToFile();
    }

    public static int GetFreeSpinTries()
    {
        return _currentUserData._freeSpinTries;
    }
    public static void SetFreeSpinTries(int tries)
    {
        _currentUserData._freeSpinTries = tries;
        SaveToFile();
    }
    public static void SetCheckedExperience()
    {
        _currentUserData._checkedExperience = true;
    }

    public static bool GetCheckedExperience()
    {
        return _currentUserData._checkedExperience;
    }

    public static void LoadFromFile()
    {
        string str = File.ReadAllText(Application.persistentDataPath + "/" + _fileName);
        string recodedString = EncryptDecrypt(str);
        _currentUserData = JsonUtility.FromJson<UserData>(recodedString);
        if(_currentUserData._cellSkins.Length != UserData.UserDataValues.cellSkins)
        {
            Array.Resize<bool>(ref _currentUserData._cellSkins, UserData.UserDataValues.cellSkins);
        }
        if (_currentUserData._expositorSkins.Length != UserData.UserDataValues.expositorSkins)
        {
            Array.Resize<bool>(ref _currentUserData._expositorSkins, UserData.UserDataValues.expositorSkins);
        }
        if (_currentUserData._groundSkins.Length != UserData.UserDataValues.groundSkins)
        {
            Array.Resize<bool>(ref _currentUserData._groundSkins, UserData.UserDataValues.groundSkins);
        }
        if (_currentUserData._framesSkins.Length != UserData.UserDataValues.frameSkins)
        {
            Array.Resize<bool>(ref _currentUserData._framesSkins, UserData.UserDataValues.frameSkins);
        }
        if (_currentUserData._dinosaurs.Length != UserData.UserDataValues.dinosaurs)
        {
            int previousLength = _currentUserData._dinosaurs.Length;

            Array.Resize<int>(ref _currentUserData._dinosaurs, UserData.UserDataValues.dinosaurs);
            for(int i = previousLength; i< UserData.UserDataValues.dinosaurs; i++)
            {
                _currentUserData._dinosaurs[i] = -1;
            }
        }
        if (_currentUserData._extraSkinsFragments.Length != UserData.UserDataValues.extraSkins)
        {
            Array.Resize<int>(ref _currentUserData._extraSkinsFragments, UserData.UserDataValues.extraSkins);
        }
        if(_currentUserData._extraSkinsUnlocked.Length != UserData.UserDataValues.extraSkins)
        {
            Array.Resize<bool>(ref _currentUserData._extraSkinsUnlocked, UserData.UserDataValues.extraSkins);
        }
        if (_currentUserData._skins.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._skins, UserData.UserDataValues.dinosaurs*2);
        }
        if (_currentUserData._specialCards.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._specialCards, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._galleryImagesToOpen.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._galleryImagesToOpen, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._purchasedTimes.Length != UserData.UserDataValues.dinosaurs*2)
        {
            Array.Resize<int>(ref _currentUserData._purchasedTimes, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._obtainedTimes.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<int>(ref _currentUserData._obtainedTimes, UserData.UserDataValues.dinosaurs * 2);
        }
        if(_currentUserData._characterLevel.Length!= UserData.UserDataValues.dinosaurs / 2)
        {
            Array.Resize<int>(ref _currentUserData._characterLevel, UserData.UserDataValues.dinosaurs / 2);
        }
        if (_currentUserData._characterFragments.Length != UserData.UserDataValues.dinosaurs / 2)
        {
            Array.Resize<int>(ref _currentUserData._characterFragments, UserData.UserDataValues.dinosaurs / 2);
        }
        if (_currentUserData._cellSkinsFragments.Length != UserData.UserDataValues.cellSkins)
        {
            Array.Resize<int>(ref _currentUserData._cellSkinsFragments, UserData.UserDataValues.cellSkins);
        }
        if (_currentUserData._expositorSkinsFragments.Length != UserData.UserDataValues.expositorSkins)
        {
            Array.Resize<int>(ref _currentUserData._expositorSkinsFragments, UserData.UserDataValues.expositorSkins);
        }
        if (_currentUserData._groundSkinsFragments.Length != UserData.UserDataValues.groundSkins)
        {
            Array.Resize<int>(ref _currentUserData._groundSkinsFragments, UserData.UserDataValues.groundSkins);
        }
        _checked = true;
        for(int i = 0; i< _currentUserData._characterLevel.Length; i++)
        {
            
            switch (_currentUserData._characterLevel[i])
            {
                case 0:
                    _currentUserData._specialCards[i * 4] = true;
                    break;

                case 1:
                    _currentUserData._specialCards[i * 4] = true;
                    _currentUserData._specialCards[(i * 4) + 1] = true;
                    break;

                case 2:
                    _currentUserData._specialCards[i * 4] = true;
                    _currentUserData._specialCards[(i * 4)+1] = true;
                    _currentUserData._specialCards[(i * 4) + 2] = true;
                    break;

                case 3:
                    _currentUserData._specialCards[i * 4] = true;
                    _currentUserData._specialCards[(i * 4) + 1] = true;
                    _currentUserData._specialCards[(i * 4) + 2] = true;
                    _currentUserData._specialCards[(i * 4) + 3] = true;
                    break;
            }
        }
    }

    public static bool CanLevelUp(int character)
    {
        bool canLevelUp = false;
        switch (GetCharacterLevel(character))
        {
            case 0:
                if (GetCharacterFragments(character) >= GameData.characterFragmentCapsByLevel[0])
                {
                    canLevelUp = true;
                }
                break;

            case 1:
                if (GetCharacterFragments(character) >= GameData.characterFragmentCapsByLevel[1])
                {
                    canLevelUp = true;
                }
                break;

            case 2:
                if (GetCharacterFragments(character) >= GameData.characterFragmentCapsByLevel[2])
                {
                    canLevelUp = true;
                }
                break;

            case 3:
                canLevelUp = false;
                break;
        }
        return canLevelUp;
    }
    public static void LevelUpCharacter(int character)
    {
        if (CanLevelUp(character))
        {
            _currentUserData._characterLevel[character]++;
            _currentUserData._characterFragments[character] = 0;
            switch (_currentUserData._characterLevel[character])
            {
                case 0:
                    _currentUserData._specialCards[character * 4] = true;
                    break;

                case 1:
                    _currentUserData._specialCards[(character * 4) + 1] = true;
                    break;

                case 2:
                    _currentUserData._specialCards[(character * 4) + 2] = true;
                    break;

                case 3:
                    _currentUserData._specialCards[(character * 4) + 3] = true;
                    break;
            }
            SaveToFile();
        }
    }
    public static bool IsExtraSkinUnlocked(int skin)
    {
        return _currentUserData._extraSkinsUnlocked[skin];
    }
    public static int GetExtraSkinFragments(int extraSkin)
    {
        return _currentUserData._extraSkinsFragments[extraSkin];
    }
    public static void AddExtraSkinFragments(int extraSkin, int fragments)
    {
        _currentUserData._extraSkinsFragments[extraSkin] += fragments;
        
        int maxFragments = GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[extraSkin]._rarity];
        if(GetExtraSkinFragments(extraSkin)> maxFragments)
        {
            _currentUserData._characterFragments[extraSkin] = maxFragments;
        }
        SaveToFile();
    }
    public static void UnlockExtraSkin(int extraSkin)
    {
        _currentUserData._extraSkinsUnlocked[extraSkin] = true;
        SaveToFile();
    }

    public static int GetCharacterFragments(int character)
    {
        return _currentUserData._characterFragments[character];
    }
    public static void AddCharacterFragments(int character, int fragments)
    {
        _currentUserData._characterFragments[character] += fragments;
        
        int maxFragments = GameData.characterFragmentCapsByLevel[GetCharacterLevel(character)];
        if (GetCharacterFragments(character) > maxFragments)
        {
            _currentUserData._characterFragments[character] = maxFragments;
        }
        SaveToFile();
    }
    public static int GetRemainingSkinFragments(int skin)
    {
        int maxFragments = GameData.specialSkinsFragmentCapsByRarity[SpecialSkinsManager._specialSkins[skin]._rarity];
        int currentFragments = GetExtraSkinFragments(skin);
        int remainingFragments = maxFragments - currentFragments;
        return remainingFragments;
    }
    public static int GetRemainingCellFragments(int cellIndex)
    {
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[cellIndex]._rarity];
        int currentFragments = _currentUserData._cellSkinsFragments[cellIndex];
        int remainingFragments = maxFragments - currentFragments;
        return remainingFragments;
    }
    public static int GetRemainingExpositorFragments(int expIndex)
    {
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[expIndex]._rarity];
        int currentFragments = _currentUserData._expositorSkinsFragments[expIndex];
        int remainingFragments = maxFragments - currentFragments;
        return remainingFragments;
    }
    public static int GetChestAmount(int chestType)
    {
        return _currentUserData._lootBoxes[chestType];
    }
    public static int GetTotalChestAmount()
    {
        int sum = 0;
        for(int i = 0; i<_currentUserData._lootBoxes.Length; i++)
        {
            sum += _currentUserData._lootBoxes[i];
        }
        return sum;
    }
    public static void AddLootBoxes(int type, int amount)
    {
        _currentUserData._lootBoxes[type] += amount;
        SaveToFile();
    }
    public static int GetRemainingGroundFragments(int groundIndex)
    {
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[groundIndex]._rarity];
        int currentFragments = _currentUserData._groundSkinsFragments[groundIndex];
        int remainingFragments = maxFragments - currentFragments;
        return remainingFragments;
    }
    public static int GetRemainingCharacterFragments(int character)
    {
        int maxFragments = GameData.characterFragmentCapsByLevel[GetCharacterLevel(character)];
        int currentFragments = GetCharacterFragments(character);
        int remainingFragments = maxFragments - currentFragments;
        return remainingFragments;
    }
    public static void RemoveSkinFragments(int skin)
    {
        _currentUserData._extraSkinsFragments[skin] = 0;
        SaveToFile();
    }
    public static void RemoveUnlockedSkin(int skin)
    {
        _currentUserData._extraSkinsUnlocked[skin] = false;
        SaveToFile();
    }
    public static void AddCellFragments(int cell, int fragments)
    {
        _currentUserData._cellSkinsFragments[cell] += fragments;
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._cellSkins[cell]._rarity];
        if (_currentUserData._cellSkinsFragments[cell] > maxFragments)
        {
            _currentUserData._cellSkinsFragments[cell] = maxFragments;
        }
        SaveToFile();
    }
    public static void AddExpositorFragments(int expositor, int fragments)
    {
        _currentUserData._expositorSkinsFragments[expositor] += fragments;
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._expositorSkins[expositor]._rarity];
        if (_currentUserData._expositorSkinsFragments[expositor] > maxFragments)
        {
            _currentUserData._expositorSkinsFragments[expositor] = maxFragments;
        }
        SaveToFile();
    }
    public static void AddGroundFragments(int ground, int fragments)
    {
        _currentUserData._groundSkinsFragments[ground] += fragments;
        int maxFragments = GameData.customiceFragmentsCapByRarity[CustomizeSkinsManager._groundSkins[ground]._rarity];
        if (_currentUserData._groundSkinsFragments[ground] > maxFragments)
        {
            _currentUserData._groundSkinsFragments[ground] = maxFragments;
        }
        SaveToFile();
    }
    public static void ResetCharacterFragments(int character)
    {
        _currentUserData._characterFragments[character] = 0;
        SaveToFile();
    }

    public static int GetCharacterLevel(int character)
    {
        return _currentUserData._characterLevel[character];
    }
    public static void DeleteFile()
    {
        File.Delete(Application.persistentDataPath + "/" + _fileName);
    }
    public static bool IsSkinUnlocked(int skinIndex)
    {
        return _currentUserData._skins[skinIndex];
    }
    public static bool IsSpecialCardUnlocked(int skinIndex)
    {
        return _currentUserData._specialCards[skinIndex];
    }
    public static void UnlockSpecialCard(int skinIndex)
    {
        _currentUserData._specialCards[skinIndex] = true;
        SaveToFile();
    }
    public static bool CheckSpecialOffer()
    {
        return _currentUserData._specialOffer;
    }

    public static void PurchaseSpecialOffer(int packIndex)
    {
        _currentUserData._specialOffer = true;
        UnlockCellSkin(packIndex);
        UnlockExpositorSkin(packIndex);
        UnlockGroundSkin(packIndex);
        UnlockFrameSkin(packIndex);
        SaveToFile();
    }

    public static bool HasBeenVip()
    {
        return _currentUserData._hasBeenVip;
    }

    public static void UnlockPack(int packIndex)
    {
        UnlockCellSkin(packIndex);
        UnlockExpositorSkin(packIndex);
        UnlockGroundSkin(packIndex);
    }

    public static void UnlockSkin(int skinIndex)
    {
        _currentUserData._skins[skinIndex] = true;
        SaveToFile();
    }
    public static void UnlockCellSkin(int skinIndex)
    {
        _currentUserData._cellSkins[skinIndex] = true;
        SaveToFile();
    }
    public static void UnlockExpositorSkin(int skinIndex)
    {
        _currentUserData._expositorSkins[skinIndex] = true;
        SaveToFile();
    }
    public static void UnlockGroundSkin(int skinIndex)
    {
        _currentUserData._groundSkins[skinIndex] = true;
        SaveToFile();
    }
    public static void UnlockFrameSkin(int skinIndex)
    {
        _currentUserData._framesSkins[skinIndex] = true;
        SaveToFile();
    }
    public static bool IsVipUser()
    {
        return _currentUserData._vipUser;
    }
    public static void ActiveVip()
    {
        _currentUserData._vipUser = true;
        _currentUserData._hasBeenVip = true;
        _currentUserData._vipExpireTime = System.DateTime.Now.AddDays(30).ToBinary().ToString();
        SaveToFile();
    }
    public static void VipExpire()
    {
        _currentUserData._vipExpireTime = System.DateTime.Now.ToBinary().ToString();
        _currentUserData._vipUser = false;
        SaveToFile();
    }
    public static int GetUnlockedSkinsNumber()
    {
        int skins = 0;
        for(int i = 0; i< _currentUserData._skins.Length; i++)
        {
            if (_currentUserData._skins[i])
            {
                skins++;
            }
        }
        return skins;
    }

    public static int key = 129;

    public static int GetSkinNumber()
    {
        return _currentUserData._skins.Length;
    }
    public static bool GetFreeCustomizedReward()
    {
        return _currentUserData._freeCustomizedPackage;
    }

    public static void ObtainFreeCustomizeReward()
    {
        _currentUserData._freeCustomizedPackage = true;
        SaveToFile();
    }

    public static string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
    public static void SaveToFile()
    {
        if(lastSaveTime < 0)
        {
            DateTime lastUpdatedTime = DateTime.FromBinary(Convert.ToInt64(_currentUserData._lastUpdatedTime));
            DateTime now = System.DateTime.Now;
            TimeSpan elapsedTime = now.Subtract(lastUpdatedTime);
            int elapsedSeconds = (int)elapsedTime.TotalSeconds;
            lastSaveTime = elapsedSeconds;
        }
        

        _currentUserData._lastUpdatedTime = System.DateTime.Now.ToBinary().ToString();
        string str = JsonUtility.ToJson(_currentUserData);
        string encodedString = EncryptDecrypt(str);
        File.WriteAllText(Application.persistentDataPath + "/" + _fileName, encodedString);
    }
    private void Start()
    {
        lastSaveTime = -1;
    }
    public static bool Exist()
    {
        return File.Exists(Application.persistentDataPath + "/" + _fileName);
    }
    
    public static void AddCell()
    {
        _currentUserData._unlockedCells ++;
        SaveToFile();
    }
    public static void AddExpositor()
    {
        _currentUserData._unlockedExpositors++;
        SaveToFile();
    }

    public static bool[] GetExpositorsSkins()
    {
        return _currentUserData._expositorSkins;
    }
    public static bool[] GetCellsSkins()
    {
        return _currentUserData._cellSkins;
    }
    public static bool[] GetGroundSkins()
    {
        return _currentUserData._groundSkins;
    }
    public static bool[] GetFramesSkins()
    {
        return _currentUserData._framesSkins;
    }
    public static void SetCurrentGround(int groundIndex)
    {
        _currentUserData._currentGround = groundIndex;
        SaveToFile();
    }
    public static int GetCurrentGround()
    {
        return _currentUserData._currentGround;
    }
    public static int GetCurrentFrame()
    {
        return 0;
    }
    public static bool HasSeenGalleryTutorial()
    {
        return _currentUserData._haswatchedGalleryTutorial;
    }
    public static bool HasSeenGalleryTutorial2()
    {
        return _currentUserData._haswatchedGalleryTutorial2;
    }
    public static void WatchGalleryTutorial()
    {
        _currentUserData._haswatchedGalleryTutorial = true;
        SaveToFile();
    }
    public static void WatchSecondGalleryTutorial()
    {
        _currentUserData._haswatchedGalleryTutorial2 = true;
        SaveToFile();
    }
    public static void SetCurrentFrame(int frameIndex)
    {
        _currentUserData._currentFrame = frameIndex;
        SaveToFile();
    }
    public static float GetExperienceAmount()
    {
        CalculateLevel();
        float amount;
        float baseExp = ExperienceManager.experienceCost[UserDataController.GetLevel() - 1];
        float targetExp = ExperienceManager.experienceCost[UserDataController.GetLevel()];
        float currentExp = _currentUserData._experience;
        float diff = targetExp - baseExp;
        float currentDiff = currentExp - baseExp;
        amount = currentDiff / diff;
        return amount;
    }
    public static void SetExperience(int exp)
    {
        _currentUserData._experience = exp;
    }
    public static void AddExperiencePoints(int expAmount)
    {
        CalculateLevel();
        int preLevel = _currentUserData._level;
        _currentUserData._experience += expAmount;
        CalculateLevel();
        int postLevel = _currentUserData._level;
        if(preLevel < postLevel)
        {
            GameEvents.LevelUp.Invoke(postLevel);
        }
    }

    public static bool IsGoingToLvlUp(int expAmount)
    {
        int preLevel = _currentUserData._level;
        int level = 0;
        do
        {
            level++;
        }
        while ((_currentUserData._experience+expAmount) >= ExperienceManager.experienceCost[level]);

        if (level > _currentUserData._level)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CalculateLevel()
    {
        int currentUserDataLevel = _currentUserData._level;
        int level = 0;
        do
        {
            level++;
        }
        while (_currentUserData._experience >= ExperienceManager.experienceCost[level]);
        if (currentUserDataLevel != level)
        {
            _currentUserData._level = level;
            SaveToFile();
        }
        
    }
    public static int GetLevel()
    {
        return _currentUserData._level;
    }
    public static void SetLevel(int level)
    {
        _currentUserData._level = level;
    }
    public static void SaveTutorial(int tutorialIndex)
    {
        _currentUserData._tutorialCompleted[tutorialIndex] = true;
        SaveToFile();
    }

    public static void BuyDinosaur(int dinosaurIndex)
    {
        for (int i = 0; i < _currentUserData._unlockedCells; i++)
        {
            if (_currentUserData._dinosaurs[i] == -1)
            {
                _currentUserData._dinosaurs[i] = dinosaurIndex;
                break;
            }
        }
        _currentUserData._purchasedTimes[dinosaurIndex]++;
        _currentUserData._obtainedTimes[dinosaurIndex]++;
        SaveToFile();
    }

    public static void CreateDinosaur(int targetCell, int dinosaurIndex) 
    {
        _currentUserData._dinosaurs[targetCell] = dinosaurIndex;
        SaveToFile();
    }
    public static void CreateBox(BoxManager.BoxType boxType,  int targetCell, int boxIndex)
    {
        int sum = 0;
        switch (boxType)
        {
            case BoxManager.BoxType.StandardBox:
                sum = 100;
                break;
            case BoxManager.BoxType.RewardedBox:
                sum = 200;
                break;
            case BoxManager.BoxType.LootBox:
                sum = 300;
                break;
        }
        _currentUserData._dinosaurs[targetCell] = boxIndex + sum;
        SaveToFile();
    }

    public static int GetEmptyCells()
    {
        int unlockedCells = 0;
        for(int i = 0; i<_currentUserData._unlockedCells; i++)
        {
            if(_currentUserData._dinosaurs[i] == -1)
            {
                unlockedCells++;
            }
        }
        return unlockedCells;
    }
    public static void RemoveCustomizeData()
    {
        for(int i = 0; i< _currentUserData._cellSkins.Length; i++)
        {
            _currentUserData._cellSkins[i] = false;
        }
        _currentUserData._cellSkins[0] = true;


        for (int i = 0; i < _currentUserData._expositorSkins.Length; i++)
        {
            _currentUserData._expositorSkins[i] = false;
        }
        _currentUserData._expositorSkins[0] = true;

        for (int i = 0; i < _currentUserData._groundSkins.Length; i++)
        {
            _currentUserData._groundSkins[i] = false;
        }
        _currentUserData._groundSkins[0] = true;

        for (int i =0; i< _currentUserData._cellSkinsFragments.Length; i++)
        {
            _currentUserData._cellSkinsFragments[i] = 0;
        }
        for (int i = 0; i < _currentUserData._expositorSkinsFragments.Length; i++)
        {
            _currentUserData._expositorSkinsFragments[i] = 0;
        }
        for (int i = 0; i < _currentUserData._groundSkinsFragments.Length; i++)
        {
            _currentUserData._groundSkinsFragments[i] = 0;
        }
        SaveToFile();
        
    }
    public static int GetCellFragments(int cellIndex)
    {
        return _currentUserData._cellSkinsFragments[cellIndex];
    }
    public static int GetExpositorFragments(int expositorIndex)
    {
        return _currentUserData._expositorSkinsFragments[expositorIndex];
    }
    public static int GetGroundFragments(int groundIndex)
    {
        return _currentUserData._groundSkinsFragments[groundIndex];
    }
    public static int GetEmptyExpositors()
    {
        int unlockedExpositors = 0;
        for (int i = 0; i < _currentUserData._unlockedExpositors; i++)
        {
            if (_currentUserData._workingCellsByExpositor[i] == -1)
            {
                unlockedExpositors++;
            }
        }
        return unlockedExpositors;
    }

    public static bool HaveMoney(GameCurrency cost)
    {
        bool haveMoney = false;
        if(new GameCurrency(_currentUserData._softCoins).CompareCurrencies(cost))
        {
            haveMoney = true;
        }
        return haveMoney;
    }

    public static bool HaveGems(int amount)
    {
        bool haveGems = false;
        if((_currentUserData._hardCoins - amount) >= 0)
        {
            haveGems = true;
        }
        return haveGems;
    }

    public static void MoveDinosaur(int cellIndex1, int cellIndex2)
    {
        int aux = _currentUserData._dinosaurs[cellIndex2];
        _currentUserData._dinosaurs[cellIndex2] = _currentUserData._dinosaurs[cellIndex1];
        _currentUserData._dinosaurs[cellIndex1] = aux;
        SaveToFile();
    }

    public static void DeleteDino(int dinocell)
    {
        _currentUserData._dinosaurs[dinocell] = -1;
        SaveToFile();
    }

    public static bool MergeDinosaurs(int originCell, int targetCell, int mergeDinoType)
    {
        bool mergeUp = false;
        if((mergeDinoType + 1) > GetBiggestDino())
        {
            SetBiggestDino(mergeDinoType + 1);
            mergeUp = true;
        }
        _currentUserData._obtainedTimes[mergeDinoType + 1]++;
        _currentUserData._dinosaurs[originCell] = -1;
        _currentUserData._dinosaurs[targetCell] = mergeDinoType +1;
        SaveToFile();
        return mergeUp;
    }
    public static void ShowCell(int cell, int expositor)
    {
        _currentUserData._workingCellsByExpositor[expositor] = cell;
        SaveToFile();
    }
    public static void StopShowCell(int expositor)
    {
        _currentUserData._workingCellsByExpositor[expositor] = -1;
        SaveToFile();
    }

    public static int GetExpositorIndexByCell(int cell)
    {
        int foundedExpositor = -1;
        for (int i = 0; i < _currentUserData._workingCellsByExpositor.Length; i++)
        {
            if(_currentUserData._workingCellsByExpositor[i] == cell)
            {
                foundedExpositor = i;
                break;
            }
        }
        return foundedExpositor;
    }

    public static int GetObtainedDinosByDinotype(int dinoType)
    {
        return _currentUserData._obtainedTimes[dinoType];
    }
    public static void AddSoftCoins(GameCurrency softCoins)
    {
        GameCurrency gCToAdd = new GameCurrency((_currentUserData._softCoins));
        gCToAdd.AddCurrency(softCoins);
        _currentUserData._softCoins = gCToAdd.GetIntList();
        _currentUserData._totalSoftCoins = gCToAdd.GetIntList();
        SaveToFile();
    }


    public static void AddHardCoins(int amount)
    {
        _currentUserData._hardCoins += amount;
        SaveToFile();
    }
    public static void SpendHardCoins(int amount)
    {
        _currentUserData._hardCoins -= amount;
        SaveToFile();
    }
    public static void SpendSoftCoins(GameCurrency amount)
    {
        GameCurrency g = new GameCurrency(_currentUserData._softCoins);
        g.SubstractCurrency(amount);
        _currentUserData._softCoins = g.GetIntList();
        SaveToFile();
    }
    public static int GetHardCoins()
    {
        return _currentUserData._hardCoins;
    }

    public static GameCurrency GetTotalEarnings()
    {
        GameCurrency t = new GameCurrency(_currentUserData._totalSoftCoins);
        return t;
    }
    public static int GetOwnedDinosByDinoType(int dinoType)
    {
        return _currentUserData._purchasedTimes[dinoType];
    }

    public static int GetBiggestDino()
    {
        return _currentUserData._biggestDino;
    }

    public static int GetBiggestCharacter()
    {
        return (int)_currentUserData._biggestDino / 2;
    }

    public static void SetBiggestDino(int biggestDino)
    {
        _currentUserData._biggestDino = biggestDino;
        SaveToFile();
    }
    public static void SetCharacterLevel(int character, int level)
    {
        _currentUserData._characterLevel[character] = level;
    }
    public void SpeedUp(int secs)
    {
        _currentUserData._speedUpRemainingSecs = secs;
        _currentUserData._SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static DateTime GetLastSpeedUpTime()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._SpeedUpLastUse));
    }
    public static int GetSpeedUpRemainingSecs()
    {
        return _currentUserData._speedUpRemainingSecs;
    }
    public static void UpdateSpeedUpData(int remainingSecs)
    {
        _currentUserData._speedUpRemainingSecs = remainingSecs;
        _currentUserData._SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }

    public static bool[] GetGalleryImagesToOpen()
    {
        return _currentUserData._galleryImagesToOpen;
    }
    public static void SetGalleryImage(int index, bool state)
    {
        _currentUserData._galleryImagesToOpen[index] = state;
        SaveToFile();
    }

    public static int GetSpinRemainingAds()
    {
        return _currentUserData._spinRemainingAds;
    }
    public static DateTime GetSpinLastViewTime()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._spinLastViewedAd));
    }
    public static void UpdateSpinData(int remainingAds)
    {
        _currentUserData._spinRemainingAds = remainingAds;
        _currentUserData._spinLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static GameCurrency GetSoftCoins()
    {
        return new GameCurrency(_currentUserData._softCoins);
    }

    public static int GetDinoAmountByType(int dinoType)
    {
        int amount = 0;
        for(int i = 0; i<_currentUserData._dinosaurs.Length; i++)
        {
            if(_currentUserData._dinosaurs[i] == dinoType)
            {
                amount++;
            }
        }
        return amount;
    }
    public static List<int> GetFirstThreeDinosByType(int dinoType)
    {
        List<int> threeFirstDinos = new List<int>();

        for (int i = 0; i < _currentUserData._dinosaurs.Length; i++)
        {
            if (_currentUserData._dinosaurs[i] == dinoType)
            {
                threeFirstDinos.Add(i);
                if (threeFirstDinos.Count > 2)
                {
                    break;
                }
            }
        }
        return threeFirstDinos;
    }
    public static void AddPlayedDay()
    {
        _currentUserData._playedDays++;
        _currentUserData._dailyRewardCheck = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static int GetPlayedDays()
    {
        return _currentUserData._playedDays % 7;
    }
    public static void ClaimAchievement(int achievementID)
    {
        _currentUserData._claimedAchievements[achievementID] = true;
        SaveToFile();
    }
    public static void SetchievementToClaim(int index, bool state)
    {
        _currentUserData._achievementsToClaim[index] = state;
        SaveToFile();
    }
    public static bool[] GetAchievementsToClaim()
    {
        return _currentUserData._achievementsToClaim;
    }
    public static bool GetClaimedAchievement(int index)
    {
        return _currentUserData._claimedAchievements[index];
    }

    public static bool GetCompensationVipState()
    {
        return _currentUserData._compensationVip;
    }
    public static void ClaimCompensationVip()
    {
        _currentUserData._compensationVip = true;
        SaveToFile();
    }

    public static bool GetCompensationPassiveEarningsState()
    {
        return _currentUserData._compensationPassiveEarnings;
    }
    public static void ClaimCompensationPassiveEarnings()
    {
        _currentUserData._compensationPassiveEarnings = true;
        SaveToFile();
    }

    public static bool GetCompensationFramesState()
    {
        return _currentUserData._compensationFrames;
    }
    public static void ClaimCompensationFrames()
    {
        _currentUserData._compensationFrames = true;
        SaveToFile();
    }

    //DAILYMISSIONS MERGES
    public static int GetDailyMerges()
    {
        return _currentUserData._dailyMerges;
    }
    public static int GetDailyMergeLevel()
    {
        return _currentUserData._dailyMergeLevel;
    }
    public static void AddDailyMergeLevel()
    {
        _currentUserData._dailyMerges = 0;
        _currentUserData._dailyMergeLevel ++;
        SaveToFile();
    }
    public static void AddDailyMerge()
    {
        _currentUserData._dailyMerges++;
        SaveToFile();
    }

    //DAILYMISSIONS ADDS
    public static int GetDailySkinLevel()
    {
        return _currentUserData._dailySkinLevel;
    }
    public static void AddDailySkinLevel()
    {
        _currentUserData._dailySkinLevel++;
        SaveToFile();
    }

    //DAILYMISSIONS PURCHASES
    public static int GetDailyPurchases()
    {
        return _currentUserData._dailyPurchases;
    }
    public static int GetDailyPurchaseLevel()
    {
        return _currentUserData._dailyPurchaseLevel;
    }
    public static void AddDailyPurchaseLevel()
    {
        _currentUserData._dailyPurchases = 0;
        _currentUserData._dailyPurchaseLevel++;
        SaveToFile();
    }
    public static void AddDailyPurchase()
    {
        _currentUserData._dailyPurchases++;
        SaveToFile();
    }

    public static void RestoreDailyMissions()
    {
        _currentUserData._dailyMerges = 0;
        _currentUserData._dailyMergeLevel = 0;
        _currentUserData._dailyPurchases = 0;
        _currentUserData._dailyPurchaseLevel = 0;
    }
}
