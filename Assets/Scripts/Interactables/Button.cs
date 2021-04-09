using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [Header("Data")]
    [SerializeField] Mode mode;
    [SerializeField] Material[] lightMats;
    [SerializeField] List<GameObject> interactTargets;

    [Header("Internal Data")]
    [SerializeField] bool isActive;
    [SerializeField] GameObject uiCanvas, player;
    [SerializeField] Quaternion uiOrigRot;
    [SerializeField] Light buttonLight;
    [SerializeField] Camera mainCamera;

    enum Mode { Switch, OneTime, Press}

    public void Interact() => Press();

    public void Awake() => SetUp();

    void Update()
    {
        HandleUI();
    }

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
            case Mode.Press:
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
                    Invoke(nameof(ResetButton), 0.15f);
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
                lightMats[i].DisableKeyword("_EMISSION");
            }
            buttonLight.enabled = false;
        }
        else if (!isActive)
        {
            for (int i = 0; i < lightMats.Length; i++)
            {
                lightMats[i].EnableKeyword("_EMISSION");
            }
            buttonLight.enabled = true;
        }
    }

    void HandleUI()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        switch (mode)
        {
            case Mode.Switch:
                
                uiCanvas.SetActive(distance <= 2f);
                uiCanvas.transform.rotation = mainCamera.transform.rotation * uiOrigRot;

                break;
            case Mode.OneTime:
                if (!isActive)
                {
                    uiCanvas.SetActive(distance <= 2f);
                    uiCanvas.transform.rotation = mainCamera.transform.rotation * uiOrigRot;
                }
                if (isActive) uiCanvas.SetActive(false);
                break;
            case Mode.Press:

                uiCanvas.SetActive(distance <= 2f);
                uiCanvas.transform.rotation = mainCamera.transform.rotation * uiOrigRot;

                break;
            default:
                break;
        }

    }

    void ResetButton()
    {
        isActive = false;
        HandleFX();
    }

    void SetUp()
    {
        buttonLight = GetComponentInChildren<Light>();
        lightMats = GetComponent<Renderer>().materials;
        uiCanvas = transform.GetChild(0).gameObject;
        mainCamera = Camera.main;
        uiOrigRot = uiCanvas.transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player");
        HandleFX();
    }
}
