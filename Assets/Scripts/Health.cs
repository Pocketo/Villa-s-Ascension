// Health.cs - Va en cualquier objeto que pueda recibir daño (enemigos, jugador, etc.)
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public UnityEvent onDeath;   // Arrastra aquí lo que pase al morir desde el Inspector

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }
}