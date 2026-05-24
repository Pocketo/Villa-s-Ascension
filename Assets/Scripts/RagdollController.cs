// RagdollController.cs
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody hipsRigidbody;   // Arrastra el hueso Hips directamente

    [Header("Fuerza de impacto")]
    [SerializeField] private float minForce = 3f;
    [SerializeField] private float maxForce = 8f;
    [SerializeField] private float upwardForce = 2f;    // Pequeño empuje hacia arriba
    [SerializeField] private float torqueForce = 4f;    // Rotación al caer

    private Rigidbody[] ragdollBodies;
    private Collider[]  ragdollColliders;

    void Awake()
    {
        ragdollBodies    = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void EnableRagdoll(Vector3 hitDirection)
    {
        if (characterController != null) characterController.enabled = false;
        if (animator != null)           animator.enabled = false;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
        }

        foreach (Collider col in ragdollColliders)
            col.enabled = true;

        ApplyDeathImpulse(hitDirection);
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
            rb.isKinematic = true;

        foreach (Collider col in ragdollColliders)
            col.enabled = false;

        if (characterController != null) characterController.enabled = true;
        if (animator != null)           animator.enabled = true;
    }

    private void ApplyDeathImpulse(Vector3 hitDirection)
    {
        if (hipsRigidbody == null)
        {
            Debug.LogWarning("Arrastra el hueso Hips al campo hipsRigidbody en el Inspector.");
            return;
        }

        // Fuerza aleatoria dentro de un rango para que no sea idéntica siempre
        float randomForce = Random.Range(minForce, maxForce);

        // Dirección base del disparo + componente vertical + desviación lateral aleatoria
        Vector3 lateralVariation = Random.insideUnitSphere * 0.4f;
        lateralVariation.y = 0f;

        Vector3 finalDirection = (hitDirection + lateralVariation).normalized;
        finalDirection.y += upwardForce;

        hipsRigidbody.AddForce(finalDirection * randomForce, ForceMode.Impulse);

        // Torque aleatorio para que ruede distinto cada vez
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * torqueForce;

        hipsRigidbody.AddTorque(randomTorque, ForceMode.Impulse);
    }
}