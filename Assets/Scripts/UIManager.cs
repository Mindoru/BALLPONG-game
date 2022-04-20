using System;
using System.Collections;
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
    // Variables para el control de volumen
    [Header("Volume settings")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] float volumeSliderValue;

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
    Resolution[] resolutions;

    void Start()
    {
        // Volume settings
        HandleVolume();

        // FPS settings
        HandleFPS();

        // Screen settings
        HandleScreen();
    }

    void HandleVolume()
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
        fullScreenToggle.isOn = Screen.fullScreen;
        CheckResolution();
    }


    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
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
        volumeSliderValue = value;
        PlayerPrefs.SetFloat("volumeAudio", volumeSliderValue);
        AudioListener.volume = volumeSlider.value;
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
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
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            float aspectRatio = (float)resolutions[i].width / (float)resolutions[i].height;
            if (aspectRatio < 1.7) continue; // allow 16/9 only

            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolution;
        resolutionsDropdown.RefreshShownValue();

        resolutionsDropdown.value = PlayerPrefs.GetInt("resolution", 0);
    }

    public void ChangeResolution(int index)
    {
        PlayerPrefs.SetInt("resolution", resolutionsDropdown.value);

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
