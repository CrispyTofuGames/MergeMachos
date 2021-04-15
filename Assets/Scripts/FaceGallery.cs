using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGallery : MonoBehaviour
{
    [SerializeField] GameObject _horizontalFaceGallery;
    List<Transform> _createdRows;
    bool _initialized = false;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        int counter = 0;
        _createdRows = new List<Transform>();
        for (int i = 0; i < 5; i++) //< x == numero de filas
        {
            Transform t = Instantiate(_horizontalFaceGallery, transform).transform;
            t.GetChild(0).GetComponent<GalleryFace>().Init(counter, true);
            counter++;
            t.GetChild(1).GetComponent<GalleryFace>().Init(counter, true);
            counter++;
            t.GetChild(2).GetComponent<GalleryFace>().Init(counter, true);
            counter++;
            _createdRows.Add(t);
        }
        _initialized = true;
    }

    public void RefreshFaces()
    {
        if (_initialized)
        {
            int counter = 0;
            for (int i = 0; i < _createdRows.Count; i++)
            {
                _createdRows[i].GetChild(0).GetComponent<GalleryFace>().Init(counter, true);
                counter++;
                _createdRows[i].GetChild(1).GetComponent<GalleryFace>().Init(counter, true);
                counter++;
                _createdRows[i].GetChild(2).GetComponent<GalleryFace>().Init(counter, true);
                counter++;
            }
        }
    }
}
