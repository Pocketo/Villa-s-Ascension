// EnemyAim.cs - Va en el enemigo
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Configuración")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float aimSpeed = 5f;        // Qué tan rápido voltea hacia el jugador

    private float nextFireTime = 0f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > detectionRange) return;

        AimAtPlayer();

        if (Time.time >= nextFireTime)
            Fire();
    }

    private void AimAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;                                // Solo rota en horizontal, no se inclina

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        nextFireTime = Time.time + fireRate;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    // Visualizar el rango de detección en el Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}