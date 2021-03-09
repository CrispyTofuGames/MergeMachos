using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    Queue<GameObject> _panelsToOpen;
    GameObject _currentPanel;
    bool _isAnyPanelOpen = false;
    [SerializeField]
    Image _blackBackground;
    [SerializeField]
    GameObject _closeGamePanel;
    [SerializeField]
    GalleryManager _galleryManager;
    [SerializeField]
    CustomizeController _customizeController;

    public void RequestShowPanel(GameObject panel)
    {
        if (_panelsToOpen == null)
        {
            _panelsToOpen = new Queue<GameObject>();
        }
        _panelsToOpen.Enqueue(panel);
        if (!_isAnyPanelOpen)
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        StartCoroutine(CrShowPanel());
    }

    public void ClosePanel()
    {
        _currentPanel.SetActive(false);
        _blackBackground.gameObject.SetActive(false);
        if (_panelsToOpen.Count > 0)
        {
            ShowPanel();
        }
        else
        {
            _isAnyPanelOpen = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isAnyPanelOpen)
            {
                if (_galleryManager.CanBack())
                {
                    _galleryManager.Back();
                }
                else if(_customizeController.CanBack())
                {
                    _customizeController.Back();
                }
                else
                {
                    ClosePanel();
                }
            }
            else
            {
                RequestShowPanel(_closeGamePanel);
            }
        }
    }
    public void CloseApp()
    {
        Application.Quit();
    }
    public bool GetPanelState()
    {
        return _isAnyPanelOpen;
    }
    IEnumerator CrShowPanel()
    {
        yield return null;
        GameEvents.PlaySFX.Invoke("Click");
        GameObject panelToShow = _panelsToOpen.Dequeue();
        Color transparentBlack = new Color(0, 0, 0, 0f);
        panelToShow.SetActive(true);
        Color semiTransparentBlack = new Color(0, 0, 0, 0.8f);
        _blackBackground.color = transparentBlack;
        _blackBackground.gameObject.SetActive(true);
        _isAnyPanelOpen = true;
        _currentPanel = panelToShow;
        _blackBackground.color = semiTransparentBlack;
    }
}
