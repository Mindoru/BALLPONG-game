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
    [SerializeField] Slider volumeSlider;
    [SerializeField] float volumeSliderValue;

    // Variables para el control de pantalla
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    Resolution[] resolutions;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volumeAudio", 0.5f);
        AudioListener.volume = volumeSlider.value;

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

    public void CheckResolution()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
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
