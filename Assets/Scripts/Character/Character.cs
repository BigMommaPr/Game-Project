using UnityEngine;

public class Character : Unit
{
    [Header("Physics parameters")]
    [SerializeField] public int lives;
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private int extraJumpsValue = 2;
    [SerializeField] private float timeOfInvulnerability = 0.6f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private int maxLives = 3;
    
    [Header("Related objects")]
    [SerializeField] private CharLivesBar charLivesBar;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioManager audioManager;
    
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask groundLayers;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform trf;
    
    private float invulnerabilityTimer;
    private int extraJumps;
    private float nextAttackTime;
    
    [Header("Character state conditions")]
    public bool isFacingRight;
    public bool inInvulnerability;
    private bool isGrounded;
    
    private static readonly int State = Animator.StringToHash("State");
    
    private CharacterState state
    {
        get => (CharacterState)animator.GetInteger(State);
        set => animator.SetInteger(State, (int)value);
    }

    private void Awake()
    {
        lives = maxLives;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trf = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetButton("Horizontal")) Run();
        else if (isGrounded) state = CharacterState.Idle;
        if (Input.GetButton("Fire1") && Time.time >= nextAttackTime) Attack();
        else if (Input.GetButtonDown("Jump")) Jump();
    }

    private void FixedUpdate()
    {
        UpdateInvulnerability();
        var ground = Physics2D.OverlapCircle(groundPosition.position, groundCheckRadius, groundLayers);
        if (!ground)
        {
            isGrounded = false;
            return;
        }
        if (ground.CompareTag("OneWayPlatform")) 
            isGrounded = rb.velocity.y < 0.1;
        else 
            isGrounded = ground;
    }
    
    private void UpdateInvulnerability()
    {
        if (!inInvulnerability) return;
        invulnerabilityTimer -= Time.deltaTime;
        if (invulnerabilityTimer < 0) inInvulnerability = false;
    }

    private void Run()
    {
        var direction = transform.right * Input.GetAxis("Horizontal");
        var position = transform.position; 
        transform.position = Vector3.MoveTowards(position, position + direction,  speed * Time.deltaTime);
        if (direction.x < 0 && !isFacingRight || direction.x > 0 && isFacingRight) Flip();
        if (isGrounded) state = CharacterState.Run;
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var theScale = trf.localScale;
        theScale.x *= -1;
        trf.localScale = theScale;
    }
    
    private void Jump()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (extraJumps > 0)
        {
            state = CharacterState.Jump;
            audioManager.Play("Jump");
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
    }
    
    public override void ReceiveDamage(int damage)
    {
        if (inInvulnerability) return;
        animator.SetTrigger("IsAttacked");
        audioManager.Play("ReceiveDamage");
        lives -= damage;
        charLivesBar.Refresh();
        if (lives <= 0) Die();
        inInvulnerability = true;
        invulnerabilityTimer = timeOfInvulnerability;
    }

    protected override void Die()
    {
        state = CharacterState.Died;
        levelManager.ShowGameOverWindow();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Attack()
    {
        animator.SetTrigger("IsAttack");
        state = CharacterState.Attack;
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        audioManager.Play(hitEnemies.Length == 0 ? "MissSword" : "Sword");
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Unit>()?.ReceiveDamage(attackDamage);
        }
        nextAttackTime = Time.time + 1f / attackRate;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Spores")) ReceiveDamage(1);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint is null || groundPosition is null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(groundPosition.position, groundCheckRadius);
    }
}

public enum CharacterState
{
    Idle = 0,
    Run = 1,
    Jump = 2,
    Died = 3,
    Attack = 4,
}