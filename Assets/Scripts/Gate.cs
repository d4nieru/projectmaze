using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private FinalDoorCinematic finalDoorCinematic;
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (finalDoorCinematic != null)
        {
            finalDoorCinematic.OnAllCollected.AddListener(OpenDoor);
        }
        else
        {
            Debug.LogError("Gate : FinalDoorCinematic n'est pas assigné !");
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
