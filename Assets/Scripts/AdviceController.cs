using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdviceController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _adviceText;
    [SerializeField]
    AnimationCurve _animCurve;
    [HideInInspector]
    public float moveDuration = 2.5f;
    [HideInInspector]
    public float fadeDuration = 0.25f;
    Color transParentWhite = new Color(1, 1, 1, 0);

    public IEnumerator CrShowText(GameEvents.AdviceEventData adviceKey)
    {
        string localizedKey = LocalizationController._localizedData[adviceKey._mainString];
        string outputString = string.Format(localizedKey, adviceKey._parameter);
        _adviceText.text = outputString;
        for (float i = 0; i< fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(transParentWhite, Color.white, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = Color.white;
    }
    public IEnumerator CrMoveText()
    {     
        for (float i = 0; i < moveDuration; i += Time.deltaTime)
        {
            _adviceText.rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0, 200), _animCurve.Evaluate(i / moveDuration));
            yield return null;
        }
        _adviceText.rectTransform.anchoredPosition = new Vector2(0, 200);
    }
    public IEnumerator CrHideText()
    {
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(Color.white, transParentWhite, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = transParentWhite;
    }
}
