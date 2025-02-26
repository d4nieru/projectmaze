using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] private EndMenu endMenu;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (endMenu != null)
            {
                endMenu.showEndMenu();
            }
            else
            {
                Debug.LogError("EndMenu non assigné dans l'inspecteur !");
            }
        }
    }
}
