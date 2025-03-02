using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI itemText;
    
    private int totalGems;
    private int totalItems;


    void Start()
    {
        // Compter les objets dès le début
        //totalGems = GameObject.FindGameObjectsWithTag("Gem").Length;
        totalItems = Object.FindObjectsByType<ItemInteractable>(FindObjectsSortMode.None).Length;
        totalGems = Object.FindObjectsByType<Gem>(FindObjectsSortMode.None).Length;

        // Trouver l'inventaire du joueur
        PlayerInventory playerInventory = Object.FindFirstObjectByType<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.OnGemCollected.RemoveAllListeners(); // Évite le double comptage
            playerInventory.OnItemCollected.RemoveAllListeners();

            // Écouter les événements de collecte
            playerInventory.OnGemCollected.AddListener(UpdateGemText);
            playerInventory.OnItemCollected.AddListener(UpdateItemText);
        }

        // Initialiser l'affichage
        UpdateGemText(playerInventory);
        UpdateItemText(playerInventory);
    }

    private void UpdateGemText(PlayerInventory playerInventory)
    {
        gemText.text = playerInventory.numberOfGems + " / " + totalGems;
    }

    private void UpdateItemText(PlayerInventory playerInventory)
    {
        itemText.text = playerInventory.numberOfItems + " / " + totalItems;
    }
}
