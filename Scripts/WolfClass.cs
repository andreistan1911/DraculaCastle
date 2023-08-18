using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfClass : ShapeClass
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
        anim.SetFloat("walk", value);
    }

    public override void Run(float value)
    {
        anim.SetFloat("run", value);
    }

    public override void SetGrounded(bool value)
    {
    }
}
