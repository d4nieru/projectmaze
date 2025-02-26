using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrouille")]
    public Transform pointA, pointB;
    private Transform currentTarget;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    [Header("Détection du joueur")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float chaseDistance = 15f;
    [SerializeField] private float lostTime = 5f; // Temps avant de retourner à la patrouille

    [Header("Alerte")]
    [SerializeField] private GameObject alertQuad; // GameObject pour l'alerte
    public float alertDisplayTime = 2f;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private float lostTimer = 0f;

    [SerializeField] private GameOverMenu gameOverMenu;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = pointA;
        GoToNextPatrolPoint();

        if (alertQuad == null)
        {
            Debug.LogError("alertQuad n'est pas assigné !");
        }
        else
        {
            alertQuad.SetActive(false); // Cache l'alerte au début
        }
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                ResumePatrol();
            }
        }
        else
        {
            if (isChasing)
            {
                ChasePlayer();
            }
            else
            {
                Patrolling();
                DetectPlayer();
            }
        }

        Debug.DrawLine(pointA.position, pointB.position, Color.red);
    }

    void Patrolling()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            agent.isStopped = true;
            isWaiting = true;
            waitTimer = 2f;
        }
    }

    void ResumePatrol()
    {
        isWaiting = false;
        agent.isStopped = false;
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
    }

    void DetectPlayer()
    {
        if (player == null || isChasing) return;

        Vector3 enemyToPlayer = player.position - transform.position;
        float distance = enemyToPlayer.magnitude;

        // Vérifier si le joueur est bien entre A et B
        if (IsPlayerBetweenPoints(player.position) && distance <= detectionRange)
        {
            Debug.Log("Joueur détecté !");
            StartChase();
        }
    }

    bool IsPlayerBetweenPoints(Vector3 playerPosition)
    {
        Vector3 AtoB = pointB.position - pointA.position;
        Vector3 AtoPlayer = playerPosition - pointA.position;

        float dotProduct = Vector3.Dot(AtoPlayer.normalized, AtoB.normalized);
        return dotProduct > 0 && dotProduct < 1; // Le joueur est entre A et B
    }

    void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            alertQuad.SetActive(true); // Cache l'alerte au début

            // Place le sprite au-dessus de l'ennemi
            alertQuad.transform.position = transform.position + Vector3.up * 2f;
            alertQuad.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

            Debug.Log("Alerte affichée !");
            agent.isStopped = true;
            Invoke(nameof(HideAlert), alertDisplayTime);
            Invoke(nameof(BeginChase), alertDisplayTime);
        }
    }

    void HideAlert()
    {
        alertQuad.SetActive(false); // Cache l'alerte
    }

    void BeginChase()
    {
        agent.isStopped = false;
        agent.speed *= 1.5f;
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > chaseDistance)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= lostTime)
            {
                Debug.Log("Le joueur a fui, retour à la patrouille !");
                isChasing = false;
                agent.speed /= 1.5f;
                lostTimer = 0f;
                GoToNextPatrolPoint();
            }
        }
        else
        {
            lostTimer = 0f;
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Le joueur a été attrapé !");
            gameOverMenu.showGameOverMenu();
            agent.isStopped = true;
        }
    }
}
