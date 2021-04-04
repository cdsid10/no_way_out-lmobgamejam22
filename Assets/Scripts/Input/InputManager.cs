using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Data")] //These can be set up in the Inspector to personalize the player
    public float lookSens = 100f;
    public float lookClamp = 85f;
    public float moveSpeed = 10f;
    public float airMoveDivider = 2f;
    public float jumpHeight = 2.5f;
    public float gravity = -9.81f;
    public bool invertLook = false;
    [SerializeField] LayerMask playerLayer;

    [Header("Internal Data")] //These are internal variables that are automatically set. Useful for debug.
    [SerializeField] GameObject player;
    [SerializeField] CharacterController playerController;
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 lookInput;
    [SerializeField] Vector3 vertVel = Vector3.zero;
    [SerializeField] float xRot = 0;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;
    [SerializeField] Inputs inputs;
    [SerializeField] Inputs.MovementActions inputMovement;

    #region Unity Methods
    void Awake() => SetUp();

    void Update()
    {       
        HandleMovement(moveInput);
        HandleLook(lookInput);
        HandleJump();
    }

    void OnEnable() => inputs.Enable();

    void OnDisable() => inputs.Disable();
    #endregion

    #region Custom Methods
    void HandleMovement(Vector2 moveInp)
    {
        //Compact *if* for slower air movement
        Vector3 horVel = isGrounded ? (player.transform.right * moveInp.x + player.transform.forward * moveInp.y) * moveSpeed : (player.transform.right * moveInp.x + player.transform.forward * moveInp.y) * moveSpeed / airMoveDivider;
        playerController.Move(horVel * Time.deltaTime);
        playerController.Move(vertVel * Time.deltaTime);
        //Groundcheck and gravity calcs
        isGrounded = Physics.CheckSphere(new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z), 0.4f, ~playerLayer);
        vertVel.y = isGrounded && !isJumping ? 0f : vertVel.y += gravity * Time.deltaTime;
    }

    void HandleLook(Vector2 lookInp)
    {
        //Sets look axis
        float lookX = lookInp.x * lookSens * Time.deltaTime;
        float lookY = lookInp.y * lookSens * Time.deltaTime;
        //Clamps vertical axis
        xRot = invertLook ? xRot += lookY : xRot -= lookY;
        xRot = Mathf.Clamp(xRot, -lookClamp, lookClamp);
        //Moves camera and player
        mainCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        player.transform.Rotate(Vector3.up, lookX);
    }

    void HandleJump()
    {
        if (isGrounded && inputMovement.Jump.triggered)
        {
            isJumping = true;
            vertVel.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity); //Math function for jumping
        }
        else isJumping = false;
    }

    void ToggleCursor() => Cursor.visible = !Cursor.visible;

    void SetUp()
    {
        //Initialization
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InputManager");
        if (objs.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        //Input Setup
        inputs = new Inputs();
        inputMovement = inputs.Movement;       
        inputMovement.Movement.performed += _ => moveInput = _.ReadValue<Vector2>();
        inputMovement.Look.performed += _ => lookInput = _.ReadValue<Vector2>();
        //Data SetUp
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<CharacterController>();
        mainCamera = Camera.main;
        //Cursor SetUp
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    #endregion
}
