using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] bool isActive;
    [SerializeField] Mode mode;
    [SerializeField] List<GameObject> interactTargets;
    
    enum Mode {Switch, Press}

    public void Update()
    {
        HandleButton();
    }

    bool IsPressed()
    {
        bool isPressed = false;
        Collider[] gosInRange = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.localScale * 0.4f);
        for (int i = 0; i < gosInRange.Length; i++)
        {
            if (!gosInRange[i].gameObject.TryGetComponent(out MultiTag mt)) continue;
            isPressed = mt.CompareTags("HasWeight");
        }
        return isPressed;
    }

    void HandleButton()
    {
        switch (mode)
        {
            case Mode.Switch:
                if (IsPressed())
                {
                    foreach (var interactTarget in interactTargets)
                    {
                        if (interactTarget.TryGetComponent(out IInteractable interactable))
                        {
                            interactable.Interact();
                        }
                        else continue;
                    }
                    enabled = false;
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
                }
                break;
            default:
                break;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.localScale * 0.8f);
    }
}
