using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    public float speed;
    public float upLimit;
    public float downLimit;

    private Vector3 upLimitVec, downLimitVec;
    private bool movingUp;

    private Transform oldParent;

    private void Start()
    {
        movingUp = true;
        upLimitVec = new Vector3(transform.position.x, upLimit, transform.position.z);
        downLimitVec = new Vector3(transform.position.x, downLimit, transform.position.z);
    }

    private void Update()
    {
        if (transform.position == upLimitVec || transform.position == downLimitVec)
            movingUp = !movingUp;

        if (movingUp)
            transform.position = Vector3.MoveTowards(transform.position, upLimitVec, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, downLimitVec, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            oldParent = collision.transform.parent;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(oldParent);
    }
}
