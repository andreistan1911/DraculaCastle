using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Vector3 startingPosition;

    public void Awake()
    {
        startingPosition = transform.position;
        GlobalValues.EnemiesAwaken++;
    }

    public virtual void RestartPosition()
    {
        transform.position = startingPosition;
    }
}
