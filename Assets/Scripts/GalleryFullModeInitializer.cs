using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryFullModeInitializer : MonoBehaviour
{
    [SerializeField] GameObject _galleryMediumInstance, _gallerySingleSkin, _animationCardPrefab;
    [SerializeField] Transform _grid, _skinsGrid, _animationsGrid;
    [SerializeField] GalleryManager _galleryManager;
    [SerializeField] Button _basicButton, _skinButton, _animationButton;

    //CREAMOS LA TARJETA DE ANIMACION CON INDICE FIJO, MÁS ADELANTE HAREMOS QUE SE GUARDEN DATOS EN EL JUGADOR

    public void Init()
    {
        for(int i = 0; i< UserDataController.GetSkinNumber(); i++)
        {
            GameObject imageInstance = Instantiate(_galleryMediumInstance, _grid);
            imageInstance.GetComponent<GallerySinglePhotoInstance>().Init(i);
        }
    }

    public void InitSkins()
    {
        for (int i = 0; i < SpecialSkinsManager._specialSkins.Length; i++)
        {
            GameObject skinInstance = Instantiate(_gallerySingleSkin, _skinsGrid);
            skinInstance.GetComponent<GallerySingleSkinPhotoInstance>().InitWithoutButton(i);
        }
    }
    public void InitAnimations()
    {
        bool unlocked = false;
        bool free = false;

        for (int i = 0; i < AnimationCardManager._animationCards.Length; i++)
        {
            unlocked = false;
            free = false;
            GameObject animationInstance = Instantiate(_animationCardPrefab, _animationsGrid);
            if(UserDataController.GetCharacterLevel(AnimationCardManager._animationCards[i]._character) > 0)
            {
                unlocked = true;
            }
            if(AnimationCardManager._animationCards[i]._rarity == 0)
            {
                free = true;
                unlocked = true;
            }
            animationInstance.GetComponent<GalleryAnimationCard>().Init(i,unlocked, AnimationCardManager._animationCards[i]._name, free);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < _grid.childCount; i++)
        {
            Destroy(_grid.GetChild(i).gameObject);
        }
        Init();

    }
    public void RefreshSkins()
    {
        for (int i = 0; i < _skinsGrid.childCount; i++)
        {
            Destroy(_skinsGrid.GetChild(i).gameObject);
        }
        InitSkins();
    }
    public void RefreshAnimations() 
    {
        for (int i = 0; i < _animationsGrid.childCount; i++)
        {
            Destroy(_animationsGrid.GetChild(i).gameObject);
        }
        InitAnimations();
    }

    public void BasicMode()
    {
        Refresh();
        _basicButton.interactable = false;
        _skinButton.interactable = true;
        _animationButton.interactable = true;
        _grid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _grid.gameObject.SetActive(true);
        _animationsGrid.gameObject.SetActive(false);
        _skinsGrid.gameObject.SetActive(false);
        _galleryManager.SwapGrid(true);
    }
    public void SkinMode()
    {
        RefreshSkins();
        _basicButton.interactable = true;
        _skinButton.interactable = false;
        _animationButton.interactable = true;
        _skinsGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _grid.gameObject.SetActive(false);
        _skinsGrid.gameObject.SetActive(true);
        _animationsGrid.gameObject.SetActive(false);
        _galleryManager.SwapGrid(false);
    }
    public void AnimationMode()
    {
        RefreshAnimations();
        _basicButton.interactable = true;
        _skinButton.interactable = true;
        _animationButton.interactable = false;
        _skinsGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _grid.gameObject.SetActive(false);
        _skinsGrid.gameObject.SetActive(false);
        _animationsGrid.gameObject.SetActive(true);
        _galleryManager.SwapGrid(false);
    }
}
