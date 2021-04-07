using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [Header("Data")]
    [SerializeField] bool isActive;
    [SerializeField] Mode mode;
    [SerializeField] List<GameObject> interactTargets;

    enum Mode { Switch, OneTime}

    public void Interact() => Press();

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
                }
          
                break;
            default:
                break;
        }
    }
}
