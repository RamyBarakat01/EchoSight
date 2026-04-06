using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyFootstepPulse : MonoBehaviour
{
    public GameObject soundPulsePrefab;
    public AudioClip footstepClip;

    public float footstepInterval = 0.5f;
    public float minMoveSpeedToTrigger = 0.1f;

    private float footstepTimer = 0f;
    private Vector3 lastPosition;
    private AudioSource audioSource;

    void Start()
    {
        lastPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float movedDistance = Vector3.Distance(transform.position, lastPosition);
        float moveSpeed = movedDistance / Time.deltaTime;

        if (moveSpeed > minMoveSpeedToTrigger)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval)
            {
                TriggerFootstep();
                footstepTimer = 0f;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void TriggerFootstep()
    {
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }

        if (soundPulsePrefab != null)
        {
            Instantiate(soundPulsePrefab, transform.position, Quaternion.identity);
        }
    }
}