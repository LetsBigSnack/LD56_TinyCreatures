using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _popupText;
    [SerializeField]
    private GameObject _popupContainer;
    [SerializeField]
    private GameObject _settingsPopup;
    [SerializeField]
    private GameObject _textPopup;

    private SoundManager soundManager;

    [SerializeField]
    private Button _mainMenu;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button continueTextButton;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private GameObject robot;

    [SerializeField]
    private string[] entryTexts;
    [SerializeField]
    private string[] battleTexts;
    [SerializeField]
    private string[] inspectorTexts;
    [SerializeField]
    private string[] fusionTexts;
    [SerializeField]
    private string[] shopTexts;

    [SerializeField]
    private StringState currentState;

    [SerializeField]
    private string[] currentTexts;

    [SerializeField]
    private int currentPage;


    public enum StringState
    {
        Entry,
        Battle,
        Inspector,
        Fusion,
        Shop,
        Settings
    }

    public Dictionary<string, StringState> stateDictonary;
    
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

        soundManager = FindObjectOfType<SoundManager>();

        stateDictonary = new Dictionary<string, StringState>(){
            { "Entry", StringState.Entry },
            { "Battle", StringState.Battle },
            { "Inspector", StringState.Inspector },
            { "Fusion", StringState.Fusion },
            { "Shop", StringState.Shop },
            { "Settings", StringState.Settings },
        };

    }

    public void SetString(StringState currentState)
    {
        switch (currentState)
        {
            case StringState.Entry:
                currentTexts = entryTexts;
                break;
            case StringState.Battle:
                currentTexts = battleTexts;
                break;
            case StringState.Inspector:
                currentTexts = inspectorTexts;
                break;
            case StringState.Fusion:
                currentTexts = fusionTexts;
                break;
            case StringState.Shop:
                currentTexts = shopTexts;
                break;
        }
    }

    public void ChangeState(StringState state)
    {
        currentState = state;
        SetString(state);
    }

    public void SkipToNextString()
    {
        _popupText.text = currentTexts[currentPage];
        Debug.Log(currentPage);
    }

    public void ContinuePopup()
    {
        soundManager.PlaySFX("Click");
        currentPage++;
        if (currentTexts.Length > currentPage && currentTexts != null)
        {
            SkipToNextString();
        } 
        else
        {
            _popupContainer.SetActive(false);
        }        
    }

    public void ViewPopup(string state)
    {
        StringState newState = stateDictonary.GetValueOrDefault(state);

        if (newState != StringState.Settings && !TutorialManager.Instance.CheckBool(state))
        {
            robot.SetActive(true);
            TutorialManager.Instance.SetBool(state);
            currentPage = 0;
            ChangeState(newState);
            _popupText.text = currentTexts[currentPage];
            _popupContainer.SetActive(true);
            _settingsPopup.SetActive(false);
            _textPopup.SetActive(true);
        }
        else if(newState == StringState.Settings)
        {
            robot.SetActive(false);
            _popupContainer.SetActive(true);
            _textPopup.SetActive(false);
            _settingsPopup.SetActive(true);
        }
    }

    public void MainMenu(string scenename)
    {
        soundManager.PlaySFX("Click");
        SceneManager.LoadScene(scenename);
        ContinuePopup();
    }

    
    
    public void ReloadScene()
    {
        soundManager.PlaySFX("Click");
        // Get the active scene and reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ContinuePopup();
    }
}
