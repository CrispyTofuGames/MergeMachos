using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashBin : MonoBehaviour
{
    RectTransform rectTr;
    Coroutine expandCr;
    Coroutine contractCr;
    bool overTrashBin = false;

    private void Start()
    {
        rectTr = GetComponent<RectTransform>();
    }
    public void EnterPoint()
    {
        expandCr = StartCoroutine(Expand());
        overTrashBin = true;
    }
    public void ExitPoint()
    {
        contractCr = StartCoroutine(Contract());
        StartCoroutine(WaitForPointUp());
    }
    IEnumerator Expand()
    {
        Vector2 currentSize = rectTr.sizeDelta;
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            rectTr.sizeDelta = Vector2.Lerp(currentSize, new Vector2(120, 120), 0.5f / i);
            yield return null;
        }
        rectTr.sizeDelta = new Vector2(120, 120);
    }

    IEnumerator Contract()
    {
        Vector2 currentSize = rectTr.sizeDelta;
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            rectTr.sizeDelta = Vector2.Lerp(currentSize, new Vector2(100, 100), 0.5f / i);
            yield return null;
        }
        rectTr.sizeDelta = new Vector2(100, 100);
    }

    public bool IsOverTrashBin()
    {
        return overTrashBin;
    }

    IEnumerator WaitForPointUp()
    {
        yield return null;
        overTrashBin = false;
    }
}
