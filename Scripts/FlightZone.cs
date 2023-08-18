using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GlobalValues.dragonAvailable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GlobalValues.dragonAvailable = false;
    }
}
