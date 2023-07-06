using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject player;
    [SerializeField] float rotateSpeed = 180f;
   
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        
    }

    
    void FixedUpdate()
    {
        transform.RotateAround(player.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);

    }

   
}
