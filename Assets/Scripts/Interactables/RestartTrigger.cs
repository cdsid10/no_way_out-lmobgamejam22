using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartTrigger : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] float restartTime;

    [Header("Internal Data")]
    [SerializeField] InputManager inputManager;

    void Awake() => SetUp();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Invoke(nameof(RestartLevel), restartTime);
    }

    void RestartLevel() => inputManager.StartCoroutine(inputManager.RestartLevel());

    void SetUp()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
    }
}
