// Health.cs - El mismo script genérico, ahora notifica al RagdollController
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public UnityEvent onDeath;

    private RagdollController ragdoll;

    void Start()
    {
        currentHealth = maxHealth;
        ragdoll = GetComponent<RagdollController>();
    }

    public void TakeDamage(float amount, Vector3 hitDirection = default)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        if (currentHealth <= 0f)
            Die(hitDirection);
    }

    private void Die(Vector3 hitDirection)
    {
        onDeath?.Invoke();

        if (ragdoll != null)
            ragdoll.EnableRagdoll(hitDirection);

        // Destruye después de que el ragdoll caiga
        Destroy(gameObject, 5f);
    }

    public float GetHealthPercent() => currentHealth / maxHealth;
}