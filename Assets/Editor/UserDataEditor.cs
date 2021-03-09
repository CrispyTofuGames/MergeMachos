using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class UserDataEditor : EditorWindow
{
    public static string _fileName = "CurrentUserData.json";
    bool groupEnabled;
    bool myBool = true;
    int _level;
    int[] _softCoins;
    int _hardCoins;
    int _unlockedCells;
    int _unlockedExpositors;
    int _selectedSeatSkin;
    int _selectedTableSkin;
    int _selectedGroundSkin;
    bool[] _seatSkins;
    bool[] _tableSkins;
    bool[] _groundSkins;
    int[] _dinosaurs;
    bool[] _skins;
    bool[] _shinySkins;
    UserData _currentUserData;
    bool _checked;
    [MenuItem("Window/UserDataValues")]
    public static void ShouwWindow()
    {
        GetWindow(typeof(UserDataEditor));
    }
    int selectedCharacter = 0;
    int selectedCharacterFragments = 0;
    int selectedSkin = 0;
    int selectedSkinFragments = 0;
    int selectedSkinToRemove = 0;
    int selectedCell =0;
    int selectedExpositor =0;
    int selectedGround = 0;
    int selectedCellFragments = 0;
    int selectedExpositorFragments = 0;
    int selectedGroundFragments = 0;
    int addLootBoxType;
    int addLootBoxAmount;
    void OnGUI()
    {
        int height = 20;

        //if (_checked)
        //{
        //    GUILayout.Label("UserDataValues", EditorStyles.boldLabel);
        //    EditorGUILayout.BeginHorizontal();
        //    EditorGUILayout.BeginVertical();
        //    GUILayout.Label("SoftCoins", GUILayout.Height(height));
        //    GUILayout.Label("HardCoins", GUILayout.Height(height));
        //    GUILayout.Label("Level", GUILayout.Height(height));
        //    GUILayout.Label("Unlocked Seats", GUILayout.Height(height));
        //    GUILayout.Label("Unlocked Expositor", GUILayout.Height(height));
        //    GUILayout.Label("Seat Skins", GUILayout.Height(height));
        //    GUILayout.Label("Selected Seat Skin", GUILayout.Height(height));
        //    GUILayout.Label("Tables Skins", GUILayout.Height(height));
        //    GUILayout.Label("Selected Table Skin", GUILayout.Height(height));
        //    GUILayout.Label("Ground Skins", GUILayout.Height(height));
        //    GUILayout.Label("Selected Ground Skin", GUILayout.Height(height));
        //    GUILayout.Label("Dinosaurs", GUILayout.Height(height));
        //    GUILayout.Label("Skins", GUILayout.Height(height));
        //    GUILayout.Label("Shiny Skins", GUILayout.Height(height));
        //    EditorGUILayout.EndVertical();
        //    EditorGUILayout.BeginVertical();
        //    EditorGUILayout.BeginHorizontal();

        //    for (int i =0; i< 5; i++)
        //    {
        //        _softCoins[i] = EditorGUILayout.IntField(_softCoins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    _hardCoins = EditorGUILayout.IntField( _hardCoins, GUILayout.Height(height));
        //    _level = EditorGUILayout.IntField(_level, GUILayout.Height(height));
        //    _unlockedCells = EditorGUILayout.IntField( _unlockedCells, GUILayout.Height(height));
        //    _unlockedExpositors = EditorGUILayout.IntField(_unlockedExpositors, GUILayout.Height(height));
        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _seatSkins.Length; i++)
        //    {
        //        _seatSkins[i] = EditorGUILayout.Toggle(_seatSkins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    _selectedSeatSkin = EditorGUILayout.IntField( _selectedSeatSkin, GUILayout.Height(height));



        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _tableSkins.Length; i++)
        //    {
        //        _tableSkins[i] = EditorGUILayout.Toggle(_tableSkins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    _selectedTableSkin = EditorGUILayout.IntField(_selectedTableSkin, GUILayout.Height(height));
        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _groundSkins.Length; i++)
        //    {
        //        _groundSkins[i] = EditorGUILayout.Toggle(_groundSkins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    _selectedGroundSkin = EditorGUILayout.IntField(_selectedGroundSkin, GUILayout.Height(height));
        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _dinosaurs.Length; i++)
        //    {
        //        _dinosaurs[i] = EditorGUILayout.IntField(_dinosaurs[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _skins.Length; i++)
        //    {
        //        _skins[i] = EditorGUILayout.Toggle(_skins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUILayout.BeginHorizontal();
        //    for (int i = 0; i < _shinySkins.Length; i++)
        //    {
        //        _shinySkins[i] = EditorGUILayout.Toggle(_shinySkins[i], GUILayout.Height(height));
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUILayout.EndVertical();
        //    EditorGUILayout.EndHorizontal();
        //    if (GUILayout.Button("Clear Data"))
        //    {
        //        _checked = false;
        //    }
        //    if (GUILayout.Button("Save Data"))
        //    {

        //    }
        //}
        //if (!_checked)
        //{
        //    if (GUILayout.Button("Load User Data Values"))
        //    {
        //        LoadFromFile();
        //    }
        //}
        
        
        if (GUILayout.Button("Add 1000 gems"))
        {
            UserDataController.AddHardCoins(1000);
        }
        if (GUILayout.Button("Add 1000 exp"))
        {
            FindObjectOfType<ExperienceManager>().AddExperience(1000);
        }
        //AddCharacterFragments

        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add Character Fragments", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<RewardManager>().EarnCharacterFragments(selectedCharacter,selectedCharacterFragments);
        }
        GUILayout.Label("Character:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedCharacter = EditorGUILayout.IntField(selectedCharacter, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Fragments:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedCharacterFragments= EditorGUILayout.IntField(selectedCharacterFragments, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

        //AddSkinsFragments
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add Skin Fragments", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.SkinFragments, selectedSkin, UserDataController.GetRemainingSkinFragments(selectedSkin));
            //FindObjectOfType<RewardManager>().EarnSkinFragments(selectedSkin, selectedSkinFragments);
        }

        GUILayout.Label("Skin:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedSkin = EditorGUILayout.IntField(selectedSkin, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Fragments:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedSkinFragments = EditorGUILayout.IntField(selectedSkinFragments, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));

        if (GUILayout.Button("Remove Skin", GUILayout.Width(200), GUILayout.Height(height)))
        {
            UserDataController.RemoveUnlockedSkin(selectedSkinToRemove);
            UserDataController.RemoveSkinFragments(selectedSkinToRemove);
        }

        GUILayout.Label("Skin:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedSkinToRemove = EditorGUILayout.IntField(selectedSkinToRemove, GUILayout.Width(20), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Remove all customize", GUILayout.Width(200), GUILayout.Height(height)))
        {
            UserDataController.RemoveCustomizeData();
        }
        EditorGUILayout.EndHorizontal();

        // ADD CELL FRAGMENTS
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add Cell Fragments", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<RewardManager>().EarnCellFragments(selectedCell, selectedCellFragments);
        }

        GUILayout.Label("Cell:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedCell = EditorGUILayout.IntField(selectedCell, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Fragments:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedCellFragments = EditorGUILayout.IntField(selectedCellFragments, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

        // ADD EXPOSITOR FRAGMENTS
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add Expositor Fragments", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<RewardManager>().EarnExpositorFragments(selectedExpositor, selectedExpositorFragments);
        }

        GUILayout.Label("Expositor:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedExpositor = EditorGUILayout.IntField(selectedExpositor, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Fragments:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedExpositorFragments = EditorGUILayout.IntField(selectedExpositorFragments, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

        // ADD GROUND FRAGMENTS
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add Ground Fragments", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<RewardManager>().EarnGroundFragments(selectedGround, selectedGroundFragments);
        }

        GUILayout.Label("Ground:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedGround = EditorGUILayout.IntField(selectedGround, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Fragments:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        selectedGroundFragments = EditorGUILayout.IntField(selectedGroundFragments, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

        //ADD LootBoxes
        EditorGUILayout.BeginHorizontal(GUILayout.Height(height));
        if (GUILayout.Button("Add LootBoxes", GUILayout.Width(200), GUILayout.Height(height)))
        {
            FindObjectOfType<RewardManager>().EarnLootBox(addLootBoxType, addLootBoxAmount);
        }

        GUILayout.Label("Type:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        addLootBoxType = EditorGUILayout.IntField(addLootBoxType, GUILayout.Width(20), GUILayout.Height(height));
        GUILayout.Label("Amount:", EditorStyles.boldLabel, GUILayout.Width(70), GUILayout.Height(height));
        addLootBoxAmount = EditorGUILayout.IntField(addLootBoxAmount, GUILayout.Width(40), GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();

    }

    public void LoadFromFile()
    {
        string str = File.ReadAllText(Application.persistentDataPath + "/" + _fileName);
        string recodedString = EncryptDecrypt(str);
        _currentUserData = JsonUtility.FromJson<UserData>(recodedString);
        if (_currentUserData._cellSkins.Length != UserData.UserDataValues.cellSkins)
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
            for (int i = previousLength; i < UserData.UserDataValues.dinosaurs; i++)
            {
                _currentUserData._dinosaurs[i] = -1;
            }
        }
        //if(_currentUserData._extraSkinsFragments.Length != UserData.UserDataValues.extraSkins)
        //{
        //    Array.Resize<int>(ref _currentUserData._dinosaurs, UserData.UserDataValues.extraSkins);
        //}
        if (_currentUserData._skins.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._skins, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._specialCards.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._specialCards, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._galleryImagesToOpen.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<bool>(ref _currentUserData._galleryImagesToOpen, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._purchasedTimes.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<int>(ref _currentUserData._purchasedTimes, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._obtainedTimes.Length != UserData.UserDataValues.dinosaurs * 2)
        {
            Array.Resize<int>(ref _currentUserData._obtainedTimes, UserData.UserDataValues.dinosaurs * 2);
        }
        if (_currentUserData._characterLevel.Length != UserData.UserDataValues.dinosaurs / 2)
        {
            Array.Resize<int>(ref _currentUserData._characterLevel, UserData.UserDataValues.dinosaurs / 2);
        }
        if (_currentUserData._characterFragments.Length != UserData.UserDataValues.dinosaurs / 2)
        {
            Array.Resize<int>(ref _currentUserData._characterFragments, UserData.UserDataValues.dinosaurs / 2);
        }
        _softCoins = _currentUserData._softCoins;
        _hardCoins = _currentUserData._hardCoins;
        _level = _currentUserData._level;
        _unlockedCells = _currentUserData._unlockedCells;
        _checked = true;
        _unlockedExpositors = _currentUserData._unlockedExpositors;
        _selectedSeatSkin = _currentUserData._currentCell;
        _selectedTableSkin = _currentUserData._currentExpositor;
        _selectedGroundSkin = _currentUserData._currentGround;
        _seatSkins = _currentUserData._cellSkins;
        _tableSkins = _currentUserData._expositorSkins;
        _groundSkins = _currentUserData._groundSkins;
        _dinosaurs = _currentUserData._dinosaurs;
        _skins = _currentUserData._skins;
        _shinySkins = _currentUserData._specialCards;
    }

    public static string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ 129);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
}
