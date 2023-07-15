using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowController : MonoBehaviour
{

    [SerializeField] Transform camTransform;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject objectToThrow;

    public float ThrowCooldown;
    [SerializeField] float ThrowForce;
    [SerializeField] float ThrowUpwardForce;
    [SerializeField] float offsetY = 0.085f;

    PlayerController playercontrol;
    DecraseOpasity ballCd;

    Animator playeranim;

    Rigidbody playerRb;

    

    public RaycastHit hit;

    bool readyToThrow;
    public bool ReadyToThrow { get { return readyToThrow; } }

    bool avoidThrow;
    public bool AvoidThrow { get { return avoidThrow; } }

    void Start()
    {
        readyToThrow = true;
        playercontrol = GetComponent<PlayerController>();
        playeranim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        ballCd = GameObject.FindGameObjectWithTag("Skill").GetComponent<DecraseOpasity>();
        
    }


    void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && readyToThrow)
        {
            StartCoroutine(Throw());
        }
    }

    IEnumerator Throw()
    {

        readyToThrow = false;
        // GameObject projectile = Instantiate(objectToThrow, attackPoint.position, camTransform.rotation);
        GameObject projectile = ObjectPool.instance.GetPooledObject();


        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            Vector3 forceDirection = (hit.point - attackPoint.position).normalized;
            forceDirection.z = 0;
            forceDirection.y = forceDirection.y + offsetY;
            Debug.Log(forceDirection);
            if (playercontrol.moveX == 0)
            {

                playeranim.SetBool("isThrow", true);
                

            }
            if ((hit.point.x > transform.position.x && !playercontrol.FacingRight) | (hit.point.x < transform.position.x && playercontrol.FacingRight))
            {

                playeranim.SetBool("isThrow", false);
                
            }
            if (ballCd.count <= 0)
            {
                projectile.SetActive(false);
                ballCd.count = 0;
                playeranim.SetBool("isThrow", false);
                avoidThrow = true;

            }
            else
            {
                avoidThrow = false;
            }



            yield return new WaitForSeconds(1f);

            
            if (projectile != null && !avoidThrow)
            {
                projectile.transform.position = attackPoint.position;
                projectile.transform.rotation = Camera.main.transform.rotation;
                projectile.SetActive(true);
                
               

            }
            if ((hit.point.x > transform.position.x && !playercontrol.FacingRight) | (hit.point.x < transform.position.x && playercontrol.FacingRight))
            {
                
                projectile.SetActive(false);
                

            }
            
           


            Vector3 forceToAdd = forceDirection * ThrowForce;


            // Relative Velocity
            if (playercontrol.moveX != 0)
            {
                projectile.SetActive(false);
                playeranim.SetBool("isThrow", false);
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
}
