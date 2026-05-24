// EnemyBullet.cs - Va en el prefab de bala del enemigo (separado del de el jugador)
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float maxLifetime = 4f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, maxLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Solo daña al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}