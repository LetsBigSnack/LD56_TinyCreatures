using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: why just once
public class SceneChangeManager : MonoBehaviour
{
    
    public static SceneChangeManager Instance;
    
    private bool isSceneChanging = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            GameManager.Instance.ChangeState(sceneName);
            isSceneChanging = false;
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
