using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UserDataController.DeleteFile();
        SceneManager.LoadScene("Splash");
    }
}
