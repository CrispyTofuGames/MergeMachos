using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinAdvice : MonoBehaviour
{
    TextMeshProUGUI _adviceText;
    [SerializeField]
    AnimationCurve _animCurve;
    Image _coinImage;
    float moveDuration = 1f;

    float fadeDuration = 0.25f;
    Color transParentWhite = new Color(1, 1, 1, 0);
    Vector2 initialPos;
    private void Awake()
    {
        _adviceText = transform.GetComponentInChildren<TextMeshProUGUI>();
        _coinImage = transform.GetComponentInChildren<Image>();
        initialPos = _adviceText.rectTransform.anchoredPosition;
    }

    public void Play(Vector3 targetPos, GameCurrency earnedCoins)
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetPos);
        if (transform.parent.gameObject.activeSelf)
        {
            StartCoroutine(AdviceManage(earnedCoins));
        }      
    }

    IEnumerator AdviceManage(GameCurrency nCoins)
    {
        _adviceText.rectTransform.anchoredPosition = initialPos;
        yield return  StartCoroutine(CrShowText(nCoins.GetCurrentMoneyConvertedTo3Chars()));
        StartCoroutine(CrMoveText());
        yield return new WaitForSeconds(moveDuration - fadeDuration * 2f);
        yield return StartCoroutine(CrHideText());
    }

    public IEnumerator CrShowText(string earnedCoins)
    {
        _adviceText.text = earnedCoins;
        _coinImage.color = transParentWhite;
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(transParentWhite, Color.white, _animCurve.Evaluate(i / fadeDuration));
            _coinImage.color = Color.Lerp(transParentWhite, Color.white, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = Color.white;
        _coinImage.color = Color.white;
    }
    public IEnumerator CrMoveText()
    {
        for (float i = 0; i < moveDuration; i += Time.deltaTime)
        {
            _adviceText.rectTransform.anchoredPosition = Vector2.Lerp(initialPos,initialPos + new Vector2(0, 50), _animCurve.Evaluate(i / moveDuration));
            yield return null;
        }
        _adviceText.rectTransform.anchoredPosition = initialPos + new Vector2(0, 50);
    }
    public IEnumerator CrHideText()
    {
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(Color.white, transParentWhite, _animCurve.Evaluate(i / fadeDuration));
            _coinImage.color = Color.Lerp(Color.white, transParentWhite, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = transParentWhite;
        _coinImage.color = transParentWhite;
    }
}
