using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoLoadConfiguration());
    }

    IEnumerator CoLoadConfiguration()
    {
        yield return new WaitForSeconds(2f);
        GameEvents.LoadScene.Invoke("Configuration");
    }

    void Update()
    {
        
    }
}
