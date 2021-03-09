using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpositorInstance : MonoBehaviour
{
    int _expositorNumber;
    MainGameSceneController _mainSceneController;
    CellInstance _referencedCell;
    [SerializeField]
    SpriteRenderer dinoImage;
    bool _clicking;
    EconomyManager _economyManager;
    GameObject dinoCopy;
    bool locked;
    [SerializeField]
    SpriteRenderer _expositorSprite;

    public void Lock()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        locked = true;
    }
    public void Unlock()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        locked = false;
    }
    public bool IsLocked()
    {
        return locked;
    }
    private void Awake()
    {
        _economyManager = FindObjectOfType<EconomyManager>();
        GameEvents.TouristWatchDino.AddListener(EarnMoney);
    }
    private void Start()
    {
        _mainSceneController = FindObjectOfType<MainGameSceneController>();
        RefreshExpositorSprite();
    }
    public void ShowDinosaur(CellInstance cellInstance)
    {
        _referencedCell = cellInstance;
        dinoCopy = Instantiate(_referencedCell.GetDinoInstance().gameObject, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        dinoCopy.transform.localScale = new Vector3(1,1,1);//SOLO FUNCIONA EN CHIBIS
        Destroy(dinoCopy.GetComponent<DinosaurInstance>());
    }
    public void SetReferencedCell(CellInstance targetCell)
    {
        _referencedCell = targetCell;
        if(targetCell != null)
        {
            Destroy(dinoCopy);
            dinoCopy = Instantiate(targetCell.GetDinoInstance().gameObject, transform.position, Quaternion.identity);
            dinoCopy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }

    public void HideDinosaur()
    {
        _referencedCell = null;
        Destroy(dinoCopy);
    }

    public void RefreshExpositorSprite()
    {
        _expositorSprite.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
    }

    public void SetExpositor(int expoNumber)
    {
        _expositorNumber = expoNumber;
    }
    public DinosaurInstance GetDinoInstance()
    {
        if(_referencedCell != null)
        {
            return _referencedCell.GetDinoInstance();
        }
        else
        {
            return null;
        }
    }
    public int GetExpositorNumber()
    {
        return _expositorNumber;
    }
    private void OnMouseEnter()
    {
        _mainSceneController.EnterExpositor(this);
    }
    private void OnMouseExit()
    {
        _mainSceneController.ExitExpositor();
        _clicking = false;
    }

    private void OnMouseDown()
    {
        if (_referencedCell != null)
        {
            _clicking = true;
            StartCoroutine(DisableClickingState());
        }
    }
    private void OnMouseUp()
    {
        //if (_referencedCell != null && _clicking)
        //{
        //    if (CurrentSceneManager._canTakeBackByExpositor)
        //    {
        //        _mainSceneController.StopShowDino(_referencedCell.GetCellNumber());
        //    }
        //}
    }
    IEnumerator DisableClickingState()
    {
        yield return new WaitForSeconds(0.25f);
        _clicking = false;
    }
    public void EarnMoney(int expoIndex)
    {
        if(_referencedCell != null)
        {
            int dinoType = _referencedCell.GetDinoInstance().GetDinosaur();
            if (expoIndex == _expositorNumber)
            {
                if (dinoType >= 0)
                {
                    GameCurrency currentDinoEarnings = new GameCurrency(_economyManager.GetEarningsByType(dinoType).GetIntList());
                    currentDinoEarnings.MultiplyCurrency(2f);
                    _economyManager.EarnSoftCoins(currentDinoEarnings);
                    Vector3 padding = new Vector3(0, 0.2f, 0);
                    switch (expoIndex)
                    {
                        case 6:
                        case 0:
                        case 2:
                        case 8:
                            padding = new Vector3(0.15f *(float)(currentDinoEarnings.GetCurrentMoney().Length), 0.2f, 0);
                            break;
                    }
                    GameEvents.EarnMoney.Invoke(new GameEvents.MoneyEventData(transform.position + padding, currentDinoEarnings));
                    GameEvents.PlaySFX.Invoke("Coins_" + Random.Range(0, 4));
                }
            }
        }
    }
}
