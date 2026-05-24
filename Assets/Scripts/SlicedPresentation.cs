// SlideshowPresentation.cs - Va en un GameObject vacío en la escena
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SlideshowPresentation : MonoBehaviour
{
    [Header("Imágenes")]
    [SerializeField] private Sprite[] slides;
    [SerializeField] private float slideDuration = 4f;   // Cuánto dura cada imagen

    [Header("Transición")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float zoomAmount = 1.15f;  // Zoom máximo (1 = sin zoom)
    [SerializeField] private float zoomSpeed = 0.5f;

    [Header("Música")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.8f;

    [Header("Referencias UI")]
    [SerializeField] private Image slideImage;       // Image del Canvas
    [SerializeField] private Image fadeOverlay;      // Image negra encima de todo
    [SerializeField] private GameObject endScreen;      // Panel al terminar, opcional

    [Header("Escena")]
    [SerializeField] private string nextSceneName;  // Nombre exacto de la escena

    private int currentSlide = 0;
    private bool isRunning = false;

    void Start()
    {
        if (slides.Length == 0)
        {
            Debug.LogWarning("No hay slides asignados.");
            return;
        }

        if (endScreen != null) endScreen.SetActive(false);

        SetupMusic();
        StartCoroutine(RunSlideshow());
    }

    private void SetupMusic()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
    }

    private IEnumerator RunSlideshow()
    {
        isRunning = true;

        for (int i = 0; i < slides.Length; i++)
        {
            currentSlide = i;
            yield return StartCoroutine(ShowSlide(slides[i]));
        }

        // Al final de RunSlideshow() reemplaza lo que tenías por esto
        yield return StartCoroutine(FadeOut());

        if (audioSource != null)
            StartCoroutine(FadeOutMusic());

        // Espera a que la música baje antes de cambiar de escena
        yield return new WaitForSeconds(2.5f);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator ShowSlide(Sprite sprite)
    {
        // Asigna la imagen y resetea escala
        slideImage.sprite = sprite;
        slideImage.color = new Color(1f, 1f, 1f, 0f);
        slideImage.transform.localScale = Vector3.one;

        // Fade in
        yield return StartCoroutine(FadeImage(slideImage, 0f, 1f, fadeDuration));

        // Zoom in + espera mientras dura el slide
        float timer = 0f;
        while (timer < slideDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / slideDuration;

            // Zoom suave con Lerp
            float scale = Mathf.Lerp(1f, zoomAmount, progress * zoomSpeed);
            slideImage.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        // Fade out
        yield return StartCoroutine(FadeImage(slideImage, 1f, 0f, fadeDuration));
    }

    private IEnumerator FadeImage(Image image, float from, float to, float duration)
    {
        float timer = 0f;
        Color color = image.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, timer / duration);
            image.color = color;
            yield return null;
        }

        color.a = to;
        image.color = color;
    }

    private IEnumerator FadeOut()
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(fadeOverlay, 0f, 1f, fadeDuration * 1.5f));
    }

    private IEnumerator FadeOutMusic()
    {
        float duration = 2f;
        float timer = 0f;
        float startVol = audioSource.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, timer / duration);
            yield return null;
        }

        audioSource.Stop();
    }
}