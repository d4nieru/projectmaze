using UnityEngine;

public class Gem : MonoBehaviour
{
    private bool isCollected = false; // Empêche la collecte multiple
    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            isCollected = true;
            playerInventory.CollectGem();  // Ajoute une gemme
            Destroy(gameObject);  // Détruit la gemme
        }
    }
}
