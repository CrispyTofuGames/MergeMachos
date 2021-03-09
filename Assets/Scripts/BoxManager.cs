using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    GameObject _standardBox;
    [SerializeField]
    GameObject _lootBox;
    [SerializeField]
    GameObject _rewardBox;
    [SerializeField]
    DayCareManager _dayCareManager;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;

    float _standardWaitingTime;
    float _standardCurrentTime;
    float _lootCurrentTime;
    float _lootWaitingTime;
    bool canDrop = false;
    int _rewardBoxCount;
    bool _rewarding;
    float _lootBoxTime = 10f;
    float _standardBoxTime = 15f;
    public enum BoxType {StandardBox, LootBox, RewardedBox };

    public void RewardBox(int boxesToReward)
    {
        _rewardBoxCount += boxesToReward;
        if (!_rewarding)
        {
            _rewarding = true;
            StartCoroutine(CrRewardBox());   
        }
        
    }
    IEnumerator CrRewardBox()
    {
        int cellIndex = _mainGameSceneController.GetFirstEmptyCell();
        while (cellIndex < 0)
        {
            yield return null;
            cellIndex = _mainGameSceneController.GetFirstEmptyCell();
        }
        int standardDropDino = _dayCareManager.GetFastPurchaseIndex();
        standardDropDino = Mathf.Max(standardDropDino - 1, 0);
        CreateBox(BoxType.RewardedBox, cellIndex, standardDropDino+1);
        _rewardBoxCount--;
        if (_rewardBoxCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(CrRewardBox());
        }
        else
        {
            _rewarding = false;
        }
    }
    private void Start()
    {
        _dayCareManager = FindObjectOfType<DayCareManager>();
        _standardWaitingTime = Random.Range(15f, 30f);
        _lootWaitingTime = Random.Range(60f, 120f);
        if (UserDataController._currentUserData._tutorialCompleted[7])
        {
            canDrop = true;
        }
    }
    public void CreateBox(BoxType boxType, int cellIndex, int dinoType)
    {
        GameObject boxToInstantiate = null;
        float remainingTime = 0f;
        switch (boxType)
        {
            case BoxType.StandardBox:
                boxToInstantiate = _standardBox;
                remainingTime = _standardBoxTime;
                break;

            case BoxType.LootBox:
                boxToInstantiate = _lootBox;
                remainingTime = _lootBoxTime;
                break;

            case BoxType.RewardedBox:
                boxToInstantiate = _rewardBox;
                remainingTime = _standardBoxTime;
                break;
        }
        
        GameObject box = Instantiate(boxToInstantiate, _cellManager.GetCellPosition(cellIndex), Quaternion.identity);
        GameEvents.PlaySFX.Invoke("Pop");
        box.GetComponent<BoxInstance>().Init(_cellManager.GetCellInstanceByIndex(cellIndex), remainingTime);
        _cellManager.GetCellInstanceByIndex(cellIndex).SetBox(boxType, dinoType, box);
        UserDataController.CreateBox(boxType, cellIndex, dinoType);
    }
    public bool DropStandardBox()
    {
        int standardDropDino = _dayCareManager.GetFastPurchaseIndex();
        standardDropDino = Mathf.Max(standardDropDino - 1, 0);

        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropStandardBox = true; 
        if (firstEmptyCell >= 0)
        {
            if (Random.value<0.2f)
            {
                CreateBox(BoxType.RewardedBox, firstEmptyCell, standardDropDino+1);
            }
            else
            {
                CreateBox(BoxType.StandardBox, firstEmptyCell, standardDropDino);
            }
        }
        else
        {
            canDropStandardBox = false;
        }
        return canDropStandardBox;
        
    }
    public bool DropSpecificBox(int dinotype)
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropStandardBox = true;
        if (firstEmptyCell >= 0)
        {
            CreateBox(BoxType.StandardBox, firstEmptyCell, dinotype);
        }
        else
        {
            canDropStandardBox = false;
        }
        return canDropStandardBox;

    }
    public bool DropLootBox()
    {
        int firstEmptyCell = _mainGameSceneController.GetFirstEmptyCell();
        bool canDropLootBox = true;
        if (firstEmptyCell >= 0)
        {
            CreateBox(BoxType.LootBox, firstEmptyCell, 0);
        }
        else
        {
            canDropLootBox = false;
        }
        return canDropLootBox;
    }

    public void SetDropTrue()
    {
        canDrop = true;
    }
    void Update()
    {
        if (canDrop)
        {
            _lootCurrentTime += Time.deltaTime;
            _standardCurrentTime += Time.deltaTime;
            if(_lootCurrentTime >= _lootWaitingTime)
            {
                if (DropLootBox())
                {
                    _lootCurrentTime = 0f;
                    _lootWaitingTime = Random.Range(60f, 120f);
                }
            }
            if (_standardCurrentTime >= _standardWaitingTime)
            {
                if (DropStandardBox())
                {
                    _standardCurrentTime = 0f;
                    _standardWaitingTime = Random.Range(15f, 30f);
                }
            }
        }
    }
}
