using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class UIManager : MonoBehaviour
{
    // Score GUI
    [SerializeField] TextMeshProUGUI matchPointText;

    // Variables para el control de volumen
    [Header("Volume settings")]
    [SerializeField] Slider volumeSlider;

    // Variables para controlar límite de FPS
    [Header("FPS settings")]
    int[] FPSValues = { 25, 30, 48, 50, 60, 72, 90, 100, 144, 240, 300 };
    [SerializeField] TextMeshProUGUI FPSLimitText;
    [SerializeField] Button FPSButtonPrevious;
    [SerializeField] Button FPSButtonNext;
    [SerializeField] int FPSDefaultValue = 60;
    [SerializeField] int FPSCurrentValue;
    int FPSCurrentValueIndex;

    [Header("Screen settings")]
    // Variables para el control de pantalla
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [Space(20)]
    List<Resolution> validResolutions;

    void Start()
    {
        if (matchPointText != null)
        {
            matchPointText.text = $"Match Point: {GameManager.Instance.maxScore.ToString()}";
        }

        // Volume settings
        InitialVolume();

        // FPS settings
        HandleFPS();

        // Screen settings
        HandleScreen();
    }

    void InitialVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volumeAudio", 0.5f);
        AudioListener.volume = volumeSlider.value;
    }

    void HandleFPS()
    {
        FPSDefaultValue = PlayerPrefs.GetInt("fps", FPSDefaultValue);
        FPSCurrentValueIndex = Array.IndexOf(FPSValues, FPSDefaultValue);

        SetFPS(FPSDefaultValue);
        OptionSlider.SetValue(FPSLimitText, FPSDefaultValue);
        FPSButtonPrevious.onClick.AddListener(() => OptionSlider.PreviousValue(FPSValues, ref FPSCurrentValue, ref FPSCurrentValueIndex, FPSLimitText, () => SetFPS(FPSCurrentValue)));
        FPSButtonNext.onClick.AddListener(() => OptionSlider.NextValue(FPSValues, ref FPSCurrentValue, ref FPSCurrentValueIndex, FPSLimitText, () => SetFPS(FPSCurrentValue)));
    }

    void HandleScreen()
    {
        bool VSyncPlayerPrefs = PlayerPrefs.GetInt("vsync", 0) > 0 ? true : false;
        SetVSync(VSyncPlayerPrefs);
        vsyncToggle.isOn = VSyncPlayerPrefs;

        int fullScreenPrefs = PlayerPrefs.GetInt("fullScreen", Screen.fullScreen ? 1 : 0);
        fullScreenToggle.isOn = fullScreenPrefs == 1 ? true : false;
        CheckResolution();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("volumeAudio", value);
        AudioListener.volume = volumeSlider.value;
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        PlayerPrefs.SetInt("fullScreen", fullScreen ? 1 : 0);
    }

    public void SetVSync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
        PlayerPrefs.SetInt("vsync", value ? 1 : 0);
    }

    public void SetFPS(int value)
    {
        Application.targetFrameRate = value;
        PlayerPrefs.SetInt("fps", value);
    }

    public void CheckResolution()
    {
        resolutionsDropdown.ClearOptions();
        Resolution[] resolutions = Screen.resolutions;
        validResolutions = Screen.resolutions.ToList();
        List<string> options = new List<string>();
        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution loopedResolution = resolutions[i];

            float aspectRatio = (float)loopedResolution.width / (float)loopedResolution.height;
            float targetRatio = 16.0f / 9.0f;
            if (aspectRatio / targetRatio >= 1.0f && (loopedResolution.refreshRate >= 50) && !validResolutions.Contains(loopedResolution)) // allow 16/9 50Hz+ only
            {
                validResolutions.Add(loopedResolution);
            }
        }

        for (int i = 0; i < validResolutions.Count; i++)
        {
            Resolution validResolution = validResolutions[i];
            string option = string.Format("{0}x{1} @{2}Hz", validResolution.width, validResolution.height, validResolution.refreshRate);
            options.Add(option);

            bool isCurrentResolution = validResolution.width == Screen.currentResolution.width && validResolution.height == Screen.currentResolution.height && validResolution.refreshRate == Screen.currentResolution.refreshRate;
            if (isCurrentResolution)
            {
                currentResolution = i;
            }
        }

        int resolutionPrefs = PlayerPrefs.GetInt("resolution", currentResolution);
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = resolutionPrefs;
        resolutionsDropdown.RefreshShownValue();
    }

    public void ChangeResolution(int index)
    {
        Resolution resolution = validResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionsDropdown.value);
    }
}
