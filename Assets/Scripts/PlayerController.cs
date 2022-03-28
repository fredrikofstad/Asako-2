using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PhotonView view; 
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    //[SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundPoint;

    private bool isGrounded;

    private Vector2 moveInput;
    private bool jumpInput;


    private void Update()
    {
        HandleInput();
        HandleGroundDetection();
        HandleMovement();
        HandleJump();
    }


    private void FixedUpdate()
    {
        //
    }
    private void HandleInput()
    {
        if (!view.IsMine) return;
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();
        jumpInput = Input.GetButtonDown("Jump");
    }
    private void HandleMovement()
    {
        body.velocity = new Vector3(moveInput.x * speed, body.velocity.y, moveInput.y * speed);
        if (!sprite.flipX && moveInput.x < 0) sprite.flipX = true;
        else if (sprite.flipX && moveInput.x > 0) sprite.flipX = false;
    }
    
    private void HandleJump()
    {
        if (jumpInput && isGrounded)
            body.velocity += new Vector3(0f, jumpForce, 0f);
    }
    
    private void HandleGroundDetection()
    {
        // Casts a ray to detect whether groundPoint is close enough to the ground
        // outputs to hit, .25f distance to check

        RaycastHit hit;
        isGrounded = Physics.Raycast(groundPoint.position, Vector3.down, out hit, .25f);
        // if player falls down
        if (transform.position.y < -50) transform.position = new Vector3(0, 2, 0);
    }
}
