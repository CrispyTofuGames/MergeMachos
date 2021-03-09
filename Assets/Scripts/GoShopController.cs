using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoShopController : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel, _shopMain;
    [SerializeField]
    PanelManager _panelManager;
    public void OpenShopQuest()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseShopQuest()
    {
        _mainPanel.SetActive(false);
    }

    public void OpenShop()
    {
        _mainPanel.SetActive(false);
        if (_panelManager.GetPanelState())
        {
            _panelManager.ClosePanel();
        }
        _panelManager.RequestShowPanel(_shopMain);
    }
}
