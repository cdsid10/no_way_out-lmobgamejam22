using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Internal Data")]
    public bool isPaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] InputManager inputManager;
    [SerializeField] SettingsManager settingsManager;
    [SerializeField] Inputs inputs;
    [SerializeField] Inputs.MovementActions inputMovement;

    void Awake() => SetUp();

    void Update() => HandlePause();

    void OnEnable() => inputs.Enable();

    void OnDisable() => inputs.Disable();

    void HandlePause()
    {
        if (inputMovement.Pause.triggered && !settingsManager.settingsMenu.activeSelf)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            inputManager.enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            inputManager.enabled = true;
            Cursor.visible = false;
        }
    }

    void SetUp()
    {
        //Data SetUp
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        settingsManager = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsManager>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);

        //Inputs SetUp
        inputs = new Inputs();
        inputMovement = inputs.Movement;
    }
}
