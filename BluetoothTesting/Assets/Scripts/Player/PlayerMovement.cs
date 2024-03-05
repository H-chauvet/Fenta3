using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 5.0f;
    public float sensitivity = 2.0f;
    public float jumpForce = 8.0f;

    [SerializeField] private Light spotLight;
    [SerializeField]private float gravity = 9.8f;
    private Camera playerCamera;
    private CharacterController characterController;
    private float rotationX = 0;
    private float horizontalJoysticks = 0;

    private float verticalJoysticks = 0;
    private float horizontalLook = 0;
    private float verticalLook = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMouseLookJoysticks();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleMovementJoysticks();
        HandleJump();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 move = transform.TransformDirection(moveDirection) * walkSpeed;

        move.y = characterController.velocity.y - gravity;
        
        characterController.Move(move * Time.deltaTime);
    }

    public void SetMovementsDirection(float horizontal, float vertical)
    {
        horizontalJoysticks = horizontal;
        verticalJoysticks = vertical;
    }

    public void SetLookDirection(float horizontal, float vertical)
    {
        horizontalLook = horizontal;
        verticalLook = vertical;
    }

    void HandleMovementJoysticks()
    {
        float horizontal = horizontalJoysticks;
        float vertical = verticalJoysticks;

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 move = transform.TransformDirection(moveDirection) * walkSpeed;

        move.y = characterController.velocity.y - gravity;
        
        characterController.Move(move * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        spotLight.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }

    void HandleMouseLookJoysticks()
    {
        float mouseX = horizontalLook;
        float mouseY = verticalLook;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        spotLight.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }

    void HandleJump()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                
                characterController.Move(Vector3.up * jumpForce);
            }
        }
    }
}


