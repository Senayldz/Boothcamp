using UnityEngine;

[RequireComponent (typeof(EnemyState))]
public class BearController : EnemyController
{
    [Header("Navigation")]
    [SerializeField] private float firstPatrolPoint;
    [SerializeField] private float secondPatrolPoint;

    private EnemyState enemyState;

    private void Start()
    {
        enemyState = GetComponent<EnemyState>();
    }

    private void Update()
    {
        if (enemyState.enemyState == EnemyState.State.Patrol)
        {
            Patrol(firstPatrolPoint, secondPatrolPoint);
        }

        if (enemyState.enemyState == EnemyState.State.Encounter)
        {
            Encounter();
        }

        if (enemyState.enemyState == EnemyState.State.Attack)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Encounter()
    {
        throw new System.NotImplementedException();
    }

    protected override void Patrol(float firstPatrolPoint, float secondPatrolPoint)
    {
        base.Patrol(firstPatrolPoint, secondPatrolPoint);
    }

}
