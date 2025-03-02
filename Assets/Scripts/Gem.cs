using UnityEngine;

public class Gem : MonoBehaviour
{
    private bool isCollected = false; // Empêche la collecte multiple

    AudioManager audioManager;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            isCollected = true;
            playerInventory.CollectGem();  // Ajoute une gemme
            audioManager.PlaySFX(audioManager.collectGem);
            Destroy(gameObject);  // Détruit la gemme
        }
    }
}
