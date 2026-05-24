// ExplosionTrigger.cs - Va en el objeto con el Trigger Collider
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [Header("Efectos")]
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    [Header("Configuración")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool repeateable = true;     // Si puede activarse más de una vez
    [SerializeField] private float cooldown = 2f;      // Tiempo entre activaciones

    private bool isOnCooldown = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (isOnCooldown) return;

        Trigger();
    }

    private void Trigger()
    {
        if (explosionParticles != null)
            explosionParticles.Play();

        if (audioSource != null && explosionSound != null)
            audioSource.PlayOneShot(explosionSound);

        if (repeateable)
            StartCoroutine(Cooldown());
        else
            GetComponent<Collider>().enabled = false;  // Se desactiva para siempre
    }

    private System.Collections.IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}