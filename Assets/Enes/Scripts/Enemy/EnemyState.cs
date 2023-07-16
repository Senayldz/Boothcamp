using UnityEngine;

public class EnemyState : MonoBehaviour 
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Death
    }

    public State currentState = State.Patrol;
}
