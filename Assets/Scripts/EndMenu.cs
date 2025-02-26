using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject endMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerController == null)
        {
            Debug.Log("PlayerController is not assigned in the Inspector!");
        }

        if (endMenu == null)
        {
            Debug.Log("End Menu is not assigned in the Inspector!");
        }
    }

    public void showEndMenu()
    {
        endMenu.SetActive(true);
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

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
