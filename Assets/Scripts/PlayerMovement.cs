using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Jump Settings")]
    public int extraJumpsValue = 1;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.9f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

    [Header("Attack Effects")]
    public GameObject attackPulsePrefab;
    public AudioClip attackSound;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private bool canMove = true;
    private float movementLockTimer = 0f;

    private bool facingRight = true;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private int dashDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!canMove)
        {
            movementLockTimer -= Time.deltaTime;
            if (movementLockTimer <= 0f)
            {
                canMove = true;
            }
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }

            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsGrounded", isGrounded);

            if (MobileControls.instance != null)
            {
                MobileControls.instance.ResetActionButtons();
            }

            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (MobileControls.instance != null)
        {
            if (MobileControls.instance.moveLeft)
                moveInput = -1f;
            else if (MobileControls.instance.moveRight)
                moveInput = 1f;
        }

        if (canMove)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
        bool attackPressed = Input.GetKeyDown(KeyCode.J);
        bool dashPressed = Input.GetKeyDown(dashKey);

        if (MobileControls.instance != null)
        {
            jumpPressed |= MobileControls.instance.jumpPressed;
            attackPressed |= MobileControls.instance.attackPressed;
            dashPressed |= MobileControls.instance.dashPressed;
        }

        if (jumpPressed)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
            }
        }

        if (dashPressed && dashCooldownTimer <= 0f && canMove)
        {
            StartDash();
        }

        if (attackPressed)
        {
            Attack();
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);

        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }

        if (attackPoint != null)
        {
            attackPoint.localPosition = Vector3.zero;
        }

        if (MobileControls.instance != null)
        {
            MobileControls.instance.ResetActionButtons();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = facingRight ? 1 : -1;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        if (attackSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(attackSound);
        }

        if (attackPulsePrefab != null)
        {
            Instantiate(attackPulsePrefab, transform.position, Quaternion.identity);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void DisableMovementTemporarily(float duration)
    {
        canMove = false;
        movementLockTimer = duration;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}