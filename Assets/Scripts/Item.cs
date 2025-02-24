using UnityEngine;

public class Item : MonoBehaviour
{
    private void PickUp()
    {
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.CollectItem();  // Ajoute un item
            Destroy(gameObject);  // Supprime l’objet après ramassage
        }
    }
}
