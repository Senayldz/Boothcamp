using System;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : EnemyState
{
    public bool draw;

    [Header("Navigation")]
    [SerializeField] private float[] patrolPoints;
    [SerializeField] private float detectRange;
    [SerializeField] private float movementDelay;
    [SerializeField] private float minDistance2Player;

    [Header("Attributes")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Chase")]
    [SerializeField] private float aggroDistance = 5f;


    
    private float movementDelayTimer = 0f;
    private int patrolPointsIndex = 0;
    private bool isAttackAnimPlaying;

    // Components
    private MouthDetector mouthDetector;
    private GameObject playerGO;
    private NavMeshAgent agent;
    private LayerMask playerLayer;
    private BoxCollider enemyCollider;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerLayer = LayerMask.GetMask("Player");
        enemyCollider = GetComponent<BoxCollider>(); 
        animator = GetComponent<Animator>();
        mouthDetector = GetComponentInChildren<MouthDetector>();
    }

    private void OnEnable()
    {
        mouthDetector.onTriggerEnter += OnMouthTriggerEnter;
        mouthDetector.onTriggerExit += OnMouthTriggerExit;
    }

    private void OnDisable()
    {
        mouthDetector.onTriggerEnter -= OnMouthTriggerEnter;
        mouthDetector.onTriggerExit -= OnMouthTriggerExit;
    }

    private void OnMouthTriggerEnter()
    {
        animator.SetBool("isRunning", false);
        currentState = State.Attack;
    }

    private void OnMouthTriggerExit()
    {
        currentState = State.Chase;
    }

    private void Start()
    {
        agent.speed = walkSpeed;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Debug.Log("Idle State");
                break;
            case State.Patrol:
                Debug.Log("Patrol State");
                Patrol();
                break;
            case State.Chase:
                Debug.Log("Chase State");
                Chase();
                break;
            case State.Attack:
                Debug.Log("Attack State");  
                Attack();
                break;
            default:
                break;
        }
    }

    private void Patrol()
    {
        float tolerance = 0.1f;

        Vector3 rayStartPos = transform.position;
        animator.SetBool("isWalking", true);

        // Switch to chase state
        if (Physics.Raycast(rayStartPos, transform.forward, out RaycastHit hit, detectRange, playerLayer))
        {
            currentState = State.Chase;
            playerGO = hit.transform.gameObject;
            animator.SetBool("isWaiting", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            agent.speed = runSpeed;
            return;
            
        }

        agent.destination = new Vector3(patrolPoints[patrolPointsIndex], transform.position.y, transform.position.z);
        
        // Has target been reached?
        if (Equal(transform.position.x, patrolPoints[patrolPointsIndex], tolerance))
        {
            animator.SetBool("isWaiting", true);
            movementDelayTimer += Time.deltaTime;
            
            // To wait enemy in its position in amount of delay
            if (movementDelayTimer >= movementDelay)
            {
                patrolPointsIndex++;
                movementDelayTimer = 0f;
                
                if (patrolPointsIndex == patrolPoints.Length)
                {
                    patrolPointsIndex = 0;
                }
                animator.SetBool("isWaiting", false);
            }
        }
    }

    private void Chase()
    {
        animator.SetBool("isRunning", true);

        // If player moves away from enemy, back to patrol
        if (Mathf.Abs(transform.position.x - playerGO.transform.position.x) > aggroDistance)
        {
            playerGO = null;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = walkSpeed;

            currentState = State.Patrol;
        }
        // Chase player
        else
        {
            agent.destination = playerGO.transform.position;
        }
    }

    private void Attack()
    {
        if (isAttackAnimPlaying) return;
        isAttackAnimPlaying = true;
        animator.SetBool("isAttacking", true);
    }
    // Connected to Attack Anim
    private void ExitAttackState()
    {
        animator.SetBool("isAttacking", false);
        isAttackAnimPlaying = false;
    }

    private bool Equal(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) <= tolerance);
    }
}
