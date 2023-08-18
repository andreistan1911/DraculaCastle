using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeball : Enemy
{
    public float speed = .5f;
    public float bombDropInterval = 2f;
    public GameObject bombPrefab;
    public Color defaultBombColor = Color.white;
    public Color lightRed = Color.red;
    public float bombHorizontalSpeed = .75f;
    public float bombExplosionDelay = 3f;

    private Transform player;
    private BoxCollider playerCollider;
    private Rigidbody rb;

    private float lastBombDropTime;

    new private void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        lastBombDropTime = Time.time;
        gameObject.tag = "death";
    }

    private void Update()
    {
        if (GlobalValues.eyeShouldChase)
            FollowPlayer();
        RotateTowardsPlayer();
        DropBomb();
    }

    private void FollowPlayer()
    {
        Vector3 playerTop = player.position + new Vector3(0, playerCollider.bounds.extents.y, 0);
        Vector3 direction = (playerTop - transform.position).normalized;
        direction.z = 0; 
        rb.velocity = direction * speed;
    }

    private void RotateTowardsPlayer()
    {
        transform.LookAt(player);
    }

    private void DropBomb()
    {
        if (Time.time >= lastBombDropTime + bombDropInterval)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            BombController bombController = bomb.GetComponent<BombController>();
            bombController.Setup(player, bombHorizontalSpeed, bombExplosionDelay, defaultBombColor, lightRed);

            lastBombDropTime = Time.time;
        }
    }
}
