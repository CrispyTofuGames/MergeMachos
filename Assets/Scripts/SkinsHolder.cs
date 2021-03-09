using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsHolder : MonoBehaviour
{
    [SerializeField] GallerySingleSkinPhotoInstance _skin0, _skin1;

    public void Init(int skin)
    {
        _skin1.gameObject.SetActive(false);
        _skin0.Init(skin);
    }
    public void Init(int skin0, int skin1)
    {
        _skin1.gameObject.SetActive(true);
        _skin0.Init(skin0);
        _skin1.Init(skin1);
    }
}
