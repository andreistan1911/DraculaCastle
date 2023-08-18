using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGirlScript : Enemy
{
    public float attackInterval;
    public float projectileSpeed;
    public GameObject deathSpherePrefab;
    public ParticleSystem collisionEffectPrefab;

    private Animator animator;

    new private void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine());
    }
    
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            animator.SetTrigger("Attack");
            StartCoroutine(SpawnDeathSphereAfterDelay(0.5f));
        }
    }

    private IEnumerator SpawnDeathSphereAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnDeathSphere();
    }

    private void SpawnDeathSphere()
    {
        Vector3 spawnOffset = new Vector3(-.75f, 1f, 0f);
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        GameObject deathSphere = Instantiate(deathSpherePrefab, transform.position + spawnOffset, rotation);
        deathSphere.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 0) * projectileSpeed;
        deathSphere.tag = "death";
        DeathSphereController controller = deathSphere.AddComponent<DeathSphereController>();
        controller.collisionEffectPrefab = collisionEffectPrefab;
    }
    
}
