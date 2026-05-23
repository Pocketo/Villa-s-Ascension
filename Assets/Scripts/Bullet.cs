// Bullet.cs - Va en el prefab de la bala
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float maxLifetime = 3f;    // Se destruye sola si no golpea nada

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, maxLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Intenta aplicar daño al objeto golpeado
        BulletDamage damage = GetComponent<BulletDamage>();
        Health targetHealth = collision.gameObject.GetComponent<Health>();

        if (damage != null && targetHealth != null)
            targetHealth.TakeDamage(damage.DamageAmount);

        Destroy(gameObject);
    }
}