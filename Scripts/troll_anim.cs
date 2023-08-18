using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troll_anim : Enemy
{
    private PlayerClass playerScript;
    private Animator anim;

    private float lastTimer = 0.0f;
    private float waitTimeAttack1 = 4.0f;
    private float threatDistance = 5.0f;

    new private void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
    }

    private void Update()
    {

        if (Vector3.Distance(playerScript.Rb.position, transform.position) < threatDistance)
        {
            if (Time.time - lastTimer > waitTimeAttack1)
            {
                anim.SetBool("attack1", true);
                lastTimer = Time.time;
            }
            else
            {
                anim.SetBool("attack1", false);
            }
        }
    }
}
