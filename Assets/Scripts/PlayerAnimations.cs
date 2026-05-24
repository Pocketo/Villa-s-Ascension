// PlayerAnimator.cs - Va en el Player junto a Movement.cs y Shooting.cs
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;
    [SerializeField] private Shooting shooting;

    // Parámetros del Animator Controller — deben coincidir exactamente
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsShooting = Animator.StringToHash("IsShooting");
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (movement == null) movement = GetComponent<Movement>();
        if (shooting == null) shooting = GetComponent<Shooting>();
    }

    void Update()
    {
        animator.SetBool(IsWalking, movement.IsMoving && !movement.IsSprinting);
        animator.SetBool(IsRunning, movement.IsSprinting);
        animator.SetBool(IsReloading, shooting.IsReloading);
    }

    // Llamado desde Shooting.cs al disparar
    public void TriggerShoot()
    {
        animator.SetTrigger(IsShooting);
    }
}