// MainMenu.cs - Va en un GameObject del menú
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Escena")]
    [SerializeField] private string gameSceneName;  // Nombre exacto en Build Settings

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        // Solo funciona en build, en el Editor usa esto:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}