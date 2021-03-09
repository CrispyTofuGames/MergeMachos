using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstance : MonoBehaviour
{
    [SerializeField]
    BoxManager.BoxType _boxType;
    CellInstance _myCell;
    float _remainingTime = 1f;
    [SerializeField]
    AnimationCurve _animationCurve;
    private void Awake()
    {
        StartCoroutine(CrGrow());
    }

    IEnumerator CrGrow()
    {
        for(float i = 0; i<0.25f; i += Time.deltaTime)
        {
            transform.localScale = Vector3.one* _animationCurve.Evaluate(i / 0.25f);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void Init(CellInstance myCell,float remainingTime )
    {
        _remainingTime = remainingTime;
        _myCell = myCell;
    }

    private void Update()
    {
        _remainingTime -= Time.deltaTime;
        if(_remainingTime < 0)
        {
            switch (_boxType)
            {
                case BoxManager.BoxType.LootBox:
                    _myCell.DestroyBox();
                    break;
                case BoxManager.BoxType.RewardedBox:
                case BoxManager.BoxType.StandardBox:
                    _myCell.OpenBox();
                    break;
            }
        }

    }
}
