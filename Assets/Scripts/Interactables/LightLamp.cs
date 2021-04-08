using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLamp : MonoBehaviour, IInteractable
{
    [Header("Data")]
    [SerializeField] Material[] lightMats;

    [Header("Internal Data")]
    [SerializeField] bool isOn = true;
    [SerializeField] Light lightComp;

    void Awake() => SetUp();

    public void Interact() => ToggleLight();

    void ToggleLight()
    {
        if (isOn)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].DisableKeyword("_EMISSION");
            }
            lightComp.enabled = false;
        } else
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].EnableKeyword("_EMISSION");
            }
            lightComp.enabled = true;
        }
        isOn = !isOn;
    }

    void SetUp()
    {
        lightComp = GetComponentInChildren<Light>();
        lightMats = GetComponent<Renderer>().materials;
    }
}
