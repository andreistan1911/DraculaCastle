using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGirlLightning : MonoBehaviour
{
    public GameObject lightningPrefab;    // the prefab for the lightning bolt
    public GameObject circlePrefab;       // the prefab for the circle on the ground
    public int nrBolts = 3;
    public float spawnInterval = 5.0f;    // the interval between lightning bolt spawns
    public float minSpawnDistance = 2.0f; // the minimum distance between spawned bolts
    public float spawnHeight = 10.0f;     // the height from where the bolts spawn
    public Vector2 spawnRange;            // the x range in which bolts can spawn

    private float timer;
    private List<Vector3> previousPositions = new();
    private int boltCounter = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && boltCounter < nrBolts)
        {
            StartCoroutine(SpawnBolts());
            timer = spawnInterval;
        }
    }

    IEnumerator SpawnBolts()
    {
        for (int i = 0; i < nrBolts; i++)
        {
            Vector3 position = new(Random.Range(spawnRange.x, spawnRange.y), spawnHeight, 0);

            // Check minimum distance
            bool validPosition = false;
            while (!validPosition)
            {
                validPosition = true;
                foreach (Vector3 prevPos in previousPositions)
                {
                    if (Vector3.Distance(prevPos, position) < minSpawnDistance)
                    {
                        validPosition = false;
                        position = new Vector3(Random.Range(spawnRange.x, spawnRange.y), spawnHeight, 0);
                        break;
                    }
                }
            }

            previousPositions.Add(position);
            StartCoroutine(SpawnLightning(position));
            animator.SetTrigger("CastBolt");

            // Increment the counter as we're about to spawn a new bolt
            boltCounter++;

            // Time delay between bolts
            yield return new WaitForSeconds(1);
        }

        // Clear previous positions for next round of bolts
        previousPositions.Clear();
    }

    IEnumerator SpawnLightning(Vector3 position)
    {
        // Compensate for rotation with offset
        GameObject bolt = Instantiate(lightningPrefab, position - new Vector3(2f, 0, 0), Quaternion.Euler(0, 0, 90));
        GameObject circle = Instantiate(circlePrefab, new Vector3(position.x, transform.position.y, position.z), Quaternion.identity);

        // Add "death" tag
        bolt.tag = "death";

        while (bolt.transform.position.y > transform.position.y)
        {
            bolt.transform.position += Vector3.down * Time.deltaTime;  // Adjust speed as needed
            yield return null;
        }

        Destroy(bolt);
        Destroy(circle);

        // Decrement the counter as we're about to destroy a bolt
        boltCounter--;
    }

}
