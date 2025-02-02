using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            StartCoroutine(LoadLevel(sceneName));
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
    
    private static IEnumerator LoadLevel (string sceneName){
        Debug.Log("Loading scene: " + sceneName);
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone){
            yield return null;
        }
        Debug.Log("Finished loading scene: " + sceneName);
        GameManager.Instance.ChangeState(sceneName);
    }
    
}
