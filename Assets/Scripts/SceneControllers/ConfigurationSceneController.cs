using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nutaku.Unity;
using System;
using UnityEngine.Networking;
using UnityEngine.Audio;

public class ConfigurationSceneController : MonoBehaviour
{
    public float barFillDuration = 2f;
    float _loadAmount;
    [SerializeField]
    bool canGo = false;
    int _frame;
    float tryTime;
    float _elapsedTime;
    int contador;
    [SerializeField]
    AudioMixer _audioMixer;

    // Start is called before the first frame update
    private void Awake()
    {
        SdkPlugin.Initialize();
        tryTime = 30f;

    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Logged")>0)
        {
            PlayerPrefs.SetInt("Logged", 0);
            RequestGetMyProfile();
        }
    }
    void Start()
    {
        if (!UserDataController.Exist())
        {
            UserDataController.InitializeUser();
            StartCoroutine(LoadingBar());
        }
        else
        {
            
            UserDataController.LoadFromFile();
            //canGo = true;
            StartCoroutine(LoadingBar());
        }
        if (PlayerPrefs.HasKey("SFX"))
        {
            if (PlayerPrefs.GetInt("SFX") == 0)
            {
                _audioMixer.SetFloat("SFXVolume", -80f);
            }
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                _audioMixer.SetFloat("OSTVolume", -80f);
            }
        }
#if !UNITY_EDITOR
   RequestGetMyProfile();
#endif
    }


    public IEnumerator LoadingBar()
    {
        while (!canGo)
        {
            yield return null;
        }
        for(float i = 0; i < barFillDuration; i += Time.deltaTime)
        {
            yield return null;
           _loadAmount = i / barFillDuration;
        }
        _loadAmount = 1f;
        GameEvents.LoadScene.Invoke("MainGame");
    }

    public float GetLoadAmount()
    {
        return _loadAmount;
    }
    void RequestGetMyProfile()
    {
        try
        {
            RestApiHelper.Request.GetMyProfile(this, this.RequestGetMyProfileCallback);
        }
        catch (Exception ex)
        {
            canGo = false;
        }
    }

    void RequestGetMyProfileCallback(RawResult rawResult)
    {
        try
        {
            if ((rawResult.statusCode > 199) && (rawResult.statusCode < 300))
            {
                var result = RestApi.HandleRequestCallback<Person>(rawResult);
                var profile = result.GetFirstEntry();
                UserDataController.ChangeName(profile.nickname);
                StartCoroutine(UploadUserData(profile.nickname, profile.id));
            }
            else
            {
                canGo = false;
            }
        }
        catch (Exception ex)
        {
            canGo = false;
        }
        finally
        {
            canGo = true;
        }
    }

    IEnumerator UploadUserData(string username, string nutakuID)
    {
        PlayerPrefs.SetString("NutakuID", nutakuID);
        PlayerPrefs.SetString("NutakuUsername", username);
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("nutakuuserid", nutakuID);
        form.AddField("currentuserdata", JsonUtility.ToJson(UserDataController._currentUserData));
        UnityWebRequest www = UnityWebRequest.Post("http://s852714066.mialojamiento.es/TheDevilsClub/uploaduserdata.php", form);
        yield return www.SendWebRequest();

    }

    IEnumerator CrRequestAddUser(string username, string nutakuID)
    {

        UnityWebRequest www = UnityWebRequest.Get("http://s852714066.mialojamiento.es/TheDevilsClub/adduser.php?username=" + username+"&& nutakuuserid=" + nutakuID);

        yield return www.SendWebRequest();
        canGo = true;
    }

    
}
