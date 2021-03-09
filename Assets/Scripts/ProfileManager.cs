using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    Sprite _buttonSelected, _buttonGray;
    [SerializeField]
    TextMeshProUGUI[] txExtras, txExtraNumber;
    [SerializeField]
    TextMeshProUGUI txSProfits, txTProfits, txLevel;
    [SerializeField]
    Button[] _panelButtons;
    [SerializeField]
    EconomyManager _economyManager;
    [SerializeField]
    GameObject[] _profilePanels;
    [SerializeField]
    AnimationCurve animationCurve;
    List<Image> _avatarFaces;
    List<Button> _avatarButtons;
    [SerializeField]
    Image _avatar;
    [SerializeField]
    Button _sfxButton, _musicButton;
    bool _sfxState = true, _musicState = true;
    [SerializeField]
    GameObject _selectedBorderPrefab;
    GameObject _currentSelectedBorder;
    [SerializeField]
    PanelManager _panelManager;
    [SerializeField]
    TextMeshProUGUI txGameVersion;
    [SerializeField]
    GameObject _avatarPrefab;
    [SerializeField]
    GameObject _avatarGrid;
    [SerializeField]
    AudioMixer _audioMixer;
    bool profileOpen = false;
    UpgradesManager _upgradesManager;

    private void Start()
    {
        _upgradesManager = FindObjectOfType<UpgradesManager>();
        _avatarFaces = new List<Image>();
        _avatarButtons = new List<Button>();
        if (PlayerPrefs.HasKey("SFX"))
        {
            if(PlayerPrefs.GetInt("SFX") == 0)
            {
                _sfxState = false;
                _sfxButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            }
            else
            {
                _sfxButton.GetComponent<Image>().color = Color.white;
            }
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                _musicState = false;
                _musicButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            }
            else
            {
                _musicButton.GetComponent<Image>().color = Color.white;
            }
        }
        for(int i = 0; i<UserDataController.GetDinoAmount(); i++)
        {
            GameObject avatarPanel = Instantiate(_avatarPrefab, _avatarGrid.transform);
            _avatarFaces.Add(avatarPanel.transform.GetChild(0).GetComponent<Image>());
            _avatarButtons.Add(avatarPanel.GetComponent<Button>());
        }    
    }

    public void OpenProfile()
    {
        if (!profileOpen)
        {
            _panelManager.RequestShowPanel(_mainPanel);
            profileOpen = true;
            txExtras[0].text = LocalizationController.GetValueByKey("PROFILE_EXTRAS_1");
            txExtras[1].text = LocalizationController.GetValueByKey("PROFILE_EXTRAS_2");
            txExtras[2].text = LocalizationController.GetValueByKey("PROFILE_EXTRAS_3");
            txExtras[3].text = LocalizationController.GetValueByKey("PROFILE_EXTRAS_4");
            txExtraNumber[0].text = _upgradesManager.GetExtraEarnings() + "%";
            txExtraNumber[1].text = _upgradesManager.GetDiscount() + "%";
            txExtraNumber[2].text = _upgradesManager .GetExtraTouristSpeed() + "%";
            txExtraNumber[3].text = _upgradesManager.GetExtraPassiveEarnings() + "%";

            txSProfits.text = _economyManager.GetEarningsPerSecond();
            txTProfits.text = UserDataController.GetTotalEarnings().GetCurrentMoney();
            txLevel.text = UserDataController.GetLevel().ToString();
            txGameVersion.text = "V." + Application.version;

            for (int i = 0; i < UserDataController.GetDinoAmount(); i++)
            {
                int auxI = i;
                _avatarFaces[i].sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + i);
                _avatarButtons[i].onClick.AddListener(() => ChooseAvatar(auxI));
                if (i < UserDataController.GetBiggestDino())
                {
                    _avatarFaces[i].color = Color.white;
                }
                else
                {
                    _avatarFaces[i].color = Color.black;
                }
            }
            _avatar.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + UserDataController.GetPlayerAvatar());
            if (_currentSelectedBorder != null)
            {
                Destroy(_currentSelectedBorder);
            }
            _currentSelectedBorder = Instantiate(_selectedBorderPrefab, _avatarFaces[UserDataController.GetPlayerAvatar()].transform.parent);
        }
    }
    public void CloseProfile()
    {
        _panelManager.ClosePanel();
        profileOpen = false;
    }

    public void ChooseAvatar(int avatarIndex)
    {
        if(avatarIndex < UserDataController.GetBiggestDino())
        {
            UserDataController.SetPlayerAvatar(avatarIndex);
            if(_currentSelectedBorder != null)
            {
                Destroy(_currentSelectedBorder);
            }
            _currentSelectedBorder = Instantiate(_selectedBorderPrefab, _avatarFaces[UserDataController.GetPlayerAvatar()].transform.parent);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOT_UNLOCKED"));
        }
    }

    public void SFXButton()
    {
        _sfxState = !_sfxState;
        PlayerPrefs.SetInt("SFX", _sfxState ? 1 : 0);
        if (_sfxState)
        {
            _sfxButton.GetComponent<Image>().color = Color.white;
            _audioMixer.SetFloat("SFXVolume", 0);
        }
        else
        {
            _sfxButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            _audioMixer.SetFloat("SFXVolume", -80f);
        }
    }
    public void MusicButton()
    {
        _musicState = !_musicState;
        PlayerPrefs.SetInt("Music", _musicState ? 1 : 0);
        if (_musicState)
        {
            _musicButton.GetComponent<Image>().color = Color.white;
            _audioMixer.SetFloat("OSTVolume", 0);
        }
        else
        {
            _musicButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            _audioMixer.SetFloat("OSTVolume", -80f);
        }
    }

    public void OpenPanel(int panel)
    {
        for(int i = 0; i<_profilePanels.Length; i++)
        {
            _profilePanels[i].SetActive(false);
            _panelButtons[i].GetComponent<Image>().sprite = _buttonGray;
        }
        _profilePanels[panel].SetActive(true);
        _panelButtons[panel].GetComponent<Image>().sprite = _buttonSelected;
        _avatar.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + UserDataController.GetPlayerAvatar());
    }

    public void HelpSupport()
    {
        Application.OpenURL("https://www.nutaku.net/support/");
    }
    public void Privacy()
    {
        Application.OpenURL("https://www.nutaku.net/age/privacy-policy/");
    }
    public void TermsOfUse()
    {
        Application.OpenURL("https://www.nutaku.net/age/terms/");
    }
}
