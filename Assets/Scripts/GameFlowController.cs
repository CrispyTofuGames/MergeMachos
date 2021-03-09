using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField]
    TransitionsController _transitionsController;
    float _sceneLoadAmount;

    public void Awake()
    {
        
        GameEvents.LoadScene.AddListener(LoadScene);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(CoLoadSceneAsync(sceneName));
    }
    public IEnumerator CoLoadSceneAsync(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            _sceneLoadAmount = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(_transitionsController.coFadeToBlack());

        asyncLoad.allowSceneActivation = true;

    }
    public float GetLevelLoadAmount()
    {
        return _sceneLoadAmount;
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(coLoadScene(sceneName));

    }
    IEnumerator coLoadScene(string sn)
    {
        yield return StartCoroutine(_transitionsController.coFadeToBlack());
        yield return null;
        SceneManager.LoadScene(sn);
    }

}
