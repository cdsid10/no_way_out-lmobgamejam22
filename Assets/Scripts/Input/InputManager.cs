using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Data")]
    public float lookSens = 100f;

    [Header("Internal Data")]
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    [SerializeField] float xRot = 0f;

    void Awake() => SetUp();

    void Update()
    {
        HandleLookInputMouse();
        HandleLookInputController();

        Debug.Log(Input.GetAxis("Horizontal"));
    }

    void HandleLookInputMouse()
    {
        float lookX = Input.GetAxis("Mouse X") * lookSens * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * lookSens * Time.deltaTime;

        player.transform.Rotate(Vector3.up * lookX);
        xRot -= lookY;
        xRot = Mathf.Clamp(xRot, -85f, 85f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    void HandleLookInputController()
    {
        float lookX = Input.GetAxis("RightStick X") * lookSens * Time.deltaTime;
        float lookY = Input.GetAxis("RightStick Y") * lookSens * Time.deltaTime;

        player.transform.Rotate(Vector3.up * lookX);
        xRot -= lookY;
        xRot = Mathf.Clamp(xRot, -85f, 85f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
    }

    void ToggleCursor() => Cursor.visible = !Cursor.visible;

    void SetUp()
    {
        //Initialization
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InputManager");
        if (objs.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //Data SetUp
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;

        //Cursor SetUp
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
