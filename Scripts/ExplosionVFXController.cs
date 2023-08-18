using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFXController : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        Destroy(gameObject, GetMaxDuration());
    }

    private float GetMaxDuration()
    {
        float maxDuration = 0f;

        foreach (ParticleSystem ps in particleSystems)
        {
            float duration = ps.main.duration + ps.main.startLifetime.constantMax;

            if (duration > maxDuration)
                maxDuration = duration;
        }

        return maxDuration;
    }
}
