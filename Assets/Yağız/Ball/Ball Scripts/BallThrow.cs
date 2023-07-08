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

    PlayerController playercontrol;

    Animator playeranim;

    bool readyToThrow;

    void Start()
    {
        readyToThrow = true;
        playercontrol = GetComponent<PlayerController>();
        playeranim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && readyToThrow )
        {
            Throw();
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
            projectile.SetActive(true);
        }

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
           
            Vector3 forceDirection = (hit.point - attackPoint.position).normalized;
            forceDirection.z = 0;
            if ((hit.point.x > transform.position.x && !playercontrol.FacingRight) | (hit.point.x < transform.position.x && playercontrol.FacingRight))
            {
                projectile.SetActive(false);
            }
            playeranim.SetTrigger("throwing");
            Vector3 forceToAdd = forceDirection * ThrowForce + transform.up * ThrowUpwardForce;
            projectileRb.AddForce(forceToAdd,ForceMode.Impulse);


        }



        

        StartCoroutine(ResetThrow());



    }

    

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(ThrowCooldown);
        readyToThrow = true;
        //Destroy(GameObject.FindWithTag("Ball"));
        GameObject obj = GameObject.FindWithTag("Ball");
        if (obj != null)
        {
            obj.SetActive(false);
        }
       
       

        
    }
}
