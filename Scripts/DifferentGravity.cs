using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentGravity : MonoBehaviour
{
    public Vector3 gravity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GlobalValues.eyeShouldChase = true;
            Physics.gravity = gravity;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        GlobalValues.eyeShouldChase = false;
        Physics.gravity = GlobalValues.standardGravity;
    }
}
