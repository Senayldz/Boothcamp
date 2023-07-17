using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : EnemyState
{
    [Header("Navigation")]
    [SerializeField] private float[] patrolPoints;
    [SerializeField] private float detectRange;
    [SerializeField] private float movementDelay;
    [SerializeField] private float minDistance2Player;
    [SerializeField] private float aggroDistance;

    [Header("Attributes")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float currentHealth;

    [Header("Chase")]
    [SerializeField] private GameObject playerObject;
    
    [Header("UI")]
    [SerializeField] private Image healthBar;

    // Components
    private MouthDetector mouthDetector;
    private NavMeshAgent agent;
    private LayerMask playerLayer;
    private BoxCollider boxCollider;
    private Animator animator;

    private float movementDelayTimer = 0f;
    private float imageHealthRatio;
    private int patrolPointsIndex = 0;
    private bool isAttackAnimPlaying;
    private bool isDeathAnimPlaying;
    private bool isAlreadyWorking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerLayer = LayerMask.GetMask("Player");
        boxCollider = GetComponent<BoxCollider>(); 
        animator = GetComponent<Animator>();
        mouthDetector = GetComponentInChildren<MouthDetector>();

        float healthBarMaxFillAmount = 1;

        imageHealthRatio = healthBarMaxFillAmount / currentHealth;
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
            case State.Death:
                Debug.Log("Death State");
                Death();
                break;
            default:
                break;
        }
        
        if (currentHealth <= 0)
            currentState = State.Death;
    }

    private void Patrol()
    {
        float tolerance = 0.1f;
        agent.destination = new Vector3(patrolPoints[patrolPointsIndex], transform.position.y, transform.position.z);
        animator.SetBool("isWalking", true);
        
        if (Physics.CheckSphere(transform.position, detectRange, playerLayer))
        {
            agent.isStopped = true;
            animator.SetBool("isWaiting", false);
            animator.SetBool("isWalking", false);
            animator.SetTrigger("isRoar");
            transform.LookAt(playerObject.transform);   
        }

        // Has target been reached?
        else if (Equal(transform.position.x, patrolPoints[patrolPointsIndex], tolerance))
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
        if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > aggroDistance)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = walkSpeed;

            currentState = State.Patrol;
        }
        // Chase player
        else
        {
            agent.destination = playerObject.transform.position;
        }
    }

    private void Attack()
    {
        if (isAttackAnimPlaying) return;
        isAttackAnimPlaying = true;
        animator.SetBool("isAttacking", true);
        playerObject.GetComponent<PlayerController>().TakeDamage(20);
    }

    private void Death()
    {
        if (isDeathAnimPlaying) return;

        isDeathAnimPlaying = true;
        agent.isStopped = true;
        animator.SetTrigger("isDeath");
    }
    // Connected to Death Animation
    private void ExitDeathState()
    {
        Destroy(this.gameObject);
    }
    // Connected to Attack Anim
    private void ExitAttackState()
    {
        animator.SetBool("isAttacking", false);
        isAttackAnimPlaying = false;
    }
    private void ExitPatrolState()
    {
        currentState = State.Chase;
        agent.speed = runSpeed;
        agent.isStopped = false;
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

    private bool Equal(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) <= tolerance);
    }

    public void StartTakeDamage(float takenDamage)
    {
        StartCoroutine(TakeDamage(takenDamage));
    }

    private IEnumerator TakeDamage(float takenDamage)
    {
        if (isAlreadyWorking) yield break;

        isAlreadyWorking = true;

        float counter = 0;
        float duration = 1f;

        //Get the current life of the player
        float startHealth = currentHealth;

        //Calculate how much to lose
        float finalHealth = currentHealth - takenDamage;

        //Stores the new player life
        float newCurrentHealth;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            newCurrentHealth = Mathf.Lerp(startHealth, finalHealth, counter / duration);
            healthBar.fillAmount = newCurrentHealth * imageHealthRatio;
            currentHealth = newCurrentHealth;
            yield return null;
        }

        isAlreadyWorking = false;
        yield return null;
    }
}
