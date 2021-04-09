using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiActivate : MonoBehaviour
{
    //Add all interactCanvas objects to this List
    [SerializeField] private List<GameObject> uiCanvas;
    
    private void Update()
    {
        for (int i = 0; i < uiCanvas.Count; i++)
        {
            //Still need a way to deactivate said UI if button can only be pressed once and isPressed
            float distance = Vector3.Distance(transform.position, uiCanvas[i].transform.position);
            uiCanvas[i].SetActive(distance <= 2.65f);
        }
    }
}
