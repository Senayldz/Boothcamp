using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float throwSpeed = 10f;
    public float returnSpeed = 10f;
    [SerializeField] float waitTime = 4f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool returning = false;
    private Rigidbody rb;
    private bool isRotating;
    private SphereCollider ballCollider;

    public bool IsRotating { get { return isRotating; } }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb.isKinematic = true;
        isRotating = true;
        ballCollider = GetComponent<SphereCollider>();
        
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !returning)
        {
            Throw();
        }
    }

    private void Throw()
    {
 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            initialPosition = transform.position;

        }
        Vector3 throwDirection = (hit.point - transform.position).normalized;
        Vector3 throwVelocity = throwDirection * throwSpeed;

        rb.isKinematic = false;
        rb.velocity = throwVelocity;
        isRotating = false;
        ballCollider.isTrigger = true;

        StartCoroutine(ReturnBoomerang());
    }

    private IEnumerator ReturnBoomerang()
    {
        yield return new WaitForSeconds(waitTime); 

        returning = true;

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            Vector3 returnDirection = (initialPosition - transform.position).normalized;
            Vector3 returnVelocity = returnDirection * returnSpeed;

            rb.velocity = returnVelocity;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, returnSpeed * Time.deltaTime * 10f);
            yield return null;
        }

        rb.isKinematic = true;
        returning = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isRotating = true;
        rb.velocity = Vector3.zero;
        ballCollider.isTrigger = false;
    }
}

   
