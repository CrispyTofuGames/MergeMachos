using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GalleryFace : MonoBehaviour
{
    [SerializeField] Image _face;

    [SerializeField] Image _starLevel1;

    [SerializeField] Image _starLevel2;

    [SerializeField] Image _starLevel3;

    [SerializeField] Sprite _comingSoon;

    [SerializeField] GameObject _lockImage;

    [SerializeField] Sprite _lockedStar;

    [SerializeField] Sprite _unlockedStar;

    [SerializeField] TextMeshProUGUI _unlockProgress;

    [SerializeField] GameObject _progressBar;

    [SerializeField] Button _openCharButton;
    [SerializeField] Image _frame;
    [SerializeField] Sprite[] _framesByLevel;
    GalleryManager _galleryManager;
    [SerializeField] TextMeshProUGUI _fragmentsProgress;
    [SerializeField] GameObject _levelUpButton;
    int _currentCharacter;
    [SerializeField] GameObject _moreFragmentsButton;
    bool _faceGallery;
    bool _canOpenData;
    public void BuyFragments()
    {
        FindObjectOfType<BuyFragmentsController>().ShowInfo(FragmentType.CharacterFragments, _currentCharacter, UserDataController.GetRemainingCharacterFragments(_currentCharacter));
    }
    private void Awake()
    {
        GameEvents.BuyCharacterFragments.AddListener(SetFragmentProgress);
    }
   
    public void Init(int character, bool facesGallery)
    {
        _faceGallery = facesGallery;
        _currentCharacter = character;
        character = character * 2;
        int toCharBiggestDino = UserDataController.GetBiggestDino();
        _moreFragmentsButton.GetComponent<Button>().onClick.AddListener(() => BuyFragments());
        Sprite sp = Resources.Load<Sprite>("Sprites/FaceSprites/" + character);
        _moreFragmentsButton.SetActive(false);
        if (sp != null)
        {
            _face.sprite = sp;
            if (facesGallery) //Para que no nos cambie a la del characterManager
            {
                _face.rectTransform.sizeDelta = new Vector2(240f, 240f);

                if (character <= toCharBiggestDino)
                {
                    _face.color = Color.white;
                    _lockImage.SetActive(false);
                    //SET TRUE WHEN 2ND UPDATE 
                    _progressBar.SetActive(false);
                    _openCharButton.interactable = true;
                }
                else
                {
                    _face.color = Color.black;
                    _lockImage.SetActive(true);
                    _progressBar.SetActive(false);
                    _openCharButton.interactable = false;
                }
            }
            else
            {
                SetFragmentProgress();
            }
            SetLevel(UserDataController.GetCharacterLevel(character / 2));

        }
        else
        {
            _lockImage.SetActive(false);
            _face.sprite = _comingSoon;
            _openCharButton.interactable = false;
            SetLevel(0);
        }
    }

    public void LevelUp()
    {
        if (UserDataController.CanLevelUp(_currentCharacter))
        {
            FindObjectOfType<RewardManager>().LevelUpCharacter(_currentCharacter, UserDataController.GetCharacterLevel(_currentCharacter) + 1);
        }
        Init(_currentCharacter, false);
    }

    public void OpenCharacterData()
    {
        _galleryManager.OpenCharacterData(_currentCharacter);
    }
    public void SetFragmentProgress()
    {
        if (!_faceGallery)
        {
            _moreFragmentsButton.SetActive(false);
            if (UserDataController.GetCharacterLevel(_currentCharacter) > 2)
            {
                _levelUpButton.SetActive(false);
                _progressBar.SetActive(false);
                _moreFragmentsButton.SetActive(false);
                _face.rectTransform.sizeDelta = new Vector2(240f, 240f);
            }
            else
            {

                _face.rectTransform.sizeDelta = new Vector2(220f, 200f);
                _progressBar.SetActive(true);
                int fragmentsCap = GameData.characterFragmentCapsByLevel[UserDataController.GetCharacterLevel(_currentCharacter)];
                _fragmentsProgress.text = UserDataController.GetCharacterFragments(_currentCharacter) + "/" + fragmentsCap;
                if (UserDataController.GetCharacterFragments(_currentCharacter) >= fragmentsCap)
                {
                    _moreFragmentsButton.SetActive(false);
                    _levelUpButton.SetActive(true);
                }
                else
                {
                    _moreFragmentsButton.SetActive(true);
                    _levelUpButton.SetActive(false);
                }
            }
        }
    }
    public void SetLevel(int level)
    {
        SetStars(level);
        SetFrame(level);
    }
    public void SetFrame(int level)
    {
        _frame.sprite = _framesByLevel[level];
    }
    public void SetStars(int stars)
    {
        _starLevel1.sprite = _lockedStar;
        _starLevel1.overrideSprite = _lockedStar;
        _starLevel2.sprite = _lockedStar;
        _starLevel2.overrideSprite = _lockedStar;
        _starLevel3.sprite = _lockedStar;
        _starLevel3.overrideSprite = _lockedStar;

        if (stars > 0)
        {
            _starLevel1.sprite = _unlockedStar;
            _starLevel1.overrideSprite = _unlockedStar;

            if(stars > 1)
            {
                _starLevel2.sprite = _unlockedStar;
                _starLevel2.overrideSprite = _unlockedStar;

                if(stars > 2)
                {
                    _starLevel3.sprite = _unlockedStar;
                    _starLevel3.overrideSprite = _unlockedStar;
                }
            }
        }
    }

    void Start()
    {
        _galleryManager = FindObjectOfType<GalleryManager>();
    }

}
