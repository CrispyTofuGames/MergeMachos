using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class ButtonsFeedback : MonoBehaviour
{
    RectTransform _selfTransform;
    Button _selfButton;
    Coroutine _reduceCr;
    Coroutine _restoreCr;

    private void Awake()
    {
        _selfTransform = GetComponent<RectTransform>();
        _selfButton = GetComponent<Button>();

        EventTrigger trigger = _selfButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => Reduce());
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerExit;
        pointerUp.callback.AddListener((e) => Restore());
        trigger.triggers.Add(pointerUp);
    }

    public void Reduce()
    {
        if(_reduceCr != null)
        {
            StopCoroutine(ReduceScale());
        }
        _reduceCr = StartCoroutine(ReduceScale());
    }
    public void Restore()
    {
        if (_restoreCr != null)
        {
            StopCoroutine(RestoreScale());
        }
        if(_selfTransform.localScale.x < 1f)
        {
            _restoreCr = StartCoroutine(RestoreScale());
        }
    }
    IEnumerator ReduceScale()
    {
        for(float i = 0; i<0.15f; i+= Time.deltaTime)
        {
            _selfTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.9f, 0.9f, 0.9f), i/0.15f);
            yield return null;
        }
        _selfTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }
    IEnumerator RestoreScale()
    {
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            _selfTransform.localScale = Vector3.Lerp(new Vector3(0.9f, 0.9f, 0.9f), Vector3.one,  i / 0.25f);
            yield return null;
        }
        _selfTransform.localScale = Vector3.one;
    }
}
