using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBallController : MonoBehaviour
{
    private Vector3 endPosition;
    private float swipeSpeed;

    public void Setup(Vector3 endPosition, float swipeSpeed)
    {
        this.endPosition = endPosition;
        this.swipeSpeed = swipeSpeed;
    }

    private void Start()
    {
        StartCoroutine(MoveCube());
    }

    IEnumerator MoveCube()
    {
        float swipeDistance = 0;
        Vector3 startPosition = transform.position;

        while (swipeDistance < 1)
        {
            swipeDistance += Time.deltaTime * swipeSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, swipeDistance);
            yield return null;
        }

        Destroy(gameObject);
    }
}
