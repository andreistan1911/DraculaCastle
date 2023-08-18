using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverElectricController : LeverController
{
    public GameObject lightningPrefab;
    public float minLightningSpawnTime;
    public float maxLightningSpawnTime;
    public float lightningSpeed = 6f;
    public Vector3 direction = new(0, -0.25f, 1);
    public GameObject spawnPointsParent;

    private List<Transform> spawnPoints = new();
    private List<GameObject> activeLightnings = new();

    private const float MAX_Z_DEPTH = 14f;

    private new void Start()
    {
        base.Start();

        foreach (Transform child in spawnPointsParent.transform)
            spawnPoints.Add(child);
    }

    private void Update()
    {
        MoveLightnings();

        if (isPlayerNearby && !hasBeenUsed && Input.GetKeyDown(interactKey))
        {
            hasBeenUsed = true;

            StartCoroutine(RotateLever());
            StartCoroutine(SpawnLightningBolts());
        }
    }

    private IEnumerator SpawnLightningBolts()
    {
        while (hasBeenUsed)
        {
            yield return new WaitForSeconds(Random.Range(minLightningSpawnTime, maxLightningSpawnTime));

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject lightning = Instantiate(lightningPrefab, spawnPoint.position, Quaternion.Euler(0, 90, 76));

            activeLightnings.Add(lightning);
        }
    }

    private void MoveLightnings()
    {
        for (int i = activeLightnings.Count - 1; i >= 0; i--)
        {
            GameObject lightning = activeLightnings[i];


            lightning.transform.position += lightningSpeed * Time.deltaTime * direction.normalized;
            
            
            if (lightning.transform.position.z >= MAX_Z_DEPTH)
            {
                Destroy(lightning);
                activeLightnings.RemoveAt(i);
            }
        }
    }
}
