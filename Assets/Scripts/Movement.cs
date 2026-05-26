// Movement.cs - Agrega propiedades públicas para que CameraLook lea el estado
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5.0f;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float currentSpeed;
    private bool isSprinting;

    // CameraLook las lee para calcular el balanceo
    public bool IsMoving   => moveInput.magnitude > 0.1f;
    public bool IsSprinting => isSprinting;
    public bool IsGrounded  => controller.isGrounded;

    void Start()
    {
        controller   = GetComponent<CharacterController>();
        currentSpeed = speed;
    }

    void Update()
    {
        ApplyMovement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
            verticalVelocity = jumpForce;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting  = true;
            currentSpeed = speed * sprintMultiplier;
        }
        else if (context.canceled)
        {
            isSprinting  = false;
            currentSpeed = speed;
        }
    }

    private void ApplyMovement()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.y = verticalVelocity;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}