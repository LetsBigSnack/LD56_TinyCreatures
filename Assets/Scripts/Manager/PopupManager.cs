using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public enum StringState
    {
        Entry,
        Battle,
        Inspector,
        Fusion,
        Shop,
        Settings,
        Input,
        Delete
    }

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
        [SerializeField]
        private GameObject _inputPopup;
        [SerializeField]
        private GameObject _deletePopup;
        
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

        [SerializeField] private TMP_InputField inputField;
        private string inputName;

        public string InputName { get { return inputName; } }
        
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            soundManager = FindObjectOfType<SoundManager>();
            
        }

        private void OnEnable()
        {
            inputField.onValueChanged.AddListener(data => { OnInputChanges(data); });
        }

        public void OnInputChanges(string data)
        {
            inputName = data;
        }

        public void SetInputText(string text)
        {
            inputName = text;
            inputField.text = text;
        }
        
        public void ClearInput()
        {
            inputName = "";
            inputField.text = "";
        }

        public void ConfirmSlotName()
        {
            UI_Save_Slot currSlot = UI_SaveSlotHelper.Instance.SelectSlot();
            Debug.Log("Currslot = "+ currSlot);
            if (currSlot != null) 
            {
                if (currSlot.OnConfirmSlot())
                {
                    _popupContainer.SetActive(false);
                }
                ClearInput();
            }
        }
        
        public void ConfirmDeletion()
        {
            UI_Save_Slot currSlot = UI_SaveSlotHelper.Instance.SelectSlot();
            Debug.Log("Currslot = "+ currSlot);
            if (currSlot != null)
            {
                currSlot.DeleteSlot();
                _popupContainer.SetActive(false);
            }
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
                ClearInput();
            }        
        }

        public void ViewPopup(StringState newState)
        {
            //TODO: set everything to false
            
            
            if (newState != StringState.Settings && !TutorialManager.Instance.CheckBool(newState) && newState != StringState.Input && newState != StringState.Delete)
            {
                robot.SetActive(true);
                TutorialManager.Instance.SetBool(newState);
                currentPage = 0;
                ChangeState(newState);
                _popupText.text = currentTexts[currentPage];
                _popupContainer.SetActive(true);
                _settingsPopup.SetActive(false);
                _textPopup.SetActive(true);
                _inputPopup.SetActive(false);
                _deletePopup.SetActive(false);
            }
            else if(newState == StringState.Settings)
            {
                robot.SetActive(false);
                _popupContainer.SetActive(true);
                _textPopup.SetActive(false);
                _settingsPopup.SetActive(true);
                _inputPopup.SetActive(false);
                _deletePopup.SetActive(false);
            }
            else if(newState == StringState.Input)
            {
                robot.SetActive(false);
                _popupContainer.SetActive(true);
                _textPopup.SetActive(false);
                _settingsPopup.SetActive(false);
                _inputPopup.SetActive(true);
                _deletePopup.SetActive(false);
            }else if (newState == StringState.Delete)
            {
                robot.SetActive(false);
                _popupContainer.SetActive(true);
                _textPopup.SetActive(false);
                _settingsPopup.SetActive(false);
                _inputPopup.SetActive(false);
                _deletePopup.SetActive(true);
            }
        }

        public void MainMenu(string scenename)
        {
            soundManager.PlaySFX("Click");
            SaveLoadManager.Instance.SaveGame();
            SceneChangeManager.Instance.ChangeScene(scenename);
            ContinuePopup();
        }
    
    
        public void ReloadScene()
        {
            soundManager.PlaySFX("Click");
            SceneChangeManager.Instance.ChangeScene(SceneManager.GetActiveScene().name);
            ContinuePopup();
        }
    }
}