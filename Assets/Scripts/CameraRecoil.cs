// CameraRecoil.cs - Script separado, va en la Main Camera junto a CameraLook
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Retroceso")]
    [SerializeField] private float recoilUp      = 2f;    // Cuánto sube la cámara
    [SerializeField] private float recoilSide    = 0.5f;  // Variación lateral aleatoria
    [SerializeField] private float recoilRoll    = 1f;    // Leve rotación en Z

    [Header("Velocidades")]
    [SerializeField] private float kickSpeed     = 30f;   // Qué tan rápido sube al disparar
    [SerializeField] private float returnSpeed   = 5f;    // Qué tan lento vuelve al centro

    private Vector3 currentRecoil  = Vector3.zero;  // Valor actual interpolado
    private Vector3 targetRecoil   = Vector3.zero;  // Hacia dónde va el recoil

    void Update()
    {
        // Decae el target hacia cero con el tiempo
        targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, Time.deltaTime * returnSpeed);

        // El recoil actual sigue al target rápido al subir, lento al bajar
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * kickSpeed);

        // Se aplica encima de la rotación base de CameraLook
        transform.localRotation *= Quaternion.Euler(currentRecoil);
    }

    // Llamado desde Shooting.cs al disparar
    public void ApplyRecoil()
    {
        float side = Random.Range(-recoilSide, recoilSide);
        float roll = Random.Range(-recoilRoll, recoilRoll);

        // Acumula en lugar de reemplazar → ráfagas se acumulan naturalmente
        targetRecoil += new Vector3(-recoilUp, side, roll);
    }
}