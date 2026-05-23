// BulletDamage.cs - Va en el prefab de la bala junto a Bullet.cs
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private float damageAmount = 75f;      // Rifle de cerrojo → daño alto
    [SerializeField] private float headshotMultiplier = 2f; // Opcional

    public float DamageAmount => damageAmount;

    // Llama esto si tu enemigo tiene colliders separados por zona (cabeza, cuerpo)
    public float GetDamage(string hitZone)
    {
        return hitZone == "Head" ? damageAmount * headshotMultiplier : damageAmount;
    }
}