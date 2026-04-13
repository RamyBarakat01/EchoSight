using UnityEngine;

public class BatChase : MonoBehaviour
{
    [Header("Movement")]
    public float chaseSpeed = 4f;
    public float detectionRange = 6f;
    public float stopDistance = 0.8f;

    [Header("Pulse")]
    public GameObject soundPulsePrefab;
    public float pulseInterval = 0.4f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float pulseTimer = 0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > stopDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );

            FacePlayer();

            pulseTimer += Time.deltaTime;

            if (pulseTimer >= pulseInterval)
            {
                SpawnPulse();
                pulseTimer = 0f;
            }
        }
    }

    void FacePlayer()
    {
        if (spriteRenderer == null || player == null) return;

        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else if (player.position.x > transform.position.x)
            spriteRenderer.flipX = false;
    }

    void SpawnPulse()
    {
        if (soundPulsePrefab != null)
        {
            Instantiate(soundPulsePrefab, transform.position, Quaternion.identity);
        }
    }
}