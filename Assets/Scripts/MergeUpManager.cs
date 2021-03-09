using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MergeUpManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _dinoTypeTx;
    [SerializeField]
    GameObject _mergeUpPanel;
    bool canDisable;
    [SerializeField]
    GameObject _mainFillBar;
    [SerializeField]
    Image lastQualityBar;
    [SerializeField]
    Image currentQualityBar;
    List<Sprite> dinoMergeUpSprites;
    [SerializeField]
    AnimationCurve animationCurve;
    PanelManager _panelManager;
    [SerializeField]
    Image _leftMergedChibi;
    [SerializeField]
    Image _rightMergedChibi;
    [SerializeField]
    Image _finalMergeResultChibi;
    [SerializeField]
    GameObject _title;
    [SerializeField]
    GameObject _touchToClose;
    [SerializeField]
    GameObject _hotnessBar;
    [SerializeField]
    VFXManager _vfxManager;
    [SerializeField]
    AnimationCurve _growAnimationCurve;
    [SerializeField]
    DayCareManager _dayCareManager;
    bool _canClosePanel = false;
    RewardManager _rewardManager;
    int _currentDinoType;
    private void Awake()
    {
        _vfxManager = FindObjectOfType<VFXManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        dinoMergeUpSprites = new List<Sprite>();
        // --> for(int i = 0; i < UserDataController.GetDinoAmount(); i++)
        for (int i = 0; i < UserDataController.GetDinoAmount(); i++)
        {
            string path = "Sprites/Chibis/" + i;
            dinoMergeUpSprites.Add(Resources.Load<Sprite>(path));
        }
        _panelManager = FindObjectOfType<PanelManager>();
        _mergeUpPanel.SetActive(false);
        _mainFillBar.SetActive(false);
        GameEvents.DinoUp.AddListener(MergeUpCallBack);
    }

    private void Update()
    {
        if (_canClosePanel)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClosePanel();
            }
        }
    }

    public void MergeUpCallBack(int dinoType)
    {
        _currentDinoType = dinoType;
        StartCoroutine(ShowNewMergeInfo(dinoType));
    }

    IEnumerator ShowNewMergeInfo(int dinoType)
    {
        while (_rewardManager.GetRemainingRewards() > 0)
        {
            yield return null;
        }
        _leftMergedChibi.enabled = true;
        _leftMergedChibi.color = Color.white;
        _rightMergedChibi.enabled = true;
        _rightMergedChibi.color = Color.white;
        _finalMergeResultChibi.enabled = false;
        _leftMergedChibi.rectTransform.anchoredPosition = new Vector2(-200, 150);
        _rightMergedChibi.rectTransform.anchoredPosition = new Vector2(200, 150);
        _title.SetActive(false);
        _touchToClose.SetActive(false);
        _hotnessBar.SetActive(false);
        _leftMergedChibi.sprite = dinoMergeUpSprites[dinoType - 1];
        _leftMergedChibi.overrideSprite = dinoMergeUpSprites[dinoType - 1];
        _rightMergedChibi.sprite = dinoMergeUpSprites[dinoType - 1];
        _rightMergedChibi.overrideSprite = dinoMergeUpSprites[dinoType - 1];
        _finalMergeResultChibi.sprite = dinoMergeUpSprites[dinoType];
        _finalMergeResultChibi.overrideSprite = dinoMergeUpSprites[dinoType];
        _finalMergeResultChibi.enabled = false;
        canDisable = false;
        _dinoTypeTx.text = _dayCareManager.GetChibiName(dinoType);
        _panelManager.RequestShowPanel(_mergeUpPanel);

        while (!_mergeUpPanel.activeSelf)
        {
            yield return null;
        }
        GameEvents.PlaySFX.Invoke("PitchMerge");
        currentQualityBar.fillAmount = (float)(dinoType - 1f) / (float)UserDataController.GetDinoAmount();
        lastQualityBar.fillAmount = (float)dinoType / (float)UserDataController.GetDinoAmount();
        yield return new WaitForSeconds(0.5f);
        _vfxManager.PlayMergeAnimation();

        for (float i = 0; i<1f; i += Time.deltaTime)
        {
            _leftMergedChibi.color = Color.Lerp(Color.white, Color.black, i / 0.5f);
            _rightMergedChibi.color = Color.Lerp(Color.white, Color.black, i / 0.5f);
            _leftMergedChibi.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-200, 150), new Vector2(0, 150), i);
            _rightMergedChibi.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(200, 150), new Vector2(0, 150), i);
            yield return null;
        }
        _leftMergedChibi.rectTransform.anchoredPosition = new Vector2(0, 150);
        _rightMergedChibi.enabled = false;
        yield return new WaitForSeconds(2f);
        float fadeOutTime = 1f;
        for(float i = 0; i<fadeOutTime; i += Time.deltaTime)
        {
            _leftMergedChibi.color = Color.Lerp(Color.black, new Color(0,0,0, 0), i / fadeOutTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        _finalMergeResultChibi.enabled = true;
        _title.SetActive(true);
        _touchToClose.SetActive(true);
        _hotnessBar.SetActive(true);
        _mainFillBar.SetActive(true);
        for (float i = 0; i< 0.2f; i += Time.deltaTime)
        {
            _finalMergeResultChibi.rectTransform.localScale = _growAnimationCurve.Evaluate(i/0.2f)*Vector3.one;
            yield return null;
        }
        _finalMergeResultChibi.rectTransform.localScale = Vector3.one;
        
        _leftMergedChibi.enabled = false;
        canDisable = true;
        _title.SetActive(true);
        _touchToClose.SetActive(true);
        _hotnessBar.SetActive(true);
        _canClosePanel = true;
        _currentDinoType = dinoType;
    }

    public void ClosePanel()
    {
        _canClosePanel = false;
        _mainFillBar.SetActive(false);
        _panelManager.ClosePanel();
        GameEvents.CloseDinoUpPanel.Invoke();
        GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(_currentDinoType * 2, 0));
    }
}
