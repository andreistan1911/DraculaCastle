using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : Hazard
{
    public GameObject explosionPrefab;
    public Material sphereMaterial;
    public float maxRadius;
    public float sphereExpandSpeed;

    private Transform player;
    private Rigidbody rb;
    private MeshRenderer mr;

    private float horizontalSpeed;
    private float explosionDelay;
    private Color defaultColor;
    private Color lightRed;

    private GameObject bombChild;
    private GameObject particleChild;
    private ParticleSystem[] particles;

    public void Setup(Transform player, float horizontalSpeed, float explosionDelay, Color defaultColor, Color lightRed)
    {
        this.player = player;
        this.horizontalSpeed = horizontalSpeed;
        this.explosionDelay = explosionDelay;
        this.defaultColor = defaultColor;
        this.lightRed = lightRed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bombChild = transform.Find("bomb").gameObject;
        mr = bombChild.GetComponent<MeshRenderer>();
        mr.material.color = defaultColor;
        particleChild = transform.Find("particle").gameObject;
        particles = particleChild.GetComponentsInChildren<ParticleSystem>();

        StartCoroutine(ChangeColorAndExplode());
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector3(direction * horizontalSpeed, rb.velocity.y, rb.velocity.z);
    }

    IEnumerator ChangeColorAndExplode()
    {
        float elapsedTime = 0f;

        while (elapsedTime < explosionDelay)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / explosionDelay;

            mr.material.color = Color.Lerp(defaultColor, lightRed, t);
            yield return null;
        }

        Explode();
    }

    void Explode()
    {
        // Instantiate the explosion particle effect
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Create the expanding sphere
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
        sphere.transform.localScale = Vector3.zero;
        sphere.GetComponent<MeshRenderer>().material = sphereMaterial;
        sphere.GetComponent<MeshRenderer>().material.color = new Color(1, 0.5f, 0, 0.3f);

        // Add a tag to the sphere
        sphere.tag = "death";

        // Set the SphereCollider component to be a trigger
        sphere.GetComponent<SphereCollider>().isTrigger = true;

        // Set the Bomb Exlosion
        BombExplosion bombExplosion = sphere.AddComponent<BombExplosion>();
        bombExplosion.Setup(maxRadius, sphereExpandSpeed);

        StartCoroutine(bombExplosion.ExpandSphereAndDestroy(sphere));

        // Destroy the bomb
        float expandTime = maxRadius / sphereExpandSpeed;
        StartCoroutine(DestroyAfterDelay(expandTime));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        mr.enabled = false;
        Destroy(GetComponent<SphereCollider>());
        foreach (ParticleSystem particle in particles)
            particle.Stop();

        yield return new WaitForSeconds(delay + 1);

        Destroy(gameObject);
    }
}
