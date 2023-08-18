using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovingPlatform : MonoBehaviour
{
    public float speed;
    public float leftLimit;
    public float rightLimit;

    private Vector3 leftLimitVec, rightLimitVec;
    private bool movingRight;

    private Transform oldParent;

    private void Start()
    {
        movingRight = true;
        leftLimitVec = new Vector3(leftLimit, transform.position.y, transform.position.z);
        rightLimitVec = new Vector3(rightLimit, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (transform.position == leftLimitVec || transform.position == rightLimitVec)
            movingRight = !movingRight;

        if (movingRight)
            transform.position = Vector3.MoveTowards(transform.position, rightLimitVec, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, leftLimitVec, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            oldParent = collision.transform.parent;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(oldParent);
        }
    }
}
