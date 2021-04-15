using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class CardConfigurator : MonoBehaviour
{

    [SerializeField]
    Image _mainImage;
    [SerializeField]
    Image _frame;
    [SerializeField]
    Image _background, _star;
    [SerializeField]
    Material _amazingBackgroundMaterial;
    [SerializeField]
    Image _foreground;
    [SerializeField]
    Material _amazingMaterial;
    [SerializeField]
    GameObject[] _vfx;
    GallerySinglePhotoInstance _gallerySinglePhotoInstance;

    static int[] _effects0 = {0,1,2};
    static List<Color> _colors;

    [SerializeField] Sprite _loadingSprite;
    [SerializeField] GameObject _preLoading;
    int _charIndex;
    bool _shinyState;


    private void Start()
    {
        if(_colors != null)
        {
            _colors = new List<Color>();
            _colors.Add(new Color(0.65f, 0.21f, 0.144f));
            _colors.Add(new Color( 0.145f,0.2012f,0.6509f));
        }
    }
    public void Init(int character)
    {
        _gallerySinglePhotoInstance = GetComponent<GallerySinglePhotoInstance>();
        int biggestDino = UserDataController.GetBiggestDino() + 1;
        _charIndex = character;
        _foreground.enabled = false;

        if (_star != null)
        {
            if (UserDataController.IsSpecialCardUnlocked(_charIndex) && UserDataController.IsSkinUnlocked(_charIndex))
            {
                _star.gameObject.SetActive(true);
            }
            else
            {
                _star.gameObject.SetActive(false);
                ActiveShinyMode(false);
            }
        }


        //StartCoroutine(LoadSpriteFromStreamingAssets(_mainImage, "/BigDraws/" + character, UserDataController.IsSkinUnlocked(character)));
        _mainImage.sprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);
        _mainImage.overrideSprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);


        _frame.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _frame.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());

        _background.sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.overrideSprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.preserveAspect = false;

    }
    public void InitSkin(int skin)
    {
        if (UserDataController.IsExtraSkinUnlocked(skin))
        {
            _mainImage.color = Color.white;
        }
        else
        {
            _mainImage.color = Color.black;
        }
        _mainImage.sprite = Resources.Load<Sprite>("Sprites/Skins/" + skin);
        _mainImage.overrideSprite = Resources.Load<Sprite>("Sprites/Skins/" + skin);
        _frame.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _frame.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _background.sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (SpecialSkinsManager._specialSkins[skin]._character));
        _background.overrideSprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (SpecialSkinsManager._specialSkins[skin]._character));
        _foreground.enabled = false;

        if (_star != null)
        {
            _star.gameObject.SetActive(false);
            ActiveShinyMode(false);
        }

        _background.material = null;
        _mainImage.material = null;
    }

    IEnumerator LoadSpriteFromStreamingAssets(Image targetImage, string path, bool unlocked)
    {
        _preLoading.SetActive(true);
        string finalPath = Application.streamingAssetsPath + path + ".png";
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(finalPath);
        if (unlocked)
        {
            _mainImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _mainImage.color = new Color(0, 0, 0, 0);
        }
        

        yield return uwr.SendWebRequest();
        Texture2D tex2D = DownloadHandlerTexture.GetContent(uwr);
        Sprite drawSprite = null;
        drawSprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0, 0), 100f);
        targetImage.sprite = drawSprite;
        targetImage.overrideSprite = drawSprite;
        _preLoading.SetActive(false);

        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            if (unlocked)
            {
                _mainImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, i / 0.2f);
            }
            else
            {
                _mainImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, i / 0.2f);
            }
            
            yield return null;
        }
        if (unlocked)
        {
            _mainImage.color = Color.white;
        }
        else
        {
            _mainImage.color = Color.black;
        }
    }

    public void ActiveShinyMode()
    {
        _shinyState = !_shinyState;

        if(_shinyState)
        {
            _background.material = _amazingBackgroundMaterial;
            Material _mat = new Material(_amazingMaterial);
            _mat.SetTexture("Texture2D_FAA9D2CE", Resources.Load<Texture2D>("Sprites/BigDraws/" + _charIndex + "_mask"));
            _mat.SetColor("Color_3F02EA66", new Color(0.65f, 0.21f, 0.144f));
            _mainImage.material = _mat;
            _foreground.enabled = true;
            //Material _foreMat = new Material(_amazingForegroundMaterial);
            //_foreMat = new Material
        }
        else
        {
            _foreground.enabled = false;
            _background.material = null;
            _mainImage.material = null;
        }
    }
    public void ActiveShinyMode(bool state)
    {

        _shinyState = state;
        if (_shinyState)
        {
            _background.material = _amazingBackgroundMaterial;
            Material _mat = new Material(_amazingMaterial);
            _mat.SetTexture("Texture2D_FAA9D2CE", Resources.Load<Texture2D>("Sprites/BigDraws/" + _charIndex + "_mask"));
            _mat.SetColor("Color_3F02EA66", new Color(0.65f, 0.21f, 0.144f));
            _mainImage.material = _mat;
            _foreground.enabled = true;
            //Material _foreMat = new Material(_amazingForegroundMaterial);
            //_foreMat = new Material
        }
        else
        {
            _foreground.enabled = false;
            _background.material = null;
            _mainImage.material = null;
        }
    }
}
