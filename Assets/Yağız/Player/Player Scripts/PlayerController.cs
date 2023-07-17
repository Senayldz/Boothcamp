using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    SoundManager soundManager;
    public float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float turnSpeed;
    
    
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private GameObject draggableObject;

    private LayerMask groundLayer;
    public float groundCheckDistance = 0.5f; // Varsayılan olarak 0.5 birim
    public GameObject groundCheck;

    bool facingRight;

    public float moveX;
    public bool FacingRight { get { return facingRight; } }
    bool isGrounded;
    bool isFlip;

    BallThrowController ballthrow;


    Collider[] groundcollision;

    Animator playerAnim;

    Rigidbody playerRb;

    PlayerDragController dragControl;
    bool isDead = true;
    private float deadCooldown;

    [SerializeField] CinemachineVirtualCamera rightCam;
    [SerializeField] CinemachineVirtualCamera topDownCam;
    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Draggable", "Ground");
    }

    private void OnEnable()
    {
        CameraSwitch.Register(rightCam);
        CameraSwitch.Register(topDownCam);
        CameraSwitch.SwitchCamera(rightCam);
    }
    void Start()
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        dragControl = GetComponent<PlayerDragController>();
        ballthrow = GetComponent<BallThrowController>();
        draggableObject = GameObject.FindGameObjectWithTag("Draggable");
        facingRight = true;
        isGrounded = true;
        isFlip = true;
        deadCooldown = 0;
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()

    {  
        deadCooldown -= Time.deltaTime;
        
        PreventInfiniteJump();
        MovementWhileDragging();
        CamChange();
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckDistance, groundLayer);
        Jump();
        //if (dragControl.IsPickedUp && transform.position.x > draggableObject.transform.localPosition.x && facingRight)
        //{
        //    Flip();
        //}

        //else if (dragControl.IsPickedUp && transform.position.x < draggableObject.transform.localPosition.x && !facingRight)
        //{
        //    Flip();
        //}
        if (playerAnim.GetBool("isFall"))
        {
            playerRb.drag = 2.0f;
            
            
        }

        if (!isDead)
        {
            TakeDamage(5);
        }
            if (!isDead && deadCooldown<=0)
          {
              
              SceneManager.LoadScene(2);
          }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void OnDisable()
    {
        CameraSwitch.UnRegister(rightCam);
        CameraSwitch.UnRegister(topDownCam);
    }

    void Movement()
    {
        if (!isDead)
        {
            soundManager.DeathSound();
            return;
        }
        moveX = Input.GetAxis("Horizontal");
        playerAnim.SetFloat("speed", Mathf.Abs(moveX));

        playerRb.velocity = new Vector3(moveX * playerSpeed, playerRb.velocity.y, 0);

        if (moveX > 0 && !facingRight)
        {
            Flip();
        }

        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

    }

    void Jump()
    {
        if (!isDead)
        {
            soundManager.DeathSound();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, 0);

            soundManager.JumpSound();
        }
        playerAnim.SetBool("grounded", isGrounded);

    }

    public void Flip()
    {
        if(!isDead || playerAnim.GetBool("Death"))
        {
            soundManager.DeathSound();
            return;
        }
        if (isFlip)
        {
            facingRight = !facingRight;

            Vector3 playerScale = transform.localScale;
            playerScale.z *= -1;
            transform.localScale = playerScale;

        }
    }

    void PreventInfiniteJump()
    {
        groundcollision = Physics.OverlapSphere(groundCheck.transform.position, groundCheckRadius, groundLayer);

        if (groundcollision.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void MovementWhileDragging()
    {
        if (dragControl.IsPickedUp)
        {
            playerRb.constraints = RigidbodyConstraints.FreezeAll;

        }
        else if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
            isFlip = false;
            Invoke("BackToFlip", 1.6f);


        }

        else
        {
            playerRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        }

    }
    void BackToFlip()
    {
        isFlip = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (isGrounded == true)
        {
            playerAnim.SetBool("isFall", false);

        }
        else
        {
            playerAnim.SetBool("isFall", true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall") && isGrounded == false)
        {
            playerAnim.SetBool("isFall", true);
        }
        if (other.CompareTag("Death"))
        {
            playerAnim.SetTrigger("Death");
            LockMovement();
        }
    }

    void LockMovement()
    {
        isDead = false;
        moveX = 0;
        playerRb.velocity = Vector3.zero;
        deadCooldown = 4;
    }
    void CamChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CameraSwitch.IsActiveCamera(rightCam))
            {
                CameraSwitch.SwitchCamera(topDownCam);
            }
            else if (CameraSwitch.IsActiveCamera(topDownCam))
            {
                CameraSwitch.SwitchCamera(rightCam);
            }
        }
    }
}


