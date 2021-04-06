using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Data")] //These can be set up in the Inspector to personalize the player
    public float lookSens = 100f;
    public float lookClamp = 85f;
    public bool invertLook = false;

    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float airMoveDivider = 2f;
    public float jumpHeight = 2.5f;

    public float gravity = -9.81f;
    public float interactDistance = 2f;


    [SerializeField] LayerMask playerLayer;

    [Header("Internal Data")] //These are internal variables that are automatically set. Useful for debug.
    [SerializeField] GameObject player;
    [SerializeField] GameObject pickedObject;
    [SerializeField] GameObject pickPosition;
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
        HandlePickUp();
    }

    void OnEnable() => inputs.Enable();

    void OnDisable() => inputs.Disable();
    #endregion

    #region Custom Methods
    void HandleMovement(Vector2 moveInp)
    {
        //Compact *if* for slower air movement
        Vector3 horVel = isGrounded ? (player.transform.right * moveInp.x + player.transform.forward * moveInp.y) * moveSpeed : (player.transform.right * moveInp.x + player.transform.forward * moveInp.y) * moveSpeed / airMoveDivider;
        //Controls Sprint
        bool sprinting = inputMovement.Sprint.activeControl != null;
        horVel = sprinting ? horVel *= sprintMultiplier : horVel;
        //Moves the player
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

    void HandleInteract()
    {
        //Uses a Raycast to Interact using the interface in the hit GameObject
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance)) if (hit.collider.TryGetComponent(out IInteractable interactable)) interactable.Interact(); else return;
    }

    void HandlePickUp()
    {
        if (inputMovement.Interact.triggered && pickedObject == null) HandleInteract();
        else if (inputMovement.Interact.triggered && pickedObject != null) TogglePickUp(pickedObject);
        if (pickedObject != null) HandlePickedObject();
    }

    public void TogglePickUp(GameObject go)
    {       
        //Toggles between picking and dropping the object.
        if(pickedObject == null)
        {
            pickedObject = go;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.useGravity = false;
            go.transform.parent = mainCamera.transform;
            go.transform.localPosition = new Vector3(0, 0, mainCamera.transform.localPosition.z + interactDistance);
            go.layer = 7;
        } 
        else
        {
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            pickedObject.transform.parent = null;
            pickedObject.layer = 0;
            pickedObject = null;
        }
    }

    void HandlePickedObject()
    {
        //Updates the picked object's position and drops it if it goes beyond some tolerance.
        Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        float distanceToPlayer = Vector3.Distance(pickedObject.transform.position, pickPosition.transform.position);
        if (distanceToPlayer >= 0.75f || xRot >= 60f) TogglePickUp(pickedObject);
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
        pickPosition = new GameObject("PickPosition");
        pickPosition.transform.parent = mainCamera.transform;
        pickPosition.transform.localPosition = new Vector3(0, 0, interactDistance);
        //Cursor SetUp
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    #endregion
}
