using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSphereController : Hazard
{
    public ParticleSystem collisionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        PlayCollisionEffect(collision.contacts[0].point);
        Destroy(gameObject);
    }

    private void PlayCollisionEffect(Vector3 position)
    {
        ParticleSystem effectInstance = Instantiate(collisionEffectPrefab, position, Quaternion.identity);
        effectInstance.Play();
        Destroy(effectInstance.gameObject, effectInstance.main.duration);
    }
}
