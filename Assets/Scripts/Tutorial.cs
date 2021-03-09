using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Tutorial : MonoBehaviour
{
    float zoomDuration = 0.55f;
    [SerializeField]
    RectTransform _circlePanelTr;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    RectTransform _fastPurchaseButtonTr;
    [SerializeField]
    GameObject _circlePanelObject;
    Camera _camera;
    [SerializeField]
    HandController _handController;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    TutorController _tutorController;
    [SerializeField]
    AdviceController _adviceController;    
    [SerializeField]
    CellManager _cellManager;
    [SerializeField]
    SpeedUpManager _speedUpManager;
    [SerializeField]
    BoxManager _boxManager;
    [SerializeField]
    PanelManager _panelManager;
    [SerializeField]
    GameObject _skinRewardPanel;
    [SerializeField]
    UpgradesManager _upgradesManager;
    bool waitingPurchaseTutorial0 = false;
    bool waitingPurchaseTutorial1 = false;
    bool waitingMergeTutorial2 = false;
    bool waitingWorkDinosaurTutorial3 = false;
    bool waitingOpenBoxTutorial4 = false;
    bool waitingTakeBackTutorial5 = false;
    bool waitingMergeTutorial6 = false;
    bool waitingWorkTutorial7 = false;
    bool waitingSpeak = false;
    Coroutine _adviceCr;
    Coroutine _showTextCr;
    Coroutine _moveTextCr;
    Coroutine _hideTextCr;
    Coroutine _firstGalleryTutorial;
    private void Awake()
    {
        GameEvents.Purchase.AddListener(FastPurchase);
        GameEvents.ShowAdvice.AddListener(ShowAdvice);
        GameEvents.CloseDinoUpPanel.AddListener(Merge);
        GameEvents.WorkDino.AddListener(Work);
        GameEvents.OpenBox.AddListener(OpenBox);
        GameEvents.TakeBack.AddListener(TakeDinoBack);
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
    }
    private void Start()
    {
        _camera = Camera.main;
        if (!UserDataController._currentUserData._tutorialCompleted[0])
        {
            StartTutorial(0);
        }
        else
        {
            if (!UserDataController._currentUserData._tutorialCompleted[1])
            {
                StartTutorial(1);
            }
            else
            {
                if (!UserDataController._currentUserData._tutorialCompleted[2])
                {
                    StartTutorial(2);
                }
                else
                {
                    if (!UserDataController._currentUserData._tutorialCompleted[3])
                    {
                        StartTutorial(3);
                    }
                    else
                    {
                        if (!UserDataController._currentUserData._tutorialCompleted[4])
                        {
                            StartTutorial(4);
                        }
                        else
                        {
                            if (!UserDataController._currentUserData._tutorialCompleted[5])
                            {
                                StartTutorial(5);
                            }
                            else
                            {
                                if (!UserDataController._currentUserData._tutorialCompleted[6])
                                {
                                    StartTutorial(6);
                                }
                                else
                                {
                                    if (!UserDataController._currentUserData._tutorialCompleted[7])
                                    {
                                        StartTutorial(7);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void StartTutorial(int tutorialIndex)
    {
        switch (tutorialIndex)
        {
            case 0:
                StartCoroutine(Tutorial0());
                break;
            case 1:
                StartCoroutine(Tutorial1());
                break;
            case 2:
                StartCoroutine(Tutorial2());
                break;
            case 3:
                StartCoroutine(Tutorial3());
                break;
            case 4:
                StartCoroutine(Tutorial4());
                break;
            case 5:
                StartCoroutine(Tutorial5());
                break;
            case 6:
                StartCoroutine(Tutorial6());
                break;
            case 7:
                StartCoroutine(Tutorial7());
                break;
            case 8:
                StartCoroutine(Tutorial8());
                break;
        }
    }

    public void EndSpeak()
    {
        waitingSpeak = false;
    }
    IEnumerator ZoomIn(float targetScale)
    {
        for(float i = 0; i< zoomDuration; i+= Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one * 25, Vector3.one * targetScale, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one * targetScale;
    }

    IEnumerator ZoomOut(float originalScale)
    {
        for (float i = 0; i < zoomDuration; i += Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one * originalScale, Vector3.one * 25, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one * 25f;
        _circlePanelObject.SetActive(false);
    }

    IEnumerator Tutorial0()
    {
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(0);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }

        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingPurchaseTutorial0 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanPurchase();
    }

    IEnumerator Tutorial1()
    {
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingPurchaseTutorial1 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanPurchase();
    }
    IEnumerator Tutorial2()
    {
        Vector3 dino1PositionUI = _mainGameSceneController.GetDinoPositionsUIByCell(0);
        Vector3 dino2PositionUI = _mainGameSceneController.GetDinoPositionsUIByCell(1);
        Vector3 middlePosition = dino1PositionUI + ((dino2PositionUI - dino1PositionUI) / 2f);
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = middlePosition;
        yield return StartCoroutine(ZoomIn(2f));
        _handController.gameObject.SetActive(true);
        waitingMergeTutorial2 = true;
        _handController.StartDragMode(dino1PositionUI, dino2PositionUI);
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanMerge();
    }
    IEnumerator Tutorial3()
    {
        Vector3 firstDinoPositionUI = _camera.WorldToScreenPoint(_mainGameSceneController.GetFirstDinoPosition());
        Vector3 targetExpositorPosition = Camera.main.WorldToScreenPoint(_cellManager.GetExpoInstanceByIndex(0).transform.position);
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(1);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = (firstDinoPositionUI + targetExpositorPosition)/2;
        _handController.GetComponent<RectTransform>().position = _circlePanelTr.position;
        yield return StartCoroutine(ZoomIn(1.75f));
        _handController.gameObject.SetActive(true);
        waitingWorkDinosaurTutorial3 = true;
        _handController.StartDragMode(firstDinoPositionUI, targetExpositorPosition);
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanShowByDrag();
    }
    IEnumerator Tutorial4()
    {
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        Vector3 emptyCellPosition = Camera.main.WorldToScreenPoint(_cellManager.GetCellPosition(_mainGameSceneController.GetFirstEmptyCell()));
        _circlePanelTr.position = emptyCellPosition;
        _boxManager.DropSpecificBox(1);
        _handController.GetComponent<RectTransform>().position = emptyCellPosition;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingOpenBoxTutorial4 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanOpenBox();
    }
    IEnumerator Tutorial5()
    {
        CurrentSceneManager.LockEverything();
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(2);
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        Vector3 workerCellPosition = Camera.main.WorldToScreenPoint(_cellManager.GetCellPosition(_mainGameSceneController.GetFirstWorkerCell()));
        _circlePanelTr.position = workerCellPosition;
        _handController.GetComponent<RectTransform>().position = workerCellPosition;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingTakeBackTutorial5 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanTakeBackByCell();
    }
    IEnumerator Tutorial6()
    {
        List<int> mergeableCells = _mainGameSceneController.GetMergeableDinosCellIndex();
        Vector2 dino1WorldPosition = _cellManager.GetCellPosition(mergeableCells[0]);
        Vector2 dino2WorldPosition = _cellManager.GetCellPosition(mergeableCells[1]);
        Vector2 dino1PositionUI = _camera.WorldToScreenPoint(dino1WorldPosition);
        Vector2 dino2PositionUI = _camera.WorldToScreenPoint(dino2WorldPosition);
        Vector2 dinosSeparation = dino1WorldPosition - dino2WorldPosition;
        Vector2 dinosUISeparation = dino1PositionUI - dino2PositionUI;
        Vector2 middleUIPosition = dino1PositionUI - (dinosUISeparation / 2f);
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = middleUIPosition;
        yield return StartCoroutine(ZoomIn(Mathf.Abs(1)));
        _handController.gameObject.SetActive(true);
        waitingMergeTutorial6 = true;
        _handController.StartDragMode(dino1PositionUI, dino2PositionUI);
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanMerge();
    }
    IEnumerator Tutorial7()
    {
        while (_panelManager.GetPanelState())
        {
            yield return null;
        }
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(3);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _camera.WorldToScreenPoint(_mainGameSceneController.GetFirstDinoPosition());
        _handController.GetComponent<RectTransform>().position = _camera.WorldToScreenPoint(_mainGameSceneController.GetFirstDinoPosition());
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingWorkTutorial7 = true;
        _handController.StartDoubleClickMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanShowByTouch();
    }    
    IEnumerator Tutorial8()
    {
        while (_panelManager.GetPanelState())
        {
            yield return null;
        }
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(5);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        _speedUpManager.SetSpeedHardCoinsCost(0);
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _speedUpManager.GetSpeedUpPosition();
        _handController.GetComponent<RectTransform>().position = _speedUpManager.GetSpeedUpPosition();
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        while (!_speedUpManager.IsPanelOpen())
        {
            yield return null;
        }
        transform.SetAsLastSibling();

        _handController.StopTouchCoroutines();
        _handController.gameObject.SetActive(false);
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _speedUpManager.GetHardCoinsButtonPosition();
        yield return StartCoroutine(ZoomIn(1f));
        _handController.GetComponent<RectTransform>().position = _speedUpManager.GetHardCoinsButtonPosition();
        _handController.gameObject.SetActive(true);
        _handController.StartTouchMode();
        while (_speedUpManager.GetHappyHourSpeed() == 1)
        {
            yield return null;
        }
        _handController.StopTouchCoroutines();
        _handController.gameObject.SetActive(false);
        _circlePanelObject.SetActive(false);
        UserDataController.SaveTutorial(8);
        CurrentSceneManager.UnlockEverything();
        _speedUpManager.CloseSpeedUpPanel();
        _speedUpManager.SetSpeedHardCoinsCost(3);
        transform.SetSiblingIndex(5);

    }
    #region EventsCallbacks
    public void FastPurchase(int n)
    {
        if (waitingPurchaseTutorial0)
        {
            waitingPurchaseTutorial0 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(0);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(1);
        }

        if (waitingPurchaseTutorial1)
        {
            waitingPurchaseTutorial1 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(1);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(2);
        }
    }
    public void Merge()
    {
        if (waitingMergeTutorial2)
        {
            waitingMergeTutorial2 = false;
            _handController.StopDragCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(2);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(3);
        }
        else
        {
            if (waitingMergeTutorial6)
            {
                waitingMergeTutorial6 = false;
                _handController.StopDragCoroutines();
                _handController.gameObject.SetActive(false);
                _circlePanelObject.SetActive(false);
                UserDataController.SaveTutorial(6);
                CurrentSceneManager.UnlockEverything();
                StartTutorial(7);
            }
            else
            {
                if (!UserDataController._currentUserData._tutorialCompleted[8])
                {
                    _handController.StopTouchCoroutines();
                    _handController.gameObject.SetActive(false);
                    _circlePanelObject.SetActive(false);
                    CurrentSceneManager.UnlockEverything();
                    StartTutorial(8);
                }
            }
        }
    }
    public void Work()
    {
        if (waitingWorkDinosaurTutorial3)
        {
            waitingWorkDinosaurTutorial3 = false;
            _handController.StopDoubleClick();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(3);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(4);
        }
        else
        {
            if (waitingWorkTutorial7)
            {
                waitingWorkTutorial7 = false;
                _handController.StopDoubleClick();
                _handController.gameObject.SetActive(false);
                _circlePanelObject.SetActive(false);
                StartCoroutine(Tutorial7End());
            }
        }
    }
    IEnumerator Tutorial7End()
    {
        _tutorController.Speak(4);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        UserDataController.SaveTutorial(7);
        _boxManager.SetDropTrue();
        CurrentSceneManager.UnlockEverything();
    }
    public void OpenBox()
    {
        if (waitingOpenBoxTutorial4)
        {
            waitingOpenBoxTutorial4 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(4);
            CurrentSceneManager.UnlockEverything();
            StartCoroutine(ZoomOut(1f));
            StartTutorial(5);
        }
    }
    public void TakeDinoBack()
    {
        if (waitingTakeBackTutorial5)
        {
            waitingTakeBackTutorial5 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(5);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(6);
        }
    }
    #endregion
    public void ShowAdvice(GameEvents.AdviceEventData advice)
    {
        if (_adviceCr != null)
        {
            StopCoroutine(_adviceCr);

            if (_showTextCr != null)
            {
                StopCoroutine(_showTextCr);
            }
            if (_hideTextCr != null)
            {
                StopCoroutine(_hideTextCr);
            }
            if (_moveTextCr != null)
            {
                StopCoroutine(_moveTextCr);
            }
        }
        _adviceCr = StartCoroutine(CrShowAdvice(advice));
    }
    public void PlayGalleryTutorial()
    {
        _firstGalleryTutorial= StartCoroutine(CrGalleryTutorial());
    }

    IEnumerator CrGalleryTutorial()
    {
        gameObject.transform.SetAsLastSibling();
        _tutorController.Speak(6);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = FindObjectOfType<GalleryManager>().GetTakoFaceButton().position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.GetComponent<RectTransform>().position = FindObjectOfType<GalleryManager>().GetTakoFaceButton().position;
        GameObject characterGallery = FindObjectOfType<GalleryManager>().GetCharacterGallery();
        _handController.gameObject.SetActive(true);
        _handController.StopTouchCoroutines();
        _handController.StartTouchMode();
        while (!characterGallery.activeSelf)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(false);
        _handController.StopTouchCoroutines();
        _handController.gameObject.SetActive(false);
        UserDataController.WatchGalleryTutorial();
        transform.SetSiblingIndex(5);
        StartCoroutine(CrSecondGalleryTutorial());

    }
    IEnumerator CrSecondGalleryTutorial()
    {
        gameObject.transform.SetAsLastSibling();
        _tutorController.Speak(7);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = FindObjectOfType<GalleryManager>().GetTakoSoftCoinsButton(0).position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.GetComponent<RectTransform>().position = FindObjectOfType<GalleryManager>().GetTakoSoftCoinsButton(0).position;
        _handController.gameObject.SetActive(true);
        _handController.StopTouchCoroutines();
        _handController.StartTouchMode();
        while (!UserDataController.IsSkinUnlocked(1))
        {
            yield return null;
        }
        _circlePanelObject.SetActive(false);
        _handController.StopTouchCoroutines();
        _handController.gameObject.SetActive(false);
        CurrentSceneManager.UnlockEverything();

        transform.SetSiblingIndex(5);
        UserDataController.WatchSecondGalleryTutorial();

    }
    IEnumerator CrShowAdvice(GameEvents.AdviceEventData adviceKey)
    {
        _adviceController.gameObject.SetActive(true);
        _moveTextCr = StartCoroutine(_adviceController.CrMoveText());

        yield return _showTextCr = StartCoroutine(_adviceController.CrShowText(adviceKey));
        yield return new WaitForSeconds(_adviceController.moveDuration - (_adviceController.fadeDuration *2f));
        yield return _hideTextCr = StartCoroutine(_adviceController.CrHideText());
        _adviceController.gameObject.SetActive(false);
    }
}
