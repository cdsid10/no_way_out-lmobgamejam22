using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Vector3 detectionZone;
    [SerializeField] Mode mode;
    [SerializeField] List<GameObject> interactTargets;

    [Header("Internal Data")]
    [SerializeField] bool isActive;
    [SerializeField] Light buttonLight;
    [SerializeField] Material[] lightMats;

    enum Mode {Switch, Press}

    public void Awake() => SetUp();

    public void Update()
    {
        HandleButton();
    }

    bool IsPressed()
    {
        bool isPressed = false;
        Collider[] gosInRange = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), detectionZone*0.5f);
        for (int i = 0; i < gosInRange.Length; i++)
        {
            if (!gosInRange[i].gameObject.TryGetComponent(out MultiTag mt)) continue;
            isPressed = mt.CompareTags("HasWeight");
        }
        return isPressed;
    }

    void HandleFX()
    {
        if (isActive)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].EnableKeyword("_EMISSION");
            }
            buttonLight.intensity = 0.75f;
        }
        else if (!isActive)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].DisableKeyword("_EMISSION");
            }
            buttonLight.intensity = 0f;
        }
    }

    void HandleButton()
    {
        switch (mode)
        {
            case Mode.Switch:
                if (IsPressed() && !isActive)
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
            case Mode.Press:
                if (IsPressed() && !isActive)
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
                else if (!IsPressed() && isActive)
                {
                    foreach (var interactTarget in interactTargets)
                    {
                        if (interactTarget.TryGetComponent(out IInteractable interactable))
                        {
                            interactable.Interact();
                        }
                        else continue;
                    }
                    isActive = false;
                    HandleFX();
                }
                break;
            default:
                break;
        }
    }

    void SetUp()
    {
        buttonLight = GetComponentInChildren<Light>();
        lightMats = GetComponent<Renderer>().materials;
        HandleFX();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y +0.1f, transform.position.z), detectionZone);
    }
}
