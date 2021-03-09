using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryFullModeInitializer : MonoBehaviour
{
    [SerializeField] GameObject _galleryMediumInstance, _gallerySingleSkin;
    [SerializeField] Transform _grid, _skinsGrid;
    [SerializeField] GalleryManager _galleryManager;
    [SerializeField] Button _basicButton, _skinButton;

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

    public void BasicMode()
    {
        Refresh();
        _basicButton.interactable = false;
        _skinButton.interactable = true;
        _grid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _grid.gameObject.SetActive(true);
        _skinsGrid.gameObject.SetActive(false);
        _galleryManager.SwapGrid(true);
    }
    public void SkinMode()
    {
        RefreshSkins();
        _basicButton.interactable = true;
        _skinButton.interactable = false;
        _skinsGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _grid.gameObject.SetActive(false);
        _skinsGrid.gameObject.SetActive(true);
        _galleryManager.SwapGrid(false);
    }
}
