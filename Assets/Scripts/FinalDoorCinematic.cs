using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class FinalDoorCinematic : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject cinematicCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameObject endLevelTrigger;
    public UnityEvent OnAllCollected;

    public void PlayCinematic()
    {
        StartCoroutine(CinematicSequence());
    }

    private IEnumerator CinematicSequence()
    {
        // Désactiver le contrôle du joueur
        playerController.enabled = false;

        // Passer à la caméra cinématique
        playerCamera.SetActive(false);
        cinematicCamera.SetActive(true);

        // Attendre x secondes
        yield return new WaitForSeconds(0.5f);

        // Déclenche l’événement pour ouvrir la porte
        OnAllCollected.Invoke();

        if (endLevelTrigger != null)
        {
            endLevelTrigger.SetActive(true);
            Debug.Log("Trigger de fin activé !");
        }
        else
        {
            Debug.LogError("EndLevelTrigger non assigné dans l'inspecteur !");
        }

        // Attendre encore x secondes après l'animation
        yield return new WaitForSeconds(6f);

        // Revenir à la caméra du joueur
        cinematicCamera.SetActive(false);
        playerCamera.SetActive(true);

        // Réactiver le contrôle du joueur
        playerController.enabled = true;
    }
}
