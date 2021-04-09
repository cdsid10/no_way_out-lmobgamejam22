using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Internal Data")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] bool isPaused;
    [SerializeField] InputManager inputManager;
    [SerializeField] Inputs inputs;
    [SerializeField] Inputs.MovementActions inputMovement;

    void Awake() => SetUp();

    void Update() => HandlePause();

    void OnEnable() => inputs.Enable();

    void OnDisable() => inputs.Disable();

    void HandlePause()
    {
        if (inputMovement.Pause.triggered)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                inputManager.ToggleCursor();
            }
            else
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                inputManager.ToggleCursor();
            }
        }
    }

    void SetUp()
    {
        //Data SetUp
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);

        //Inputs SetUp
        inputs = new Inputs();
        inputMovement = inputs.Movement;
    }
}
