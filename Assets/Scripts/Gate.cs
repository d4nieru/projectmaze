using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject playerObject; // À assigner dans l'Inspector
    private PlayerInventory playerInventory;
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Vérifie si un joueur a été assigné
        if (playerObject != null)
        {
            playerInventory = playerObject.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogError("AutoDoor : Aucun joueur assigné dans l'Inspector !");
            return;
        }

        // Vérifie si PlayerInventory existe sur l'objet assigné
        if (playerInventory != null)
        {
            playerInventory.OnAllCollected.AddListener(OpenDoor);
        }
        else
        {
            Debug.LogError("AutoDoor : L'objet assigné ne possède pas de PlayerInventory !");
        }
    }

    private void OpenDoor()
    {
        if (!isOpen) // Vérifie que la porte est fermée avant de l'ouvrir
        {
            animator.SetTrigger("Open");
            isOpen = true; // Empêche d'ouvrir plusieurs fois
        }
    }
}
