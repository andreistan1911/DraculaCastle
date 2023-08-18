using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverLaserController : LeverController
{
    public GameObject laserPrefab;
    public Transform laserSpawn;

    private void Update()
    {
        if (isPlayerNearby && !hasBeenUsed && Input.GetKeyDown(interactKey))
        {
            hasBeenUsed = true;
            StartCoroutine(RotateLever());
            Instantiate(laserPrefab, laserSpawn.position, Quaternion.identity);
        }
    }
}
