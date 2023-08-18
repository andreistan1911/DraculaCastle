using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeClass : MonoBehaviour
{
    public Vector3 colliderCenterDefault;
    public Vector3 colliderSizeDefault;
    public Vector3 colliderCenterCrouch;
    public Vector3 colliderSizeCrouch;

    public float runSpeed;
    public float walkSpeed;
    public float rollSpeed;
    public float crouchWalkSpeed;
    public float climbSpeed;
    public float jumpForce;

    [HideInInspector]
    public Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Active(bool value)
    {
        gameObject.SetActive(value);
    }

    public abstract void Crouch();
    public abstract void Roll();
    public abstract void Jump();
    public abstract void StopCrouch();
    public abstract void StopRoll();
    public abstract void Walk(float value);
    public abstract void Run(float value);
    public abstract void SetGrounded(bool value);
}
