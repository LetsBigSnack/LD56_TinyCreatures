using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField]
    private Button _btn1;
    [SerializeField]
    private TextMeshProUGUI _btn1Text;
    [SerializeField]
    private TextMeshProUGUI _popupText;
    [SerializeField]
    private GameObject _popupContainer;
    [SerializeField]
    private GameObject _settingsPopup;

    [SerializeField]
    private string[] popupTexts;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CreatePopup(string popupText, string btn1Text)
    {
        _popupText.text = popupText;
        _btn1Text.text = btn1Text;

        _btn1.onClick.AddListener(() => 
        {
            _popupContainer.SetActive(false);
        });
    }

    public void ViewPopup(int popupType)
    {
        for (int i = 0; i <= popupTexts.Length; i++)
        {
            if (popupType == i && popupType != 0)
            {
                CreatePopup(popupTexts[i], "OK");
                _btn1.gameObject.SetActive(true);
                _popupContainer.SetActive(true);
                _settingsPopup.SetActive(false);
            }
            else if(popupType == 0)
            {
                CreatePopup(" ", " ");
                _btn1.gameObject.SetActive(false);
                _popupContainer.SetActive(true);
                _settingsPopup.SetActive(true);
            }
            else
            {
                Debug.Log("No vallid popUp option");
            }
        }
    }

}
