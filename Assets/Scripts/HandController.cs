using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    RectTransform _selfTransform;
    [SerializeField]
    RectTransform _pivotRotation;
    [SerializeField]
    Image _handImage;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    AnimationCurve _dragAnimationCurve;
    [SerializeField]
    Image _touchCircleImage;

    Coroutine _crTouchManager;
    Coroutine _crDragManager;
    Coroutine _crAppear;
    Coroutine _crDisappear;
    Coroutine _crPointIn;
    Coroutine _crPointOut;
    Coroutine _crDrag;
    Coroutine _crDoubleClick;
    Coroutine _crDoubleClickManager;

    float animDuration = 0.25f;
    float pointDuration = 0.5f;
    float doubleClickDuration = 0.1f;
    float dragDuration = 0.75f;
    Color transparentWhite = new Color(1,1,1,0);
    // Start is called before the first frame update
    void Awake()
    {
        _selfTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector3 targetPosition)
    {
        _selfTransform.position = targetPosition;
    }

    public void Appear()
    {
        StartCoroutine(CrAppear());
    }

    public void ResetHand()
    {
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        _touchCircleImage.rectTransform.localScale = Vector3.zero;
    }

    public void StartTouchMode()
    {
        _crTouchManager = StartCoroutine(TouchCr());
    }
    public void StartDoubleClickMode()
    {
        _crDoubleClickManager = StartCoroutine(DoubleClickCr());
    }

    public void StartDragMode(Vector2 positionA, Vector2 positionB)
    {
        _crDragManager = StartCoroutine(DragCr(positionA, positionB));
    }

    public IEnumerator DragCr(Vector2 positionA, Vector2 positionB)
    {
        ResetHand();
        _selfTransform.position = positionA;
        yield return _crAppear = StartCoroutine(CrAppear());
        yield return _crPointIn = StartCoroutine(CrPointIn());
        yield return _crDrag = StartCoroutine(CrDrag(positionA, positionB));
        yield return new WaitForSeconds(0.5f);
        yield return _crPointOut = StartCoroutine(CrPointOut());
        StartCoroutine(DragCr(positionA, positionB));
    }

    public IEnumerator TouchCr() 
    {
        ResetHand();
        yield return _crAppear =  StartCoroutine(CrAppear());
        yield return _crPointIn = StartCoroutine(CrPointIn());
        yield return _crDisappear = StartCoroutine(CrDisappear());
        StartCoroutine(TouchCr());
    }
    public IEnumerator DoubleClickCr()
    {
        ResetHand();
        yield return _crAppear = StartCoroutine(CrAppear());
        yield return _crDoubleClick = StartCoroutine(CrDoubleClick());
        yield return _crDisappear = StartCoroutine(CrDisappear());
        StartCoroutine(DoubleClickCr());
    }

    public void StopDoubleClick()
    {
        if (_crAppear != null)
        {
            StopCoroutine(_crAppear);
        }
        if (_crDoubleClick != null)
        {
            StopCoroutine(_crDoubleClick);
        }
        if (_crDisappear != null)
        {
            StopCoroutine(_crDisappear);
        }        
        if (_crDoubleClickManager != null)
        {
            StopCoroutine(_crDoubleClickManager);
        }

    }

    public void StopTouchCoroutines()
    {
        if (_crAppear != null)
        {
            StopCoroutine(_crAppear);
        }
        if (_crPointIn != null)
        {
            StopCoroutine(_crPointIn);
        }
        if (_crDisappear != null)
        {
            StopCoroutine(_crDisappear);
        }       
        if (_crTouchManager != null)
        {
            StopCoroutine(_crTouchManager);
        }
    }

    public void StopDragCoroutines()
    {
        if (_crAppear != null)
        {
            StopCoroutine(_crAppear);
        }
        if (_crDisappear != null)
        {
            StopCoroutine(_crDisappear);
        }
        if (_crPointIn != null)
        {
            StopCoroutine(_crPointIn);
        }
        if (_crPointOut != null)
        {
            StopCoroutine(_crPointOut);
        }
        if (_crDrag != null)
        {
            StopCoroutine(_crDrag);
        }
        if (_crDragManager != null)
        {
            StopCoroutine(_crDragManager);
        }
    }
    public IEnumerator CrAppear()
    {
        _handImage.gameObject.SetActive(true);
        for (float i = 0; i< animDuration; i+=Time.deltaTime)
        {
            _handImage.color = Color.Lerp(transparentWhite, Color.white, _animationCurve.Evaluate(i / animDuration));
            yield return null;
        }
        _handImage.color = Color.white;
    }

    public IEnumerator CrDisappear()
    {
        for (float i = 0; i < animDuration; i += Time.deltaTime)
        {
            _handImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate(i / animDuration));
            yield return null;
        }
        _handImage.color = transparentWhite;
        _handImage.gameObject.SetActive(false);
    }

    public IEnumerator CrPointIn()
    {
        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0 + (40 * _animationCurve.Evaluate(i / pointDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40));

        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _touchCircleImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animationCurve.Evaluate( i / pointDuration));
            _touchCircleImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate( i / pointDuration));
            yield return null;
        }
        _touchCircleImage.rectTransform.localScale = Vector3.one;
        _touchCircleImage.color = transparentWhite;
    }
    public IEnumerator CrPointOut()
    {
        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40 - (40 * _animationCurve.Evaluate(i / pointDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    public IEnumerator CrDrag(Vector2 pointA, Vector2 pointB)
    {
        for (float i = 0; i < dragDuration; i += Time.deltaTime)
        {
            _selfTransform.position = Vector2.Lerp(pointA, pointB, _dragAnimationCurve.Evaluate(i / dragDuration));
            yield return null;
        }
        _selfTransform.position = pointB;
    }
    public IEnumerator CrDoubleClick()
    {
        for (float i = 0; i < pointDuration/1.5f; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0 + (40 * (i / (pointDuration / 1.5f)))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40));

        for (float i = 0; i < doubleClickDuration; i += Time.deltaTime)
        {
            _touchCircleImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animationCurve.Evaluate(i / doubleClickDuration));
            _touchCircleImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate(i / doubleClickDuration));
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40 - (20 * _animationCurve.Evaluate(i / doubleClickDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 20));

        for (float i = 0; i < doubleClickDuration; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 20 + (20 * _animationCurve.Evaluate(i / doubleClickDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40));

        for (float i = 0; i < doubleClickDuration; i += Time.deltaTime)
        {
            _touchCircleImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animationCurve.Evaluate(i / doubleClickDuration));
            _touchCircleImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate(i / doubleClickDuration));
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 40 - (20 * _animationCurve.Evaluate(i / doubleClickDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 20));
        _touchCircleImage.rectTransform.localScale = Vector3.one;
        _touchCircleImage.color = transparentWhite;
    }
}
