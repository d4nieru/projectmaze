using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] private EndMenu endMenu;

    AudioManager audioManager;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (endMenu != null)
            {
                audioManager.StopAllAndPlay(audioManager.LevelCompleted);
                endMenu.showEndMenu();
            }
            else
            {
                Debug.LogError("EndMenu non assigné dans l'inspecteur !");
            }
        }
    }
}
