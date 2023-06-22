using UnityEngine;


public class BearController : EnemyController
{
    [Header("Navigation")]
    [SerializeField] private float[] patrolPoints;
    [SerializeField] private float timeDelay;

    private void Start()
    {
        
    }

    private void Update()
    {

        if (enemyState.state == EnemyState.State.Patrol)
        {
            Patrol(patrolPoints, timeDelay);
        }

        if (enemyState.state == EnemyState.State.Attack)
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

    protected override void Patrol(float[] patrolPoints, float timeDelay)
    {
        base.Patrol(patrolPoints, timeDelay);
    }

}
