using UnityEngine;

public class PulseSpawner : MonoBehaviour
{
    public GameObject soundPulsePrefab;
    public Transform pulseSpawnPoint;

    public float pulseCooldown = 3f;

    private float lastPulseTime = -10f;

    void Update()
    {
        bool keyboardPulse = Input.GetKeyDown(KeyCode.E);
        bool mobilePulse = false;

        if (MobileControls.instance != null)
        {
            mobilePulse = MobileControls.instance.pulsePressed;
        }

        if ((keyboardPulse || mobilePulse) && Time.time >= lastPulseTime + pulseCooldown)
        {
            SpawnPulse();
            lastPulseTime = Time.time;

            if (MobileControls.instance != null)
            {
                MobileControls.instance.ResetPulseButton();
            }
        }
    }

    void SpawnPulse()
    {
        if (soundPulsePrefab != null && pulseSpawnPoint != null)
        {
            Instantiate(soundPulsePrefab, pulseSpawnPoint.position, Quaternion.identity);
        }
    }
}