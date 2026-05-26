// CameraLook.cs - Añade balanceo al caminar y efecto de sprint
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float verticalClamp    = 80f;
    // CameraLook.cs - Agrega referencia a CameraRecoil
    [SerializeField] private CameraRecoil cameraRecoil;  // Arrastra la Main Camera aquí

    [Header("Balanceo al caminar")]
    [SerializeField] private float bobFrequency     = 7f;    // Qué tan rápido oscila
    [SerializeField] private float bobAmplitudeY    = 0.05f; // Altura del balanceo
    [SerializeField] private float bobAmplitudeX    = 0.025f;// Balanceo lateral
    [SerializeField] private float bobSmoothing     = 10f;   // Suavizado de transición

    [Header("Efecto sprint")]
    [SerializeField] private float sprintFOV        = 75f;   // FOV al correr
    [SerializeField] private float normalFOV        = 60f;
    [SerializeField] private float fovSmoothing     = 8f;
    [SerializeField] private float sprintTiltAngle  = 3f;    // Leve inclinación al correr
    [SerializeField] private float tiltSmoothing    = 6f;

    private Transform playerBody;
    private Movement movement;
    private Camera cam;

    private Vector2 lookInput;
    private float   cameraPitch = 0f;

    // Balanceo
    private float   bobTimer    = 0f;
    private Vector3 bobOffset   = Vector3.zero;
    private Vector3 targetBob   = Vector3.zero;

    // Sprint tilt
    private float currentTilt   = 0f;

    void Start()
    {
        playerBody = transform.parent;
        movement   = playerBody.GetComponent<Movement>();
        cam        = GetComponent<Camera>();

        cam.fieldOfView = normalFOV;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void Update()
    {
        ApplyLook();
        ApplyBob();
        ApplySprintEffects();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // CameraLook.cs - ApplyLook corregido
    private void ApplyLook()
    {
        float yaw = lookInput.x * mouseSensitivity;
        playerBody.Rotate(Vector3.up, yaw);

        cameraPitch -= lookInput.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalClamp, verticalClamp);

        // Incluye el recoil en la misma rotación en lugar de aplicarlo por separado
        Vector3 recoilOffset = cameraRecoil != null ? cameraRecoil.CurrentRecoil : Vector3.zero;

        transform.localRotation = Quaternion.Euler(
            cameraPitch + recoilOffset.x,
            recoilOffset.y,
            currentTilt + recoilOffset.z
        );
    }

    private void ApplyBob()
    {
        if (movement == null) return;

        if (movement.IsMoving && movement.IsGrounded)
        {
            float frequency = movement.IsSprinting ? bobFrequency * 1.4f : bobFrequency;

            bobTimer += Time.deltaTime * frequency;

            // Sin() para Y, Cos() para X → figura de 8 suave
            targetBob = new Vector3(
                Mathf.Cos(bobTimer * 0.5f) * bobAmplitudeX,
                Mathf.Sin(bobTimer)        * bobAmplitudeY,
                0f
            );
        }
        else
        {
            // Vuelve al centro suavemente cuando para
            bobTimer  = 0f;
            targetBob = Vector3.zero;
        }

        bobOffset = Vector3.Lerp(bobOffset, targetBob, Time.deltaTime * bobSmoothing);
        transform.localPosition = bobOffset;
    }

    private void ApplySprintEffects()
    {
        if (movement == null) return;

        // FOV
        float targetFOV = movement.IsSprinting ? sprintFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmoothing);

        // Tilt lateral al correr
        float targetTilt = movement.IsSprinting ? -sprintTiltAngle : 0f;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSmoothing);
    }
}