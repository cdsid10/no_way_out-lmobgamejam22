using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLamp : MonoBehaviour, IInteractable
{
    [Header("Internal Data")]
    [SerializeField] bool isOn = true;
    [SerializeField] Light lightComp;

    void Awake() => SetUp();

    public void Interact() => ToggleLight();

    void ToggleLight()
    {
        if (isOn) lightComp.enabled = false; else lightComp.enabled = true;
        isOn = !isOn;
    }

    void SetUp()
    {
        lightComp = GetComponent<Light>();
    }
}
