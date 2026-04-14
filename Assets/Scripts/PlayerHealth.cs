using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;

    private int currentHealth;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || (GameManager.Instance != null && GameManager.Instance.gameEnded))
            return;

        currentHealth -= damage;
        Debug.Log("Player took damage! Current HP: " + currentHealth);

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage, Vector2 enemyPosition)
    {
        if (isInvincible || (GameManager.Instance != null && GameManager.Instance.gameEnded))
            return;

        currentHealth -= damage;
        Debug.Log("Player took damage! Current HP: " + currentHealth);

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        // Knockback
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (rb != null)
        {
            Vector2 knockDirection = ((Vector2)transform.position - enemyPosition).normalized;
            knockDirection.y = 0.5f;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockDirection * 6f, ForceMode2D.Impulse);
        }

        if (movement != null)
        {
            movement.DisableMovementTemporarily(0.2f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Player healed! Current HP: " + currentHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}