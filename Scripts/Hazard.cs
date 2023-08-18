using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    public void Delete()
    {
        Destroy(gameObject);
    }
}
