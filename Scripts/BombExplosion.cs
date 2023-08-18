using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : Hazard
{
    private float maxRadius;
    private float sphereExpandSpeed;

    public void Setup(float maxRadius, float sphereExpandSpeed)
    {
        this.maxRadius = maxRadius;
        this.sphereExpandSpeed = sphereExpandSpeed;
    }

    public IEnumerator ExpandSphereAndDestroy(GameObject sphere)
    {
        //Debug.Log("Starting sphere expansion");
        while (sphere.transform.localScale.x < maxRadius)
        {
            sphere.transform.localScale += Vector3.one * sphereExpandSpeed * Time.deltaTime;
            //Debug.Log(sphere.transform.localScale.x);
            yield return null;
        }
        //Debug.Log("Sphere expansion complete");

        Destroy(sphere);
    }
}
