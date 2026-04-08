using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Chase")]
    public float chaseSpeed = 3f;
    public float detectionRange = 4f;
    public float losePlayerRange = 6f;

    [Header("Attack")]
    public float attackRange = 1.2f;

    [Header("Sprite Facing")]
    public bool spriteFacesRightByDefault = true;

    [Header("Animation")]
    public bool useRunForChase = true;

    private Transform targetPoint;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        targetPoint = pointB;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing && distanceToPlayer > losePlayerRange)
        {
            isChasing = false;
            isAttacking = false;
        }

        if (isChasing && distanceToPlayer <= attackRange)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            FacePlayer();
            SetAnimatorState(0, true);   // idle + attacking
        }
        else if (isChasing)
        {
            ChasePlayer();

            if (useRunForChase)
                SetAnimatorState(2, false);   // run
            else
                SetAnimatorState(1, false);   // walk
        }
        else
        {
            Patrol();
            SetAnimatorState(1, false);       // walk
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            patrolSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
        {
            targetPoint = pointB;
            SetFacing(true);   // moving right
        }
        else if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
        {
            targetPoint = pointA;
            SetFacing(false);  // moving left
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        FacePlayer();
    }

    void FacePlayer()
    {
        if (player.position.x > transform.position.x)
            SetFacing(true);
        else if (player.position.x < transform.position.x)
            SetFacing(false);
    }

    void SetFacing(bool movingRight)
    {
        if (spriteRenderer == null) return;

        if (spriteFacesRightByDefault)
        {
            spriteRenderer.flipX = !movingRight;
        }
        else
        {
            spriteRenderer.flipX = movingRight;
        }
    }

    void SetAnimatorState(int moveState, bool attacking)
    {
        if (animator != null)
        {
            animator.SetInteger("MoveState", moveState);
            animator.SetBool("IsAttacking", attacking);
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}