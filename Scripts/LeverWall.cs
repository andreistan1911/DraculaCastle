using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverWall : LeverController
{
    public GameObject movingWall;

    private void Update()
    {
        if (isPlayerNearby && !hasBeenUsed && Input.GetKeyDown(interactKey))
        {
            hasBeenUsed = true;
            StartCoroutine(RotateLever());
            Destroy(movingWall);
        }
    }
}
