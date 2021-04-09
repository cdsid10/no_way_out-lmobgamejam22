using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteractable : MonoBehaviour, IInteractable
{
    [Header("Data")]
    [SerializeField] bool startOn = true;

    public void Awake() => SetUp();

    public void Interact() => ToggleText();

    void ToggleText()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void SetUp()
    {
        if (!startOn) ToggleText();
    }
}
