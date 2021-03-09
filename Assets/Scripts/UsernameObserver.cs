using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernameObserver : MonoBehaviour
{
    TextMeshProUGUI _usernameText;
    bool _updated = false;
    private void Start()
    {
        _usernameText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (!_updated)
        {
            if (UserDataController._checked)
            {
                _usernameText.text = UserDataController._currentUserData._username;
                _updated = true;
            }
        }
    }
    public void Refresh()
    {
        _updated = false;
    }
}
