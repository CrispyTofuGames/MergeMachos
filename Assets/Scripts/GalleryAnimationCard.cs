using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GalleryAnimationCard : MonoBehaviour
{
    GalleryManager galleryManager;
    [SerializeField] Image character, charIcon;
    [SerializeField] GameObject lockNull, freeNull;
    int characterIndex;

    private void Start()
    {
        galleryManager = FindObjectOfType<GalleryManager>();
    }

    public void Init(int charIndex, bool unlocked, string charName, bool free)
    {
        character.sprite = Resources.Load<Sprite>("Sprites/AnimatedCards/" + charIndex);
        characterIndex = charIndex;
        if (unlocked)
        {
            character.color = Color.white;
            lockNull.SetActive(false);
        }
        else
        {
            character.color = Color.black;
            charIcon.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (charIndex + 1)*2);
            charIcon.overrideSprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (charIndex + 1) * 2);
            lockNull.SetActive(true);
        }
        if (free)
        {
            freeNull.SetActive(true);
        }
        else
        {
            freeNull.SetActive(false);
        }
    }

    public void LoadButton()
    {
        galleryManager.LoadAnimatedScene(characterIndex);
    }
}
