using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChanger : MonoBehaviour
{
    bool canSwap = false;
    bool _isOpen = false;
    string username;
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    UsernameObserver _usernameObserver;
    [SerializeField]
    UsernameObserver _profileNameObserver;
    [SerializeField]
    PanelManager _panelManager;
    public void TypeName(string n)
    {
        if(n.Length > 1 && n.Length < 12)
        {
            canSwap = true;
            username = n;
        }
        else
        {
            canSwap = false;
        }
    }

    public void SaveName()
    {
        if(username != null) 
        {
            UserDataController.ChangeName(username);
            _usernameObserver.Refresh();
            _profileNameObserver.Refresh();
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        if (CurrentSceneManager._canChangeName)
        {
            if (!_isOpen)
            {
                if (!mainPanel.activeSelf)
                {
                    _isOpen = true;
                    _panelManager.RequestShowPanel(mainPanel);
                }
            }
        }
    }

    public void ClosePanel()
    {
        _isOpen = false;
        _panelManager.ClosePanel();
    }
}
