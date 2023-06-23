using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : EnemyState
{
    [Header("Navigation")]
    [SerializeField] private float[] patrolPoints;
    [SerializeField] private float detectRange = 2f;
    [SerializeField] private float movementDelay = 5f;

    [Header("Chase")]
    [SerializeField] private float aggroDistance = 5f;

    private float movementDelayTimer = 0f;
    private int patrolPointsIndex = 0;
    
    // Attack
    private float minDistance2Player = 2f;
    
    // Components
    private GameObject playerGO;
    private NavMeshAgent agent;
    private LayerMask playerLayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        if (state == EnemyController.State.Patrol)
        {
            Patrol();
        }
        if (state == EnemyController.State.Chase)
        {
            Debug.Log("test");
            Chase();
        }
        if (state == EnemyController.State.Attack)
        {
            Attack();
        }
    }

    private void Patrol()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectRange, playerLayer))
        {
            playerGO = hit.transform.gameObject;
            state = State.Chase;
        }

        agent.destination = new Vector3(patrolPoints[patrolPointsIndex], transform.position.y, transform.position.z);
        
        // Has target been reached?
        if (agent.remainingDistance < 0.1f)
        {
            movementDelayTimer += Time.deltaTime;
            // To wait enemy in its position in amount of delay
            if (movementDelayTimer > movementDelay)
            {
                patrolPointsIndex++;
                movementDelayTimer = 0;

                if (patrolPointsIndex == patrolPoints.Length)
                {
                    patrolPointsIndex = 0;
                }
            }
        }
    }

    private void Chase()
    {
        // If player moves away from enemy, back to patrol
        if (Mathf.Abs(transform.position.x - playerGO.transform.position.x) > aggroDistance)
        {
            playerGO = null;
            state = State.Patrol;
        }
        else if(!(Mathf.Abs(transform.position.x - playerGO.transform.position.x) < minDistance2Player))
        {
            agent.destination = playerGO.transform.position;
        }
        else
        {
            agent.ResetPath();
            state = State.Attack;
        }
    }

    private void Attack()
    {

    }
}
