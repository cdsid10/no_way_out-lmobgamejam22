using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameObject mainMenu;
    public GameObject settingsMenu;
    [SerializeField] TMP_Dropdown resDropdown;

    [Header("Internal Data")]
    [SerializeField] Resolution[] resolutions;

    #region Unity Methods
    void Start() => SetUp();

    #endregion

    #region Internal Methods
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void GoToSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Continue()
    {
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void SetUp()
    {
        //Data Setup
        if (SceneManager.GetActiveScene().buildIndex == 0) Cursor.visible = true;
        settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
        settingsMenu.SetActive(false);

        //Resolutions Dropdown SetUp
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int curResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                curResIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = curResIndex;
        resDropdown.RefreshShownValue();
    }
    #endregion
}
