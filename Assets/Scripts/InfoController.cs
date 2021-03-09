using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;
    [SerializeField]
    Color _disabledColor;
    int currentInfoIndex = 0;
    List<Image> _currentPanelFeedback;
    [SerializeField]
    Transform _checkList;
    private void Start()
    {
        _currentPanelFeedback = new List<Image>();
        _panelManager = FindObjectOfType<PanelManager>();
        for(int i = 0; i<_checkList.childCount; i++)
        {
            _currentPanelFeedback.Add(_checkList.GetChild(i).GetComponent<Image>());
        }
    }

    public void ShowInfo()
    {
        _panelManager.RequestShowPanel(_mainPanel);
        currentInfoIndex = 0;
        Move(0);

    }

    public void CloseInfo()
    {
        _panelManager.ClosePanel();
    }

    public void Move(int index)
    {
        if((currentInfoIndex + index) >= 0 && (currentInfoIndex + index) < _currentPanelFeedback.Count) 
        {
            currentInfoIndex += index;
            foreach(Image i in _currentPanelFeedback)
            {
                i.color = _disabledColor;
            }
            _currentPanelFeedback[currentInfoIndex].color = Color.white;
        }
    }
}
