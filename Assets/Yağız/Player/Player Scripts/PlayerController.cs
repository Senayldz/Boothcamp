using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float turnSpeed;

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

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Draggable", "Ground");
    }
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        dragControl = GetComponent<PlayerDragController>();
        ballthrow = GetComponent<BallThrowController>();
        draggableObject = GameObject.FindGameObjectWithTag("Draggable");
        facingRight = true;
        isGrounded = true;
        isFlip = true;

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        PreventInfiniteJump();
        MovementWhileDragging();
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckDistance, groundLayer);
        Jump();
        if (dragControl.IsPickedUp && transform.position.x > draggableObject.transform.position.x && facingRight)
        {
            Flip();
        }

        else if (dragControl.IsPickedUp && transform.position.x < draggableObject.transform.position.x && !facingRight)
        {
            Flip();
        }
        if (playerAnim.GetBool("isFall"))
        {
            playerRb.drag = 2.0f;
        }
    }


    void Movement()
    {
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

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, 0);


        }
        playerAnim.SetBool("grounded", isGrounded);

    }

    public void Flip()
    {
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

}


