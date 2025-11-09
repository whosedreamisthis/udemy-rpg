using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    private SpriteRenderer sr;
    private CapsuleCollider2D cc;
    public PlayerInputSet input { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerFallState fallState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }


    public Vector2 moveInput { get; private set; }

    [Header("Movement details")]
    public float moveSpeed;
    public float jumpForce = 5;
    private int facingDirection = 1;
    private bool facingRight = true;

    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }

    public bool wallDetected { get; private set; }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = anim.GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        input = new PlayerInputSet();
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");
        jumpState = new PlayerJumpState(this, stateMachine, "jumpFall");
        fallState = new PlayerFallState(this, stateMachine, "jumpFall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        // Time.timeScale = 0.1f;
    }

    private void OnEnable()
    {
        input.Enable();

        // input.Player.Movement.started
        input.Player.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        input.Player.Movement.canceled += context => moveInput = Vector2.zero;
    }
  private void OnDisable()
  {
        input.Disable();
  }

  private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleCollisionDetection ()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    private void HandleFlip(float xVelocity)
    {
        // Debug.Log("handleFlip xVelocity "  +xVelocity + " facingRight " + facingRight);
        if (xVelocity > 0 && !facingRight)
        {
            // Debug.Log(" if (xVelocity > 0 && !facingRight) flip");
            Flip();
        }
        if (xVelocity < 0 && facingRight )
        {
            // Debug.Log(" if (xVelocity < 0 && facingRight) flip");
                Flip();
        }
  }
    public void Flip()
    {
        
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDirection = -1 * facingDirection;
        Debug.Log("facing right "+ facingRight + "xVelocity " + rb.linearVelocity.x);
        //   if (moveInput.x < 0)
        //     {
        //         sr.flipX = true;
        //         facingDirection = -1;
        //         cc.offset = new Vector2(0.8f, cc.offset.y);
        //     }
        //     else if (moveInput.x > 0)
        //     {
        //         sr.flipX = false;
        //         facingDirection = 1;
        //         cc.offset = new Vector2(0, cc.offset.y);
        //     }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
