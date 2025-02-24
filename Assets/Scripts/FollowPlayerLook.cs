using UnityEngine;

public class FollowPlayerLook : MonoBehaviour
{
    private Camera playerCamera; // La caméra du joueur

    void Start()
    {
        // Trouve automatiquement la caméra principale du joueur
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("La caméra principale (MainCamera) n'a pas été trouvée !");
        }
    }

    void Update()
    {
        if (playerCamera != null)
        {
            // Regarde toujours la caméra en maintenant sa position actuelle
            Vector3 lookDirection = playerCamera.transform.position - transform.position;
            lookDirection.y = 0; // Empêche la rotation sur l'axe Y (évite de basculer l'objet)

            transform.rotation = Quaternion.LookRotation(lookDirection); // Fait tourner l'objet pour faire face à la caméra
        }
    }
}
