using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class EditorTools : MonoBehaviour
{
    [MenuItem("Scenes/Configuration")]
    public static void OpenConfiguration()
    {
        LoadScene("Configuration");
    }

    [MenuItem("Scenes/MainGame")]
    public static void OpenMainGame()
    {
        LoadScene("MainGame");
    }
    [MenuItem("CustomTools/ChangeToRelease")]
    public static void ChangeToRelease()
    {

        string releaseConfigurationPath = Application.dataPath+"/Plugins/Android/res/xml/nutaku_game_configuration_release.xml";
        string configurationPath = Application.dataPath + "/Plugins/Android/res/xml/nutaku_game_configuration.xml";
        string releaseConfigurationText =  File.ReadAllText(releaseConfigurationPath);
        PlayerPrefs.SetInt("Release", 1);
        File.WriteAllText(configurationPath, releaseConfigurationText);
    }


    [MenuItem("CustomTools/ChangeToSandbox")]
    public static void ChangeToSandbox()
    {
        string sandboxConfigurationPath = Application.dataPath + "/Plugins/Android/res/xml/nutaku_game_configuration_sandbox.xml";
        string configurationPath = Application.dataPath + "/Plugins/Android/res/xml/nutaku_game_configuration.xml";
        string sandboxConfigurationText = File.ReadAllText(sandboxConfigurationPath);
        PlayerPrefs.SetInt("Release", 0);
        File.WriteAllText(configurationPath, sandboxConfigurationText);
    }

    [MenuItem("CustomTools/Open persistentDataPath Folder")]
    [SerializeField]

    public static void OpenStreamingAssetsFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    [MenuItem("DataManagement/DeletePlayerPrefs")]
    [SerializeField]
    public static void OpenDeletePlayerPrefs()
    {
        int gameState = PlayerPrefs.GetInt("Release");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("Release",gameState);
    }
    [MenuItem("CustomTools/Delete persistentDataPath Folder")]
    [SerializeField]
    public static void DeleteStreamingAssetsFolder()
    {
        File.Delete(Application.persistentDataPath + "/CurrentUserData.json");
    }


    public static void LoadScene(string name)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Si el usuario quiere guardar la escena, guardar
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }

}
