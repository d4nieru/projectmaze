using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject gameOverMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerController == null)
        {
            Debug.Log("PlayerController is not assigned in the Inspector!");
        }

        if (gameOverMenu == null)
        {
            Debug.Log("Game Over Menu is not assigned in the Inspector!");
        }
    }

    public void showGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;

        // Désactiver les mouvements du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Afficher le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TryAgain()
    {
        // tryAgainMenu.SetActive(false);
        // Time.timeScale = 1f;
        // isDead = false;

        // // Réactiver les mouvements du joueur
        // if (playerController != null)
        // {
        //     playerController.enabled = true;
        // }

        // // Cacher le curseur
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
