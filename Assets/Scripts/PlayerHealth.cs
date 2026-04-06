using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;

    [Header("Hit Feedback")]
    public float flashDuration = 0.15f;
    public float knockbackForce = 6f;
    public float movementLockDuration = 0.15f;

    private int currentHealth;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
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

    public void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        Debug.Log("Player took damage! Current HP: " + currentHealth);

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        StartCoroutine(FlashRed());
        ApplyKnockback(hitSourcePosition);

        if (playerMovement != null)
        {
            playerMovement.DisableMovementTemporarily(movementLockDuration);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void ApplyKnockback(Vector2 hitSourcePosition)
    {
        if (rb != null)
        {
            Vector2 direction = ((Vector2)transform.position - hitSourcePosition).normalized;

            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Player healed! Current HP: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}