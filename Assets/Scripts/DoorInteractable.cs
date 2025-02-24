using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;
    
    [SerializeField] private float animationDuration = 1f; // Durée fixe de l'animation
    public bool IsAnimating => isAnimating; // Propriété publique pour vérifier l'état

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool Interact()
    {
        if (isAnimating || animator == null) return false;
        
        isAnimating = true;
        
        if (isOpen)
        {
            animator.SetTrigger("Close");
        }
        else
        {
            animator.SetTrigger("Open");
        }
        
        isOpen = !isOpen;
        Invoke(nameof(ResetInteraction), animationDuration);
        return true;
    }

    private void ResetInteraction()
    {
        isAnimating = false;
    }

    public float GetAnimationDuration() => animationDuration;
}