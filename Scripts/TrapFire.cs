using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    public float timeBurning;
    public float timeNotBurning;
    public bool burnAtStart;

    private float minIntensity = 1000f;
    private float maxIntensity = 150000f;
    private const float alpha = .8f;
    private float intensityAfterSlowIncrease;

    private new ParticleSystem particleSystem;
    private Light pointLight;

    private float timeElapsed;
    private float timeSlowIncrease;
    private bool burning;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        pointLight = GetComponentInChildren<Light>();

        timeElapsed = 0f;
        timeSlowIncrease = .5f * timeNotBurning;
        burning = burnAtStart;
        pointLight.intensity = minIntensity;
        intensityAfterSlowIncrease = minIntensity + alpha * timeSlowIncrease;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (!burning)
        {
            if (timeElapsed < timeNotBurning)
            {
                if (timeElapsed < timeSlowIncrease)
                {
                    float normalizedTime = timeElapsed / timeSlowIncrease;
                    pointLight.intensity = Mathf.Lerp(minIntensity, intensityAfterSlowIncrease, normalizedTime);
                }
                else
                {
                    float normalizedTime = (timeElapsed - timeSlowIncrease) / timeNotBurning;
                    pointLight.intensity = Mathf.Lerp(intensityAfterSlowIncrease, maxIntensity, normalizedTime);
                }
            }
            else
            {
                timeElapsed = 0f;
                particleSystem.Play();
                burning = true;
            }
        }
        else
        {
            if (timeElapsed >= timeBurning)
            {
                timeElapsed = 0f;
                particleSystem.Stop();
                burning = false;
                pointLight.intensity = minIntensity;
            }
        }
        
        tag = "Untagged";
    }

    private void OnTriggerStay(Collider other)
    {
        if (burning && other.gameObject.tag == "Player")
        {
            tag = "death";
        }
    }
}
