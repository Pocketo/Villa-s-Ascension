// CameraRecoil.cs - Ya no toca transform.localRotation
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [Header("Retroceso")]
    [SerializeField] private float recoilUp = 2f;
    [SerializeField] private float recoilSide = 0.5f;
    [SerializeField] private float recoilRoll = 1f;

    [Header("Velocidades")]
    [SerializeField] private float kickSpeed = 30f;
    [SerializeField] private float returnSpeed = 5f;

    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 targetRecoil = Vector3.zero;

    // CameraLook lee esto para incluirlo en su propia rotación
    public Vector3 CurrentRecoil => currentRecoil;

    void Update()
    {
        targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * kickSpeed);
    }

    public void ApplyRecoil()
    {
        float side = Random.Range(-recoilSide, recoilSide);
        float roll = Random.Range(-recoilRoll, recoilRoll);
        targetRecoil += new Vector3(-recoilUp, side, roll);
    }
}