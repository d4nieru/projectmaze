using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false; // Empêche l'interaction pendant l'animation

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (isAnimating || animator == null) return; // Bloque l’interaction pendant l'animation

        isAnimating = true; // Bloque les interactions pendant l'animation

        if (isOpen)
        {
            animator.ResetTrigger("Open");
            animator.SetTrigger("Close");
        }
        else
        {
            animator.ResetTrigger("Close");
            animator.SetTrigger("Open");
        }

        isOpen = !isOpen;

        // Utilisation de la durée de l'animation pour synchroniser l'interaction
        float animationTime = GetAnimationLength();
        Invoke(nameof(ResetInteraction), animationTime); // Attente de la fin de l'animation
    }

    private void ResetInteraction()
    {
        isAnimating = false; // Réactive l'interaction après l'animation
    }

    // Récupère la durée de l'animation en cours
    public float GetAnimationLength()
    {
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length; // Durée de l'animation en cours
        }

        return 0f;
    }

    // Vérifie si l'animation est terminée pour autoriser une nouvelle interaction
    void Update()
    {
        // Vérifie si l'animation est terminée
        if (isAnimating)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1f) // L'animation est terminée
            {
                isAnimating = false; // Permet une nouvelle interaction
            }
        }
    }
}