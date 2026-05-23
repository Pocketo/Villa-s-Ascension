// PlayerHealth.cs - Va en el Player
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float respawnDelay = 2f;

    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log($"Jugador recibió {amount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Jugador muerto. Reiniciando nivel...");
        Invoke(nameof(ReloadScene), respawnDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Para conectar con UI si quieres mostrar la vida
    public float GetHealthPercent() => currentHealth / maxHealth;
}