using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverProximityController : MonoBehaviour
{
    private LeverController leverController; // reference to the lever controller

    private void Start()
    {
        // Find the LeverController in the sibling GameObjects
        leverController = transform.parent.GetComponentInChildren<LeverController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            leverController.SetPlayerNearby(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            leverController.SetPlayerNearby(false);
    }
}
