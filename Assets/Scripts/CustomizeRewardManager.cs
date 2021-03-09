using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeRewardManager : MonoBehaviour
{
    [SerializeField] RectTransform _selectedPanelTransform;
    int _selectedFreeReward;
    [SerializeField] GameObject _freeRewardPanel;
    RectTransform _selectedPanelChild;
    PanelManager _panelManager;
    [SerializeField] Sprite[] _grounds;
    [SerializeField] Sprite[] _seats;
    [SerializeField] Sprite[] _tables;
    [SerializeField] Image _selectedGround;
    [SerializeField] Image[] _selectedSeats;
    [SerializeField] Image[] _selectedTables;
    RewardManager _rewardManager;
    public void OpenCustomizeRewardPanel()
    {
        _rewardManager = FindObjectOfType<RewardManager>();
        _panelManager = FindObjectOfType<PanelManager>();
        _selectedPanelChild = _selectedPanelTransform.GetChild(0).GetComponent<RectTransform>();
        _selectedPanelTransform.gameObject.SetActive(false);
        _selectedPanelChild.localScale = Vector3.zero;
        _selectedPanelChild.localScale = Vector3.zero;
        _panelManager.RequestShowPanel(_freeRewardPanel);
    }
    public void SelectReward(int selection)
    {
        _selectedFreeReward = selection;
        StartCoroutine(ShowSelection());
    }

    public void ConfirmFreeReward()
    {
        _panelManager.ClosePanel();

        UserDataController.UnlockCellSkin(5 + _selectedFreeReward);
        UserDataController.UnlockExpositorSkin(5 + _selectedFreeReward);
        UserDataController.UnlockGroundSkin(5 + _selectedFreeReward);

        _rewardManager.EarnCustomizeItem(5 +_selectedFreeReward,0);
        _rewardManager.EarnCustomizeItem(5 +_selectedFreeReward, 1);
        _rewardManager.EarnCustomizeItem(5 + _selectedFreeReward, 2);

        UserDataController.ObtainFreeCustomizeReward();
    }

    public void CloseSelection()
    {
        StartCoroutine(HideSelection());
    }
    IEnumerator ShowSelection()
    {
        _selectedGround.sprite = _grounds[_selectedFreeReward];
        _selectedGround.overrideSprite= _grounds[_selectedFreeReward];

        for(int i =0; i< _selectedTables.Length; i++)
        {
            _selectedTables[i].sprite = _tables[_selectedFreeReward];
            _selectedTables[i].overrideSprite = _tables[_selectedFreeReward];
        }

        for (int i = 0; i < _selectedSeats.Length; i++)
        {
            _selectedSeats[i].sprite = _seats[_selectedFreeReward];
            _selectedSeats[i].overrideSprite = _seats[_selectedFreeReward];
        }

        _selectedPanelTransform.gameObject.SetActive(true);
        for (float i = 0; i< 0.1f; i += Time.deltaTime)
        {
            yield return null;
            _selectedPanelChild.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i/0.1f);
        }
        _selectedPanelChild.localScale = Vector3.one;
    }

    IEnumerator HideSelection()
    {
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            yield return null;
            _selectedPanelChild.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, i / 0.1f);
        }
        _selectedPanelChild.localScale = Vector3.zero;
        _selectedPanelTransform.gameObject.SetActive(false);
    }

    void Start()
    {


    }

    void Update()
    {
        
    }
}

