using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] List<GameObject> interactTargets;

    [Header("InternalData")]
    [SerializeField] bool doOnce;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doOnce) StartCoroutine(SequentialActivate());
    }

    IEnumerator SequentialActivate()
    {
        doOnce = true;
        foreach (var interactTarget in interactTargets)
        {
            if (interactTarget.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
                yield return new WaitForSeconds(0.5f);
            }
            else continue;
        }
        yield return null;
    }

}
