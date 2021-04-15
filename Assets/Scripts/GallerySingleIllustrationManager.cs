using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GallerySingleIllustrationManager : MonoBehaviour
{
    bool _onePhotoActive;
    Vector2 _positionFirstPinch;
    Vector2 _positionSecondPinch;
    float _pinchDistance;
    bool _pinching;
    int _currentOnePhotoIndex;
    Vector2 _originalSizeDelta;
    Camera _mainCamera;
    [SerializeField] Image _onePhotoMainImage;
    [SerializeField] TextMeshProUGUI _nameTx;
    [SerializeField] GallerySinglePhotoInstance _singleIllustration;
    [SerializeField] DayCareManager _dayCareManager;
    [SerializeField] GameObject[] _zoomDisableElements;
    [SerializeField] GameObject _leftButton, _rightButton;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void LoadConfig(int charIndex)
    {
        
        _currentOnePhotoIndex = charIndex;
        _nameTx.text = GetPhotoName(_currentOnePhotoIndex);
        _singleIllustration.Init(charIndex);
        if (UserDataController.GetGalleryImagesToOpen()[_currentOnePhotoIndex])
        {
            UserDataController.SetGalleryImage(_currentOnePhotoIndex, false);
        }
    }
    public void LoadSkinConfig(int skinIndex)
    {
        _currentOnePhotoIndex = skinIndex;
        //_nameTx.text = GetPhotoName(_currentOnePhotoIndex);
        _nameTx.text = SpecialSkinsManager._specialSkins[skinIndex]._name;
        _singleIllustration.InitSkin(skinIndex);
        //if (UserDataController.GetGalleryImagesToOpen()[_currentOnePhotoIndex])
        //{
        //    UserDataController.SetGalleryImage(_currentOnePhotoIndex, false);
        //}
    }

    private void Update()
    {
        if (_onePhotoActive)
        {
            if (Input.touchCount == 1) 
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Vector2 auxPos = Input.GetTouch(0).position;

                    _positionFirstPinch = _mainCamera.ScreenToViewportPoint(auxPos);
                }
            }
            if (Input.touchCount == 2) 
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    Vector2 auxPos = Input.GetTouch(1).position;
                    _positionSecondPinch = _mainCamera.ScreenToViewportPoint(auxPos);
                    _pinchDistance = (_positionFirstPinch - _positionSecondPinch).magnitude;
                    _originalSizeDelta = _onePhotoMainImage.rectTransform.sizeDelta;
                    //obtengo la posición del punto central
                    Vector2 pointPosition = Input.GetTouch(0).position + ((Input.GetTouch(1).position - Input.GetTouch(0).position) / 2f);
                    //obtengo las dimensiones de la imagen
                    Vector2 imgDimensions = _onePhotoMainImage.rectTransform.sizeDelta;
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_onePhotoMainImage.rectTransform, pointPosition, null, out localPoint);
                    localPoint = new Vector2(localPoint.x / imgDimensions.x, localPoint.y / imgDimensions.y);

                    RectTransform _mainImageRectTransform = _onePhotoMainImage.rectTransform;
                    Vector2 prePosition = _mainImageRectTransform.position;
                    _mainImageRectTransform.pivot = localPoint;
                    _mainImageRectTransform.position = prePosition;

                    Vector2 finalLocalPoint = new Vector2(0.5f, 0.5f) + localPoint;
                    _mainImageRectTransform.pivot = finalLocalPoint;
                    _mainImageRectTransform.anchoredPosition = imgDimensions * localPoint;
                    _pinching = true;
                }
            }

            if (_pinching)
            {
                if (Input.touchCount < 2)
                {
                    _pinching = false;
                    foreach(GameObject g in _zoomDisableElements)
                    {
                        g.SetActive(true);
                    }
                    if (!UserDataController.IsSpecialCardUnlocked(_currentOnePhotoIndex) || !UserDataController.IsSkinUnlocked(_currentOnePhotoIndex))
                    {
                        _zoomDisableElements[2].SetActive(false);
                    }
                    //Cambios para que no aparezca la estrella tras hacer zoom

                    if (_currentOnePhotoIndex % 4 == 0)
                    {
                        _leftButton.SetActive(false);
                    }
                    if (_currentOnePhotoIndex % 4 == 3)
                    {
                        _rightButton.SetActive(false);
                    }
                    _onePhotoMainImage.rectTransform.sizeDelta = _originalSizeDelta;
                    _onePhotoMainImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    _onePhotoMainImage.rectTransform.anchoredPosition = Vector3.zero;
                    _singleIllustration.SetZoomButtonState(true);
                }
                else
                {
                    foreach (GameObject g in _zoomDisableElements)
                    {
                        g.SetActive(false);
                    }
                    float newPinchingDistance = (_mainCamera.ScreenToViewportPoint(Input.GetTouch(0).position) - _mainCamera.ScreenToViewportPoint(Input.GetTouch(1).position)).magnitude;
                    if (newPinchingDistance > _pinchDistance)
                    {
                        float zoom = (1 + 3 * (newPinchingDistance - _pinchDistance));
                        _onePhotoMainImage.rectTransform.sizeDelta = _originalSizeDelta * zoom;
                    }
                }
            }
        }
    }


    public void SetOnePhotoState(bool state)
    {
        _onePhotoActive = state;
    }
    public string GetPhotoName(int index)
    {
        string photoName = _dayCareManager.GetNamesList()[(index / 4)];
        switch (index % 4)
        {
            case 1:
                photoName += " (Underwear)";
                break;
            case 2:
                photoName += " (Having Fun)";
                break;
            case 3:
                photoName += " (Explicit)";
                break;
        }
        return photoName;
    }
}
