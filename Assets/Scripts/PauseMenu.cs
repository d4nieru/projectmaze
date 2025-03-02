using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode pauseGameKey = KeyCode.Escape;
    [SerializeField] private PlayerController playerController;  // Référence publique au PlayerController
    [SerializeField] private AudioManager audioManager;  // Référence à AudioManager

    private bool isPaused = false;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned in the Inspector!");
        }
        
        if (audioManager == null)
        {
            audioManager = Object.FindFirstObjectByType<AudioManager>();
            
            if (audioManager == null)
            {
                Debug.LogError("AudioManager is not found in the scene!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseGameKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Désactiver les mouvements du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Mute tous les sons
        if (audioManager != null)
        {
            audioManager.MuteAllSounds();
        }

        // Afficher le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Réactiver les mouvements du joueur
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Unmute tous les sons
        if (audioManager != null)
        {
            audioManager.UnmuteAllSounds();
        }

        // Cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
