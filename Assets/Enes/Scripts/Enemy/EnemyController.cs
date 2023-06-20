using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Patrol(float firstPoint, float secondPoint)
    {
        agent.destination = new Vector3(firstPoint, transform.position.y, transform.position.z);
    }

    protected abstract void Attack();

    protected abstract void Encounter();

    


}
