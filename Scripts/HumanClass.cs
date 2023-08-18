using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanClass : ShapeClass
{
    new void Start()
    {
        base.Start();
    }

    public override void Crouch()
    {
        anim.SetBool("crouch", true);
    }

    public override void Roll()
    {
        anim.SetBool("roll", true);
    }

    public override void Jump()
    {

    }

    public override void StopCrouch()
    {
        anim.SetBool("crouch", false);
    }

    public override void StopRoll()
    {
        anim.SetBool("roll", false);
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
        anim.SetBool("grounded", value);
    }
}
