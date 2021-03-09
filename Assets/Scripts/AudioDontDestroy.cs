using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDontDestroy : MonoBehaviour
{
    private static AudioDontDestroy _audioInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (_audioInstance == null)
        {
            _audioInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
