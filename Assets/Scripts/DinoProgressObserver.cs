using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DinoProgressObserver : MonoBehaviour
{
    Image _progressBar;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    TextMeshProUGUI txProgress;
    [SerializeField]
    TextMeshProUGUI currentLevel;
    [SerializeField]
    Image dinoImage;
    [SerializeField]
    Image nextDinoImage;
    // Start is called before the first frame update
    void Start()
    {
        _progressBar = GetComponent<Image>();
        UpdateFillAmount();
    }

    public void UpdateFillAmount()
    {
        float currentProgress = _mainGameSceneController.GetDinosSum();
        float finalAmount = currentProgress / 256f;

        //currentLevel.text = (UserDataController.GetBiggestDino() + 2).ToString();
        if(finalAmount < 0)
        {
            finalAmount = 1f;
        }
        if (UserDataController.GetBiggestDino() >= UserDataController.GetDinoAmount() -1)
        {
            _progressBar.fillAmount = 1f;
            txProgress.text = LocalizationController.GetValueByKey("COMING_SOON");
            dinoImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (UserDataController.GetBiggestDino()));
            nextDinoImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (UserDataController.GetBiggestDino()));
        }
        else
        {
            _progressBar.fillAmount = finalAmount;
            txProgress.text = Mathf.Min(Mathf.Floor(finalAmount * 100), 100f).ToString() + "%";
            dinoImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (UserDataController.GetBiggestDino()));
            nextDinoImage.sprite = Resources.Load<Sprite>("Sprites/FaceSprites/" + (UserDataController.GetBiggestDino() + 1));
        }
    }

    private void Update()
    {
        UpdateFillAmount();
    }
}
