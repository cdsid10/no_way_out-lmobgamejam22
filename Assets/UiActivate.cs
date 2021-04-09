using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiActivate : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, uiCanvas.transform.position);
        uiCanvas.SetActive(distanceToPlayer <= 2.65f);
    }
}
