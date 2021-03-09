using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurInstance : MonoBehaviour
{
    int _dinoType;
    int _cellIndex;
    [SerializeField]
    GameObject _otherCell;
    bool _working;
    [SerializeField]
    Animator _dinoAnimator;
    [SerializeField]
    SpriteRenderer _dinoSprite;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    SpriteRenderer[] _spriteRenderers;
    CellManager _cellManager;

    private void Awake()
    {
        StartCoroutine(CrGrow());
        _cellManager = FindObjectOfType<CellManager>();
    }

    IEnumerator CrGrow()
    {
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            transform.localScale = Vector3.one * _animationCurve.Evaluate(i / 0.25f);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
    public void StartWorking()
    {
        _working = true;
        GameEvents.WorkDino.Invoke();
        if (_dinoSprite != null)
        {
            _dinoSprite.color = new Color(95f / 255f, 95f / 255f, 95f / 255f, 1f);
        }

        for(int i = 0; i<_spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = Color.gray;
        }

        if(_dinoAnimator != null)
        {
            _dinoAnimator.speed = 0f;
        }
    }
    public void StopWorking()
    {
        _working = false;
        if (_dinoSprite != null)
        {
            _dinoSprite.color = Color.white;
        }

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = Color.white;
        }

        if (_dinoAnimator != null)
        {
            _dinoAnimator.speed = 1f;
        }
    }

    public void SetDinoLayer(string layerName)
    {
        foreach(SpriteRenderer sprite in _spriteRenderers)
        {
            sprite.sortingLayerID = SortingLayer.NameToID(layerName);
        }
    }

    public void SetCell(int nCell)
    {
        SetDinoLayer("Row" + _cellManager.GetDinoLayerByCellIndex(nCell));
        _cellIndex = nCell;
    }

    public void RefreshLayer()
    {
        SetDinoLayer("Row" + _cellManager.GetDinoLayerByCellIndex(_cellIndex));
    }

    public void SetDino(int nDino)
    {
        _dinoType = nDino;
    }
    public int GetDinosaur()
    {
        return _dinoType;
    }
    public int GetCellNumber()
    {
        return _cellIndex;
    }
    public bool IsWorking()
    {
        return _working;
    }
}
