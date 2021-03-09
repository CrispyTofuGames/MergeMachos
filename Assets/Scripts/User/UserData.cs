using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class UserData 
{
    public int _ID;
    public string _lastUpdatedTime;
    public int _experience;
    public int _level;
    public int[] _softCoins;
    public int[] _totalSoftCoins;
    public int _hardCoins;
    public int _unlockedCells;
    public int _unlockedExpositors;
    public string _username;
    public int[] _dinosaurs;
    public bool[] _skins;
    public bool[] _specialCards;
    public int[] _workingCellsByExpositor;
    public bool[] _tutorialCompleted;
    public int[] _purchasedTimes;
    public int[] _obtainedTimes;
    public bool[] _claimedAchievements;
    public bool[] _achievementsToClaim;
    public bool[] _galleryImagesToOpen;
    public int _currentCell;
    public bool[] _cellSkins;
    public bool[] _groundSkins;
    public bool[] _framesSkins;
    public int _currentExpositor;
    public int _currentGround;
    public int _currentFrame;
    public bool[] _expositorSkins;
    public int _spinRemainingAds;
    public string _spinLastViewedAd;
    public int _speedUpRemainingSecs;
    public string _SpeedUpLastUse;
    public string _dayCareLastViewedAd;
    public int _biggestDino;
    public string _dailyRewardCheck;
    public int _playedDays;
    public int _dailyMerges;
    public int _dailyMergeLevel;
    public int _dailySkinLevel;
    public int _dailyPurchases;
    public int _dailyPurchaseLevel;
    public int _playerAvatar;
    public int _currentRewardVideos;
    public int _freeSpinTries;
    public bool _missionWarning;
    public bool _specialOffer;
    public bool _vipUser;
    public bool _haswatchedGalleryTutorial;
    public bool _haswatchedGalleryTutorial2;
    public bool _compensationVip;
    public bool _compensationPassiveEarnings;
    public bool _compensationFrames;
    public string _vipExpireTime;
    public int[] _characterLevel;
    public int[] _characterFragments;
    public int[] _extraSkinsFragments;
    public bool[] _extraSkinsUnlocked;
    public bool suriipoCompensed;
    public bool _hasBeenVip;
    public bool _freeCustomizedPackage;
    public int[] _cellSkinsFragments;
    public int[] _skinsFragments;
    public int[] _expositorSkinsFragments;
    public int[] _groundSkinsFragments;
    public int[] _lootBoxes;
    public bool _checkedExperience;
    public UserData()
    {
        _lastUpdatedTime = System.DateTime.Now.ToBinary().ToString();
        _ID = 0;
        _level = 1;
        _softCoins = new int[31];
        _softCoins[0] = 7500;
        _totalSoftCoins = new int[31];
        _totalSoftCoins[0] = 20000;
        _hardCoins = 0; 
        _unlockedCells = 4;
        _unlockedExpositors = 4;
        _currentCell = 0;
        _currentExpositor = 0;
        _currentGround = 0;
        _currentFrame = 0;
        _cellSkins = new bool[UserDataValues.cellSkins];
        _cellSkins[0] = true;
        _expositorSkins = new bool[UserDataValues.expositorSkins];
        _expositorSkins[0] = true;
        _groundSkins = new bool[UserDataValues.groundSkins];
        _groundSkins[0] = true;
        _framesSkins = new bool[UserDataValues.frameSkins];
        _framesSkins[0] = true;
        _username = "Player";
        _dinosaurs = new int[UserDataValues.dinosaurs];
        _skins = new bool[UserDataValues.dinosaurs * 2];
        _skins[0] = true;
        _specialCards = new bool[UserDataValues.dinosaurs * 2];
        _specialCards[0] = true;
        _workingCellsByExpositor = new int[10];
        _spinRemainingAds = 3;
        _spinLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        _speedUpRemainingSecs = 0;
        _SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        _dayCareLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        _dailyRewardCheck = System.DateTime.Now.ToBinary().ToString();
        _playedDays = 0;
        _currentRewardVideos = 0;
        _freeSpinTries = 1;
        _missionWarning = false;
        _specialOffer = false;
        _vipUser = false;
        _compensationVip = false;
        _compensationPassiveEarnings = false;
        _compensationFrames = false;
        _vipExpireTime = "";
        _hasBeenVip = false;

        for (int i = 0; i<_dinosaurs.Length; i++)
        {
            _dinosaurs[i] = -1;
        }
        for (int i = 0; i < _workingCellsByExpositor.Length; i++)
        {
            _workingCellsByExpositor[i] = -1;
        }
        _purchasedTimes = new int[UserDataValues.dinosaurs*2]; //Numero de indices igual al de tipos de dinosaurio #x
        _obtainedTimes = new int[UserDataValues.dinosaurs*2]; //Numero de indices igual al de tipos de dinosaurio #x
        _tutorialCompleted = new bool[10];
        _claimedAchievements = new bool[10];
        _achievementsToClaim = new bool[_claimedAchievements.Length];
        _galleryImagesToOpen = new bool[_dinosaurs.Length*2];
        _galleryImagesToOpen[0] = true;
        _galleryImagesToOpen[1] = true;
        _dailyMerges = 0;
        _dailySkinLevel = 0;
        _dailyPurchases = 0;
        _playerAvatar = 0;
        _characterLevel = new int[_dinosaurs.Length / 2];
        _characterFragments= new int[_dinosaurs.Length / 2];
        _freeCustomizedPackage = false;
        _extraSkinsFragments = new int[UserDataValues.extraSkins];
        _extraSkinsUnlocked = new bool[UserDataValues.extraSkins];
        _cellSkinsFragments = new int[UserDataValues.cellSkins];
        _expositorSkinsFragments = new int[UserDataValues.expositorSkins];
        _groundSkinsFragments = new int[UserDataValues.groundSkins];
        _lootBoxes = new int[4];
        _checkedExperience = false;
    //    //Poner Para Desbloquear TODO
    //    _biggestDino = 30;
    //    _experience = 10000;
    //    _softCoins[0] = 999;
    //    _softCoins[1] = 999;
    //    _softCoins[2] = 999;
    //    _softCoins[3] = 999;
    //    _softCoins[4] = 999;
    //    _softCoins[5] = 999;
    //    _hardCoins = 1000;
    //    _unlockedCells = 12;
    //    _unlockedExpositors = 10;
    //    for (int i = 0; i < _cellSkins.Length; i++)
    //    {
    //        _cellSkins[i] = true;
    //    }
    //    for (int i = 0; i < _expositorSkins.Length; i++)
    //    {
    //        _expositorSkins[i] = true;
    //    }
    //    for (int i = 0; i < _groundSkins.Length; i++)
    //    {
    //        _groundSkins[i] = true;
    //    }
    //    for (int i = 0; i < _framesSkins.Length; i++)
    //    {
    //        _framesSkins[i] = true;
    //    }
    //    for (int i = 0; i < _skins.Length; i++)
    //    {

    //        if (i % 4 != 0 && i % 4 != 2)
    //        {
    //            _skins[i] = false;
    //        }
    //        else
    //        {
    //            _skins[i] = true;
    //        }
    //    }

    //    //for (int i = 0; i < _specialCards.Length; i++)
    //    //{
    //    //    _specialCards[i] = true;
    //    //}
    //    for (int i = 0; i < _tutorialCompleted.Length; i++)
    //    {
    //        _tutorialCompleted[i] = true;
    //    }

    //    _haswatchedGalleryTutorial = true;
    //    _haswatchedGalleryTutorial2 = true;
    
    }

    public static class UserDataValues
    {
        public static int cellSkins = 8;
        public static int expositorSkins = 8;
        public static int groundSkins = 8;
        public static int frameSkins = 4;
        public static int dinosaurs = 14;
        public static int extraSkins = 0;
    }
}