using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour
{
    public UnityEvent onInteract;  // Événement pour l'interaction (ramasser un objet, ouvrir une porte, etc.)
    private bool isCollected = false; // Empêche le double appel

    public void Interact()
    {
        if (!isCollected)
        {
            isCollected = true;
            onInteract.Invoke(); // Déclenche l'événement de collecte
            Destroy(gameObject); // Supprime l’item après interaction
        }
    }
}
