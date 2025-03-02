using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;
    
    [SerializeField] private float animationDuration = 1f; // Durée fixe de l'animation
    public bool IsAnimating => isAnimating; // Propriété publique pour vérifier l'état

    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
            audioManager.PlaySFX(audioManager.oldWoodDoorClose);
            animator.SetTrigger("Close");
        }
        else
        {
            audioManager.PlaySFX(audioManager.oldWoodDoorOpen);
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