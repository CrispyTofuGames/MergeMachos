using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarWidthController : MonoBehaviour
{
    RectTransform rectTransformBar;
    [SerializeField]
    ConfigurationSceneController _configurationSceneController;

    // Start is called before the first frame update
    void Start()
    {
        rectTransformBar = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransformBar.sizeDelta = new Vector2(50 + (1030 * _configurationSceneController.GetLoadAmount()), 90);
    }

}
