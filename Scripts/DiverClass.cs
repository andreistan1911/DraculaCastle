using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverClass : ShapeClass
{
    new void Start()
    {
        base.Start();
    }

    // TODO: fix animations

    public override void Crouch()
    {
    }

    public override void Roll()
    {
    }

    public override void Jump()
    {

    }

    public override void StopCrouch()
    {
    }

    public override void StopRoll()
    {
    }

    public override void Walk(float value)
    {
        anim.SetBool("isWalking", value > 0.1);
    }

    public override void Run(float value)
    {
        anim.SetBool("isRunning", value > 0.1);
    }

    public override void SetGrounded(bool value)
    {
    }
}
