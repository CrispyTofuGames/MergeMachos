using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellInstance : MonoBehaviour
{
    int _cellNumber;
    DinosaurInstance _placedDino;
    MainGameSceneController _mainSceneController;
    ExpositorInstance _targetExpositor;
    bool _clicking;
    int _nClicks;
    Coroutine _clickCr;
    int _boxNumber;
    GameObject _currentBox;
    BoxManager.BoxType _currentBoxType;
    [SerializeField]
    SpriteRenderer _cellSprite;

    private void Awake()
    {
        _boxNumber = -1;
    }
    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
        RefreshCellSprite();
    }
    public void SetDinosaur(DinosaurInstance dinosaur)
    {
        _placedDino = dinosaur;
    }
    public void ExposeDinosaur(ExpositorInstance targetExpositor)
    {
        _targetExpositor = targetExpositor;
        _targetExpositor.ShowDinosaur(this);
    }
    public void StopExpose()
    {
        _targetExpositor.HideDinosaur();
        _targetExpositor = null;
        _placedDino.StopWorking();
    }
    public void SetCell(int cellNumber)
    {
        _cellNumber = cellNumber;
    }

    public void RefreshCellSprite()
    {
        _cellSprite.sprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
    }

    public void SetExpositor(ExpositorInstance expositor)
    {
        _targetExpositor = expositor;
    }

    public void SetBox(BoxManager.BoxType boxType, int boxNumber, GameObject box)
    {
        _currentBoxType = boxType;
        _boxNumber = boxNumber;
        _currentBox = box;
    }
    public DinosaurInstance GetDinoInstance()
    {
        return _placedDino;
    }
    public int GetCellNumber()
    {
        return _cellNumber;
    }
    public int GetBoxNumber()
    {
        return _boxNumber;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (_placedDino != null)
            {
                if (CurrentSceneManager._canPickDinosaur)
                {
                    if (!_placedDino.IsWorking())
                    {
                        _mainSceneController.PickDinosaur(_placedDino);
                    }
                }
            }
            if (_clickCr != null)
            {
                StopCoroutine(_clickCr);
            }
            _clicking = true;
            _clickCr = StartCoroutine(DisableClickingState());
        }
    }
    private void OnMouseEnter()
    {
        _mainSceneController.EnterCell(this);
    }
    private void OnMouseExit()
    {
        _mainSceneController.ExitCell();
        _clicking = false;
        _nClicks = 0;
    }
    IEnumerator DisableClickingState()
    {
        yield return new WaitForSeconds(0.25f);
        _clicking = false;
        _nClicks = 0;
    }
    private void Update()
    {

    }
    private void OnMouseUp()
    {
        if (_clicking)
        {
            if(_placedDino != null)
            {
                if (_placedDino.IsWorking())
                {
                    if (CurrentSceneManager._canTakeBackByCell)
                    {
                        _mainSceneController.StopShowDino(_cellNumber);
                    }
                }
                else
                {
                    _nClicks++;
                    if (_nClicks == 2)
                    {
                        _nClicks = 0;
                        if (CurrentSceneManager._canShowDinosaurByTouch)
                        {
                            _mainSceneController.ShowDinosaurInFirstExpo(_cellNumber);
                        }
                    }
                }
            }
            else
            {
                if (_currentBox != null)
                {
                    if (_currentBoxType == BoxManager.BoxType.StandardBox || _currentBoxType == BoxManager.BoxType.RewardedBox)
                    {
                        if (GetBoxNumber() >= 0)
                        {
                            OpenBox();
                        }
                    }
                    else
                    {
                        if (_currentBoxType == BoxManager.BoxType.LootBox)
                        {
                            OpenLootBox();
                        }
                    }
                }
            }
        }
        _clicking = false;
    }

    public ExpositorInstance GetTargetExpositor()
    {
        return _targetExpositor;
    }

    public GameObject HaveBox()
    {
        return _currentBox;
    }
    public void OpenBox()
    {
        if (CurrentSceneManager._canOpenBox)
        {
            Destroy(_currentBox);
            _currentBox = null;
            _mainSceneController.CreateDinosaur(_cellNumber, _boxNumber);
            _boxNumber = -1;
            GameEvents.OpenBox.Invoke();
        }
    }
    public void OpenLootBox()
    {
        Destroy(_currentBox);
        _currentBox = null;
        UserDataController.CreateDinosaur(_cellNumber, -1);
        _boxNumber = -1;
        FindObjectOfType<NestManager>().OpenNest();
    }
    
    public void DestroyBox()
    {
        Destroy(_currentBox);
        _currentBox = null;
        UserDataController.CreateDinosaur(_cellNumber, -1);
    }
}
