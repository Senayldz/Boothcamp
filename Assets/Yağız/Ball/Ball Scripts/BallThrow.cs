using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrow : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject objectToThrow;

    [SerializeField] float ThrowCooldown;
    [SerializeField] float ThrowForce;
    [SerializeField] float ThrowUpwardForce;
    [SerializeField] float offsetY = 0.085f;

    PlayerController playercontrol;

    Animator playeranim;

    Rigidbody playerRb;

    bool readyToThrow;

    void Start()
    {
        readyToThrow = true;
        playeranim.SetBool("isThrow", true);

        playercontrol = GetComponent<PlayerController>();
        playeranim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && readyToThrow )
        {
            StartCoroutine(ThrowDelay());
        }
    }

    void Throw()
    {
        readyToThrow = false;
        // GameObject projectile = Instantiate(objectToThrow, attackPoint.position, camTransform.rotation);
        GameObject projectile = ObjectPool.instance.GetPooledObject();
        if (projectile != null)
        {
            projectile.transform.position = attackPoint.position;
            projectile.transform.rotation = Camera.main.transform.rotation;
            projectile.SetActive(true);
        }

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
           
            Vector3 forceDirection = (hit.point - attackPoint.position).normalized;
            forceDirection.z = 0;
            forceDirection.y = forceDirection.y + offsetY;
            Debug.Log(forceDirection);
            if ((hit.point.x > transform.position.x && !playercontrol.FacingRight) | (hit.point.x < transform.position.x && playercontrol.FacingRight))
            {
                projectile.SetActive(false);
            }
            playeranim.SetTrigger("throwing");
           Vector3 forceToAdd = forceDirection * ThrowForce;
            

            // Relative Velocity
            if (playercontrol.moveX != 0)
            {
                projectileRb.velocity = forceToAdd + new Vector3(playercontrol.moveX * playercontrol.playerSpeed, playerRb.velocity.y, 0);
            }
            else
            {
                projectileRb.velocity = forceToAdd;
            }
            


        }



        

        StartCoroutine(ResetThrow());



    }

    

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(ThrowCooldown);
        readyToThrow = true;
        playeranim.SetBool("isThrow", false);

        //Destroy(GameObject.FindWithTag("Ball"));
        GameObject obj = GameObject.FindWithTag("Ball");
        if (obj != null)
        {
            obj.SetActive(false);
        }
       
       

        
    }
    IEnumerator ThrowDelay()
    {
        yield return new WaitForSeconds(1.15f);
        Throw();
    }
}
