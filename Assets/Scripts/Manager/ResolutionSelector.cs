using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ResolutionSelector : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;
    public Button startGameButton;

    private List<Resolution> availableResolutions;
    private List<Resolution> validResolutions; // Store valid 16:9 resolutions

    void Start()
    {
        // Fetch available screen resolutions
        availableResolutions = new List<Resolution>(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        // Filter and store valid 16:9 resolutions
        validResolutions = new List<Resolution>();
        List<string> options = new List<string>();

        foreach (var resolution in availableResolutions)
        {
            // Check if the resolution matches 16:9 aspect ratio (1.77 ratio)
            float aspectRatio = (float)resolution.width / (float)resolution.height;
            if (Mathf.Approximately(aspectRatio, 16f / 9f))  // Check if it's approximately 16:9
            {
                validResolutions.Add(resolution);
                options.Add($"{resolution.width} x {resolution.height} @ {resolution.refreshRate} Hz");
            }
        }

        // Add the filtered valid resolutions to the dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = options.Count - 1; // Select the highest resolution by default

        // Set up window mode options
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new List<string> { "Windowed", "Fullscreen" });
        windowModeDropdown.value = 0; // Default to Windowed

        // Set up button click listener
        startGameButton.onClick.AddListener(OnStartGame);
    }

    void OnStartGame()
    {
        // Get selected resolution and window mode
        int selectedResolutionIndex = resolutionDropdown.value;
        bool isFullscreen = windowModeDropdown.value == 1; // 1 = Fullscreen

        // Set the screen resolution and window mode based on selected 16:9 resolution
        Resolution selectedResolution = validResolutions[selectedResolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, isFullscreen);

        // Load the main game scene (replace "MainScene" with your actual game scene name)
        SceneManager.LoadScene("StartScene");
    }
}
