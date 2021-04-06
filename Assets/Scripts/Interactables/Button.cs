using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] List<GameObject> interactTargets;

    public void Interact() => Press();

    void Press()
    {
        foreach (var interactTarget in interactTargets)
        {
            if (interactTarget.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
            else continue;
        }
    }
}
