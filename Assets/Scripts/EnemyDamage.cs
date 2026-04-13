using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 1;
    public float attackCooldown = 1f;
    public float attackRange = 1.2f;

    [Header("Bat / Simple Enemy Support")]
    public bool useSimpleDistanceAttack = false;

    private float cooldownTimer = 0f;
    private Transform player;
    private EnemyPatrol enemyPatrol;

    void Start()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        bool canAttack = false;

        // For bats or simple flying enemies
        if (useSimpleDistanceAttack)
        {
            canAttack = distance <= attackRange;
        }
        // For patrol/chase enemies like wolf/tree
        else if (enemyPatrol != null && enemyPatrol.IsAttacking() && distance <= attackRange)
        {
            canAttack = true;
        }

        if (canAttack && cooldownTimer <= 0f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount, transform.position);
                cooldownTimer = attackCooldown;
            }
        }
    }
}