using UnityEngine;

public class BallDamage : MonoBehaviour
{
    private float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyController>().StartTakeDamage(damage);
    }

}