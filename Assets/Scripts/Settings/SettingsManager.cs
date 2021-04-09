using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] TMP_Dropdown resDropdown;

    [Header("Internal Data")]
    [SerializeField] Resolution[] resolutions;

    #region Unity Methods
    void Awake() => SetUp();
    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
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
        gameObject.SetActive(false);
    }
    public void Continue()
    {
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void SetUp()
    {
        //Initialization SetUp
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SettingsManager");
        if (objs.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

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
