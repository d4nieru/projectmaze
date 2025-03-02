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
    [SerializeField] private float lostTime = 5f;

    [Header("Alerte")]
    [SerializeField] private GameObject alertQuad;
    public float alertDisplayTime = 2f;

    private NavMeshAgent agent;
    private bool isChasing = false;
    private float lostTimer = 0f;

    [SerializeField] private GameOverMenu gameOverMenu;
    private AudioManager audioManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Empêche le NavMeshAgent de gérer la rotation
        currentTarget = pointA;
        GoToNextPatrolPoint();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (alertQuad == null)
        {
            Debug.LogError("alertQuad n'est pas assigné !");
        }
        else
        {
            alertQuad.SetActive(false);
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

            // Faire tourner l'ennemi vers l'opposé du prochain point
            Vector3 direction = (currentTarget == pointA) ? (pointA.position - pointB.position) : (pointB.position - pointA.position);
            FaceDirection(direction);
        }
    }

    void ResumePatrol()
    {
        isWaiting = false;
        agent.isStopped = false;
        currentTarget = (currentTarget == pointA) ? pointB : pointA;

        // Regarder vers la prochaine destination
        Vector3 direction = currentTarget.position - transform.position;
        FaceDirection(direction);

        GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
    }

    void FaceDirection(Vector3 direction)
    {
        direction.y = 0; // Évite de modifier l'axe vertical
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    void DetectPlayer()
    {
        if (player == null || isChasing) return;

        Vector3 enemyToPlayer = player.position - transform.position;
        float distance = enemyToPlayer.magnitude;

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
        return dotProduct > 0 && dotProduct < 1;
    }

    void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            alertQuad.SetActive(true);
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
        alertQuad.SetActive(false);
    }

    void BeginChase()
    {
        agent.isStopped = false;
        agent.speed *= 1.2f;
        audioManager.FadeInMusic(audioManager.enemyChasingPlayer, 2f);
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
                audioManager.FadeOutMusic(4f);
                audioManager.FadeInMusic(audioManager.backgroundMusic, 4f);
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
            audioManager.StopAllAndPlay(audioManager.gameOver);
            gameOverMenu.showGameOverMenu();
            agent.isStopped = true;
        }
    }
}