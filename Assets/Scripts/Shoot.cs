using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CameraRecoil cameraRecoil;
    [SerializeField] private Camera playerCamera;

    [Header("Rifle de cerrojo")]
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float range = 100f;

    [Header("Munición")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadTime = 2.5f;

    [Header("Sonido")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptySound;

    [Header("Daño")]
    [SerializeField] private float damageAmount = 75f;
    [SerializeField] private float headshotMultiplier = 2f;

    [Header("Impacto")]
    [SerializeField] private GameObject impactParticlePrefab;
    [SerializeField] private LayerMask hitLayers;

    [Header("Partículas")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform muzzlePoint;  // Arrastra el Empty aquí

    private float nextFireTime = 0f;
    private bool triggerPressed = false;
    private Ray ray;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading) return;

        if (triggerPressed && Time.time >= nextFireTime)
            Fire();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) triggerPressed = true;
        else if (context.canceled) triggerPressed = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentAmmo < magazineSize)
            StartCoroutine(Reload());
    }

    private void Fire()
    {
        if (currentAmmo <= 0)
        {
            if (audioSource != null && emptySound != null)
                audioSource.PlayOneShot(emptySound);
            return;
        }

        nextFireTime = Time.time + fireRate;
        currentAmmo--;

        if (cameraRecoil != null) cameraRecoil.ApplyRecoil();
        if (audioSource != null && shootSound != null) audioSource.PlayOneShot(shootSound);
        if (muzzleFlash != null && muzzlePoint != null)
        {
            muzzleFlash.transform.position = muzzlePoint.position;
            muzzleFlash.transform.rotation = muzzlePoint.rotation;
            muzzleFlash.Play();
        }

        ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
            HandleHit(hit);
    }

    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (audioSource != null && reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }

    private void HandleHit(RaycastHit hit)
    {
        float damage = damageAmount;

        if (hit.collider.name == "Head")
            damage *= headshotMultiplier;

        Health targetHealth = hit.collider.GetComponent<Health>();

        if (targetHealth == null)
            targetHealth = hit.collider.GetComponentInParent<Health>();

        if (targetHealth != null)
            targetHealth.TakeDamage(damage, ray.direction);

        if (impactParticlePrefab != null)
            Instantiate(impactParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public int CurrentAmmo => currentAmmo;
    public int MagazineSize => magazineSize;
    public bool IsReloading => isReloading;

    void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, hit.point);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 0.5f);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * range);
        }
    }
}