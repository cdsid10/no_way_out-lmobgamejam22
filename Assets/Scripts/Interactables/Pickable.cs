using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    [Header("Internal Data")]
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject uiCanvas, player;
    [SerializeField] Camera mainCamera;

    void Awake() => SetUp();

    void Update()
    {
        HandleUI();
    }

    public void Interact() => PickUp();

    void PickUp() => inputManager.TogglePickUp(gameObject);

    void HandleUI()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        uiCanvas.SetActive(distance <= 2f && inputManager.pickedObject == null);
        uiCanvas.transform.forward = new Vector3(mainCamera.transform.forward.x, mainCamera.transform.forward.y, mainCamera.transform.forward.z);

    }

    void SetUp()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        uiCanvas = transform.GetChild(0).gameObject;
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
