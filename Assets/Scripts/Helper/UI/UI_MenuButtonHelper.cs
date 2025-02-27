using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuButtonHelper : MonoBehaviour
{
    public void StartGame()
    {
        SceneChangeManager.Instance.ChangeScene("NewUISpritesScene");
    }

    public void QuitGame()
    {
        SceneChangeManager.Instance.ExitApplication();
    }
}
