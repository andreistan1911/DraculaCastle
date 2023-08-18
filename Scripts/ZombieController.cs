using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : Enemy
{
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    public float walkingSpeed = 1.0f;
    public float rotationSpeed = 180.0f;
    public float idleTime = 1.0f;

    private Vector3 targetPosition;
    private Animator animator;

    new private void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        targetPosition = leftPosition;
        SetAnimationState(true);
    }

    private void Update()
    {
        if (!animator.GetBool("IsWalking")) return;

        float step = walkingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(IdleAndTurn());
        }

    }

    IEnumerator IdleAndTurn()
    {
        SetAnimationState(false);
        yield return new WaitForSeconds(idleTime);

        // Rotate 180 degrees
        float targetRotation = transform.eulerAngles.y + 180.0f;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation)) > 0.1f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, angle, 0);

            yield return null;
        }


        // Set the new target position
        targetPosition = targetPosition == leftPosition ? rightPosition : leftPosition;
        SetAnimationState(true);
    }

    private void SetAnimationState(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }

}
