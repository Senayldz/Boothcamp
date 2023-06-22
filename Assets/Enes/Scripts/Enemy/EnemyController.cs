using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyState))]
public abstract class EnemyController : MonoBehaviour
{
    protected EnemyState enemyState;

    private NavMeshAgent agent;

    private LayerMask playerLayer;

    private float timer = 0f;
    private int index = 0;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Start()
    {
        enemyState = GetComponent<EnemyState>();
    }

    protected virtual void Patrol(float[] patrolPoints, float timeDelay)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, playerLayer))
        {
            enemyState.state = EnemyState.State.Attack;
        }

        agent.destination = new Vector3(patrolPoints[index], transform.position.y, transform.position.z);

        // Has target been reached?
        if (agent.remainingDistance < 0.1f)
        {
            timer += Time.deltaTime;
            // To wait enemy in its position in amount of delay
            if (timer > timeDelay)
            {
                index++;
                timer = 0;

                if (index == patrolPoints.Length)
                {
                    index = 0;
                }
            }
        }
    }

    protected virtual void Encounter()
    {

        
    }

    protected abstract void Attack();

}
