using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float objectInteractDistance = 3f;
    [SerializeField] private float doorInteractDistance = 5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    private Camera playerCamera;
    private bool canInteract = true;
    private DoorInteractable currentDoor; // Pour suivre la dernière porte

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && canInteract)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            
            // Vérifie d'abord les portes
            if (Physics.Raycast(ray, out RaycastHit hit, doorInteractDistance))
            {
                DoorInteractable door = hit.collider.GetComponent<DoorInteractable>();
                if (door != null)
                {
                    if (door.Interact()) // Si l'interaction réussit
                    {
                        canInteract = false;
                        currentDoor = door;
                        Invoke(nameof(ResetInteraction), door.GetAnimationDuration());
                        return;
                    }
                }
            }

            // Vérification des objets
            if (Physics.Raycast(ray, out hit, objectInteractDistance))
            {
                ItemInteractable item = hit.collider.GetComponent<ItemInteractable>();
                if (item != null)
                {
                    item.Interact();
                    canInteract = false;
                    Invoke(nameof(ResetInteraction), 0.5f);
                }
            }
        }
    }

    private void ResetInteraction()
    {
        canInteract = true;
        currentDoor = null;
    }
}