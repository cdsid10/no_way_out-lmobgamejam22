using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiActivate : MonoBehaviour
{
    [SerializeField] private GameObject UiInstruction;
    private BoxCollider _boxCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        UiInstruction.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        UiInstruction.SetActive(false);
    }
}
