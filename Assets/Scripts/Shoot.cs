// Shooting.cs - Va en el Player o en la cámara
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;        // Empty GameObject al frente de la cámara
    [SerializeField] private GameObject bulletPrefab;

    [Header("Rifle de cerrojo")]
    [SerializeField] private float fireRate = 1.5f;     // Segundos entre disparos

    private float nextFireTime = 0f;
    private bool triggerPressed = false;

    void Update()
    {
        if (triggerPressed && Time.time >= nextFireTime)
            Fire();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            triggerPressed = true;
        else if (context.canceled)
            triggerPressed = false;
    }

    private void Fire()
    {
        nextFireTime = Time.time + fireRate;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}