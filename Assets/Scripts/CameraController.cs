using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float verticalClamp = 80f;

    private Transform playerBody;
    private Vector2 lookInput;
    private float cameraPitch = 0f;

    void Start()
    {
        // La cámara es hija del Player, entonces su padre es el cuerpo
        playerBody = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ApplyLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void ApplyLook()
    {
        // Rota el cuerpo horizontalmente
        float yaw = lookInput.x * mouseSensitivity;
        playerBody.Rotate(Vector3.up, yaw);

        // Rota solo la cámara verticalmente
        cameraPitch -= lookInput.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
}