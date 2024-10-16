using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    private bool isSceneChanging = false;

    public void ChangeScene(string sceneName)
    {
        if (isSceneChanging)
        {
            Debug.Log("Scene change is already in progress.");
            return;
        }

        isSceneChanging = true;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " cannot be loaded. Please check if it is added to the build settings.");
            isSceneChanging = false;
        }
    }

    public void ExitApplication()
    {
        Debug.Log("Exiting application.");
        Application.Quit();
    }

    private void OnEnable()
    {
        isSceneChanging = false;
    }
}
