using UnityEngine;

public class EnemySoundEmitter : MonoBehaviour
{
    public GameObject soundPulsePrefab;
    public float pulseInterval = 2.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= pulseInterval)
        {
            EmitPulse();
            timer = 0f;
        }
    }

    void EmitPulse()
    {
        if (soundPulsePrefab != null)
        {
            Instantiate(soundPulsePrefab, transform.position, Quaternion.identity);
        }
    }
}