using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode pauseGameKey = KeyCode.Escape;
    [SerializeField] private PlayerController playerController;  // Référence publique au PlayerController

    private bool isPaused = false;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned in the Inspector!");
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
