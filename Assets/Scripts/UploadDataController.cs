using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UploadDataController : MonoBehaviour
{
    float _elapsedTime;
    void Start()
    {
        print(JsonUtility.ToJson(new WebParameters(0f, 1f, 2f, 3f)));
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= 300)
        {
            _elapsedTime = 0;
            Save();
        }
    }

    public void Save()
    {
        StartCoroutine(UploadUserData(PlayerPrefs.GetString("NutakuUsername"), PlayerPrefs.GetString("NutakuID")));
    }
    IEnumerator UploadUserData(string username, string nutakuID)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("nutakuuserid", nutakuID);
        form.AddField("currentuserdata", JsonUtility.ToJson(UserDataController._currentUserData));

        UnityWebRequest www = UnityWebRequest.Post("http://s852714066.mialojamiento.es/TheDevilsClub/uploaduserdata.php", form);
        yield return www.SendWebRequest();
    }

    [System.Serializable]
    public class WebParameters
    {
        public float _extraEarnings;
        public float _extraHireDiscount;
        public float _extraIdleEarnings;
        public float _extraGemsAtShop;

        public WebParameters(float extraEarnings, float extraHireDiscount, float extraIdleEarnings, float extraGemsAtShop)
        {
            _extraEarnings = extraEarnings;
            _extraHireDiscount = extraHireDiscount;
            _extraIdleEarnings = extraIdleEarnings;
            _extraGemsAtShop = extraGemsAtShop;
        }
    }

}
