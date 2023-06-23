using UnityEngine;
using UnityEngine.AI;

public abstract class Child : MonoBehaviour
{
    protected NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
}
