using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private float maxLifetime = 3f;

    private Rigidbody rb;
    private BulletDamage bulletDamage;

    void Start()
    {
        rb          = GetComponent<Rigidbody>();
        bulletDamage = GetComponent<BulletDamage>();

        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, maxLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null && bulletDamage != null)
        {
            // Lee el daño desde BulletDamage y pasa la dirección del impacto
            targetHealth.TakeDamage(bulletDamage.DamageAmount, transform.forward);
        }
        Destroy(gameObject);
    }
}