using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    [Header("Internal Data")]
    [SerializeField] InputManager inputManager;

    void Awake() => SetUp();

    public void Interact() => PickUp();

    void PickUp() => inputManager.TogglePickUp(gameObject);

    void SetUp()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }
}
