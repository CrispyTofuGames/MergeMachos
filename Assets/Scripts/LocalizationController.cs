using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LocalizationController : MonoBehaviour
{
    public static Dictionary<string, string> _localizedData;
    static string _fullJSON;  

    public static string GetValueByKey(string key)
    {
        return _localizedData[key];
    }

    private void Awake()
    {
        if (_localizedData == null)
        {
            TextAsset myTextData = (TextAsset)Resources.Load("LocalizedData/English");
            string dataAsJson = "{\"items\":" + myTextData.text + "}";
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            _localizedData = new Dictionary<string, string>();
            for(int  i =0; i< loadedData.items.Length; i++)
            {
                _localizedData.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
        }
    }

    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }

    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }
    public static void PrintDictionary()
    {
        foreach (KeyValuePair<string, string> kv in _localizedData)
        {
            Debug.Log("KEY: " + kv.Key + " ==> " + kv.Value);
        }
    }
}
