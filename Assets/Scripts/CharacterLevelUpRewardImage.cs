using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelUpRewardImage : MonoBehaviour
{
    [SerializeField] Sprite[] _frames;
    [SerializeField] Image _frameImage;
    [SerializeField] Image[] _stars;
    [SerializeField] Sprite _starOff;
    [SerializeField] Sprite _starOn;
    [SerializeField] Image _face;

    public void Init(int character, int level)
    {
        _face.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + character*2);
        _face.overrideSprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + character*2);
        _frameImage.sprite = _frames[level];
        _frameImage.overrideSprite= _frames[level];
        for(int i = 0; i< 3; i++)
        {
            if(i< level)
            {
                _stars[i].sprite = _starOn;
            }
            else
            {
                _stars[i].sprite = _starOff;
            }
        }
    }
}
