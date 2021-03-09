using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionsController : MonoBehaviour
{

    [SerializeField]
    Image _fadeImage;
    float _fadeTime = 1f;
    public void Start()
    {
        FadeFromBlack();
    }
    public IEnumerator coFadeToBlack()
    {
        _fadeImage.enabled = true;
        for (float i = 0; i < _fadeTime; i += Time.deltaTime)
        {
            _fadeImage.color = Color.Lerp(Color.clear, Color.black, i / _fadeTime);
            yield return 0;
        }
        _fadeImage.color = Color.black;
    }
    public void FadeToBlack(float time)
    {
        StartCoroutine(coFadeToBlack(time));
    }
    public IEnumerator coFadeToBlack(float t)
    {
        _fadeImage.enabled = true;

        for (float i = 0; i < t; i += Time.deltaTime)
        {
            _fadeImage.color = Color.Lerp(Color.clear, Color.black, i / t);
            yield return 0;
        }
        _fadeImage.color = Color.black;
    }

    public void FadeFromBlack()
    {
        StartCoroutine(coFadeFromBlack());
    }

    public IEnumerator coFadeFromBlack()
    {
        _fadeImage.enabled = true;
        for (float i = 0; i < _fadeTime; i += Time.deltaTime)
        {
            _fadeImage.color = Color.Lerp(Color.black, Color.clear, i / _fadeTime);
            yield return 0;
        }
        _fadeImage.color = Color.clear;
        _fadeImage.enabled = false;

    }
    public void FadeFromBlack(float time)
    {
        StartCoroutine(coFadeFromBlack(time));
    }

    public IEnumerator coFadeFromBlack(float t)
    {
        _fadeImage.enabled = true;
        for (float i = 0; i < t; i += Time.deltaTime)
        {
            _fadeImage.color = Color.Lerp(Color.black, Color.clear, i / t);
            yield return 0;
        }
        _fadeImage.color = Color.clear;
        _fadeImage.enabled = false;

    }

}

