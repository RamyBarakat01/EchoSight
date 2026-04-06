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

    private Transform targetPoint;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        targetPoint = pointB;
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        // Start chasing
        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        // Stop chasing if player is too far
        if (isChasing && distanceToPlayer > losePlayerRange)
        {
            isChasing = false;
            isAttacking = false;
        }

        // Attack if close enough
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
            // Stay in place while attacking
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
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
            if (spriteRenderer != null) spriteRenderer.flipX = false;
        }
        else if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
        {
            targetPoint = pointA;
            if (spriteRenderer != null) spriteRenderer.flipX = true;
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
        if (spriteRenderer != null)
        {
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = true;
            else if (player.position.x > transform.position.x)
                spriteRenderer.flipX = false;
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}