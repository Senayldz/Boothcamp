 using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Encounter,
        Attack
    }

    public State state = State.Patrol;
}
