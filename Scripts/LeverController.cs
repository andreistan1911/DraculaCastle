using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeverController : MonoBehaviour
{
    public float rotationAmount = 50f;  // the amount to rotate the lever in degrees
    public float rotationTime = .3f;     // the time in which the lever will rotate

    [HideInInspector]
    public KeyCode interactKey;        // the key to interact with the lever

    [HideInInspector]
    public bool isPlayerNearby;        // whether the player is close enough to interact

    [HideInInspector]
    public bool hasBeenUsed;           // whether the lever has already been used

    [HideInInspector]
    public DemonBossController boss;   // reference to the boss

    private Quaternion startRotation;
    private Quaternion endRotation;

    public void Start()
    {
        interactKey = GlobalValues.interactKey;
        isPlayerNearby = false;
        hasBeenUsed = false;

        // Set the start and end rotations;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationAmount);

        // Find the boss in the scene
        boss = FindObjectOfType<DemonBossController>();
    }

    public IEnumerator RotateLever()
    {
        float t = 0f;
        while (t < rotationTime)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / rotationTime);
            yield return null;
        }

        // Ensure that the lever ends up exactly at the target rotation
        transform.rotation = endRotation;
    }

    public void SetPlayerNearby(bool value)
    {
        isPlayerNearby = value;
    }

    public void Restart()
    {
        hasBeenUsed = false;
        transform.rotation = startRotation;
    }
}
