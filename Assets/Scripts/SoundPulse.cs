using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SoundPulse : MonoBehaviour
{
    public float lifetime = 1.5f;
    public float maxRadius = 6f;
    public float growthSpeed = 8f;
    public float fadeSpeed = 1.5f;

    private Light2D pulseLight;
    private float currentIntensity;

    void Start()
    {
        pulseLight = GetComponent<Light2D>();

        if (pulseLight != null)
        {
            pulseLight.pointLightOuterRadius = 0.5f;
            pulseLight.pointLightInnerRadius = 0f;
            currentIntensity = pulseLight.intensity;
        }
    }

    void Update()
    {
        if (pulseLight != null)
        {
            // Expand the light radius
            pulseLight.pointLightOuterRadius += growthSpeed * Time.deltaTime;

            // Fade the light intensity
            pulseLight.intensity -= fadeSpeed * Time.deltaTime;

            if (pulseLight.pointLightOuterRadius >= maxRadius || pulseLight.intensity <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject, lifetime);
        }
    }
}