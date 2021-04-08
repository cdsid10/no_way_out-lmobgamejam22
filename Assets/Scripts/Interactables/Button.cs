using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [Header("Data")]
    [SerializeField] Mode mode;
    [SerializeField] List<GameObject> interactTargets;

    [Header("Internal Data")]
    [SerializeField] bool isActive;
    [SerializeField] Light buttonLight;
    [SerializeField] Material[] lightMats;

    enum Mode { Switch, OneTime}

    public void Interact() => Press();

    public void Awake() => SetUp();

    void Press()
    {
        switch (mode)
        {
            case Mode.Switch:
                foreach (var interactTarget in interactTargets)
                {
                    if (interactTarget.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.Interact();
                    }
                    else continue;
                }
                isActive = !isActive;
                HandleFX();
                break;
            case Mode.OneTime:
                if (!isActive)
                {
                    foreach (var interactTarget in interactTargets)
                    {
                        if (interactTarget.TryGetComponent(out IInteractable interactable))
                        {
                            interactable.Interact();
                        }
                        else continue;
                    }
                    isActive = true;
                    HandleFX();
                }
          
                break;
            default:
                break;
        }
    }
    void HandleFX()
    {
        if (isActive)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].EnableKeyword("_EMISSION");
            }
            buttonLight.enabled = true;
        }
        else if (!isActive)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].DisableKeyword("_EMISSION");
            }
            buttonLight.enabled = false;
        }
    }

    void SetUp()
    {
        buttonLight = GetComponentInChildren<Light>();
        HandleFX();
    }
}
