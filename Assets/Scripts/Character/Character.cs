using System;
using UnityEngine;

public class Character : Unit
{
    [SerializeField] public int lives = 3;
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float jumpTime = 0.35f;
    [SerializeField] private float invulnerabilityDuration = 1.0f;
    [SerializeField] private bool inInvulnerability;
    
    private static readonly int State = Animator.StringToHash("State");

    private float invulnerabilityTimer;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    private new Transform transform;
    public CharLivesBar charLivesBar;
    
    private float jumpTimeCounter;
    private bool isJumping;
    private bool isGrounded;
    private bool isFacingRight;
    
    private CharacterState state
    {
        get => (CharacterState)animator.GetInteger(State);
        set => animator.SetInteger(State, (int)value);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }
    
    private void Update()
    {
        if (isGrounded) state = CharacterState.Idle;
        if (Input.GetButton("Jump")) Jump();
        if (Input.GetButtonUp("Jump")) isJumping = false;
        if (Input.GetButton("Horizontal")) Run();
    }
    
    private void FixedUpdate()
    {
        
    }
    
    private void Run()
    {
        var direction = transform.right * Input.GetAxis("Horizontal");
        var position = transform.position; 
        transform.position = Vector3.MoveTowards(position, position + direction, speed * Time.deltaTime);
        if (direction.x < 0 && !isFacingRight || direction.x > 0 && isFacingRight) Flip();
        if (isGrounded) state = CharacterState.Run;
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            state = CharacterState.Jump;
            rigidbody.velocity = transform.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
            isGrounded = false;
        }

        if (!isJumping) return;
        
        if (jumpTimeCounter > 0)
        {
            rigidbody.velocity = transform.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }
    
    public override void ReceiveDamage(int damage)
    {
        lives -= damage;
        charLivesBar.Refresh();
        if (lives <= 0) Die();
        inInvulnerability = true;
        // Player hurt animation
    }

    protected override void Die()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("OneWayPlatform");
    }
}

public enum CharacterState
{
    Idle = 0,
    Run = 1,
    Jump = 2,
}