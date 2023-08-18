using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMonster : Enemy
{
    public float speed = 5.0f;
    public float chargeDistance = 10.0f;
    public float yTolerance = 2f;
    public float waitTime = 5.0f;
    public float turnTime = 2.0f;

    private GameObject player;
    private bool isWaiting = false;
    private bool isCharging = false;
    private bool hasCharged = false;
    private Vector3 chargeDirection;
    private Vector3 chargeStartPosition;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && !isWaiting && !isCharging && !hasCharged)
        {
            float distance_x = Mathf.Abs(transform.position.x - player.transform.position.x);
            float distance_y = Mathf.Abs(transform.position.y - player.transform.position.y);

            if (distance_x < chargeDistance && distance_y < yTolerance)
            {
                // start charging
                isCharging = true;
                chargeStartPosition = transform.position;

                // Determine direction
                chargeDirection = player.transform.position.x > transform.position.x ? Vector3.right : Vector3.left;

                // Update rotation based on charge direction
                if (chargeDirection == Vector3.right)
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                else
                    transform.rotation = Quaternion.Euler(0, -90, 0);

                // Activate walking animation
                animator.SetTrigger("Walk_Cycle_1");
            }
        }

        if (isCharging)
        {
            transform.position += chargeDirection * speed * Time.deltaTime;
            if (Vector3.Distance(chargeStartPosition, transform.position) >= chargeDistance)
            {
                isCharging = false;
                hasCharged = true;
                StartCoroutine(WaitAfterCharge(true));
            }
        }
    }

    IEnumerator TurnAround()
    {
        // Activate turning animation
        animator.SetTrigger("Walk_Cycle_2");

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        float elapsedTime = 0;

        while (elapsedTime < turnTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / turnTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;

        // Activate idle animation
        animator.SetTrigger("Fight_Idle_1");

        StartCoroutine(WaitAfterCharge());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Assuming the wall has the tag "Wall" or "ladder"
        if ((collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("ladder")) && isCharging)
        {
            isCharging = false;
            hasCharged = true;
            StartCoroutine(TurnAround());
        }
    }

    IEnumerator WaitAfterCharge(bool reachedMaxDistance = false)
    {
        isWaiting = true;
        // If it reached max distance without colliding with a wall or ladder, activate Fight_Idle_1
        if (reachedMaxDistance)
        {
            animator.SetTrigger("Fight_Idle_1");
        }
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        hasCharged = false;
    }
}
