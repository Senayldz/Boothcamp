using UnityEngine;

public class EnemyState : MonoBehaviour 
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    public State state = State.Patrol;
}
