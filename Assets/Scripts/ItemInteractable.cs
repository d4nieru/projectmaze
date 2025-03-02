using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour
{
    public UnityEvent onInteract;  // Événement déclenché lors de l’interaction
    private bool isCollected = false; // Évite la collecte multiple
    private AudioManager audioManager;
    [SerializeField] private PlayerInventory playerInventory; // Assignable depuis Unity

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Interact()
    {
        if (isCollected || playerInventory == null) return; // Vérifie si l’objet est déjà ramassé

        isCollected = true;
        
        // Ajoute l'objet à l'inventaire
        playerInventory.CollectItem();
        
        // Déclenche l’événement (utile si un objet doit activer un mécanisme)
        onInteract.Invoke();

        // Joue le son
        audioManager.PlaySFX(audioManager.itemPickup);

        // Détruit l’objet après un court délai
        Destroy(gameObject, 0.2f);
    }
}
