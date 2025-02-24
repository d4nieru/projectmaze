using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float objectInteractDistance = 3f;  // Distance pour les objets
    [SerializeField] private float doorInteractDistance = 5f;    // Distance pour les portes
    [SerializeField] private KeyCode interactKey = KeyCode.E;    // Touche d'interaction

    private Camera playerCamera;
    private bool canInteract = true; // Cooldown d'interaction

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && canInteract)
        {
            canInteract = false; // Bloque temporairement l'interaction

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, doorInteractDistance)) // Vérifie d'abord les portes
            {
                DoorInteractable door = hit.collider.GetComponent<DoorInteractable>();
                if (door != null)
                {
                    door.Interact();
                    // Utilise la durée de l'animation de la porte pour éviter de trop vite interagir à nouveau
                    float animationTime = door.GetAnimationLength();
                    Invoke(nameof(ResetInteraction), animationTime); // Réactive l'interaction après la fin de l'animation
                    return; // Empêche de tester les objets si une porte a été trouvée
                }
            }

            if (Physics.Raycast(ray, out hit, objectInteractDistance)) // Vérifie ensuite les objets
            {
                ItemInteractable item = hit.collider.GetComponent<ItemInteractable>();
                if (item != null)
                {
                    item.Interact();
                    Invoke(nameof(ResetInteraction), 0.5f); // Temps fixe pour éviter les spams
                }
            }
        }
    }

    private void ResetInteraction()
    {
        canInteract = true; // Réactive l'interaction après un délai
    }
}