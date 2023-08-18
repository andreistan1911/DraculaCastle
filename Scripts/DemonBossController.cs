using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonBossController : Enemy
{
    public float groundSlamFrequency = 5f;
    public float platformSwipeFrequency = 10f;
    public float fallingDebrisFrequency = 7f;

    public float floatAmplitude = 0.4f;         // the maximum distance to float up or down
    public float floatSpeed = 1.5f;             // how fast to float up or down
    public float followSpeed = 0.7f;

    public Color electricDamageColor = Color.blue;  // the color to flash when damaged by lightning bolt
    public Color laserDamageColor = Color.red;      // the color to flash when damaged by laser
    public Color debrisDamageColor = Color.green;   // the color to flash when damaged by debris

    public GameObject lightningBolt;        // the lightning bolt to instantiate on lever pull
    public GameObject burningFire;          // the fire vfx to instantiate on lever pull
    public float flashDuration = 0.3f;      // the duration of each flash in seconds

    public GameObject explosionPrefab;

    //

    public float upwardAttackForce = 500.0f;

    public GameObject swipeObjectPrefab;
    public float swipeSpeedBoss = 1.25f;
    public float lavaballSpeed = 0.5f;
    public float platformSwipeDistance = 1f;
    public float spawnOffset = 1f;

    public GameObject[] debrisPrefabs;
    public GameObject debrisSpawnPointsParent;
    public Material blackMaterial;
    public float debrisFallSpeed = .00001f;
    public int debrisSpawnCount = 3;
    public float minDebrisInterval = .5f;
    public float maxDebrisInterval = 1.5f;
    public float shakeDuration = .5f;
    public float shakeMagnitude = .1f;

    //

    public GameObject keyPrefab;
    public Transform keySpawn;

    //

    private Animator animator;
    private Rigidbody rb;
    private Transform player;
    private CameraController cameraController;
    private List<Transform> debrisSpawnPoints;

    private float groundSlamTimer;
    private float platformSwipeTimer;
    private float fallingDebrisTimer;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Color originalColor;
    private Vector3 startPosition;

    private Vector3 crHp;
    private int runningCoroutines;
    private bool hitDebris;
    private bool dead;
    private GameComplete gameComplete;
    private GameObject cloud;

    //private LeverController laserLever;
    //private LeverController electricLever;

    public Vector3 CrHP => crHp;

    new private void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cloud = GameObject.Find("Cloud");
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        gameComplete = GameObject.Find("gamecomplete").GetComponent<GameComplete>();
        //electricLever = GameObject.Find("Electric Lever").transform.Find("Lever").GetComponent<LeverController>();
        //laserLever = GameObject.Find("Laser Lever").transform.Find("Lever").GetComponent<LeverController>();
        groundSlamTimer = groundSlamFrequency;
        platformSwipeTimer = platformSwipeFrequency;
        fallingDebrisTimer = fallingDebrisFrequency;

        debrisSpawnPoints = new List<Transform>();
        foreach (Transform child in debrisSpawnPointsParent.transform)
            debrisSpawnPoints.Add(child);

        // Get the SkinnedMeshRenderer of the child named "LP.original"
        skinnedMeshRenderer = transform.Find("LP.original").GetComponent<SkinnedMeshRenderer>();

        // Store the original color
        originalColor = skinnedMeshRenderer.material.color;

        startPosition = transform.position;

        crHp = new Vector3(2, 2, 2);
        runningCoroutines = 0;
        hitDebris = false;
        dead = false;
    }

    private void Update()
    {
        FollowPlayer();
        FloatMovement();
        Attack();

        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (crHp.x > 0 || crHp.y > 0 || crHp.z > 0 || dead)
            return;

        dead = true;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        groundSlamTimer = int.MaxValue;
        platformSwipeTimer = int.MaxValue;
        fallingDebrisTimer = int.MaxValue;

        while (runningCoroutines > 0)
            yield return null;

        GameObject explosion = Instantiate(explosionPrefab, transform.position + new Vector3(1, 5.5f, -0.5f), Quaternion.identity);
        ParticleSystem[] particleSystems = explosion.GetComponentsInChildren<ParticleSystem>();

        Destroy(cloud);
        skinnedMeshRenderer.enabled = false;

        while (true)
        {
            bool isPlaying = false;
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem.isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }

            if (!isPlaying)
                break;

            yield return null;
        }

        Destroy(explosion);
        Instantiate(keyPrefab, keySpawn.position, Quaternion.Euler(90, 0, 0));
        //yield return gameComplete.FadeInImage(); // Fade in image
    }

    private void FollowPlayer()
    {
        // Calculate the direction vector for horizontal movement only
        Vector3 horizontalDirection = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;

        // Normalize the direction vector
        horizontalDirection.Normalize();

        Vector3 verticalDirection = new Vector3(transform.position.x, player.position.y, player.position.z) - transform.position;

        // Normalize the direction vector
        verticalDirection.Normalize();

        // Create the movement vector
        Vector3 moveVector = new(horizontalDirection.x * followSpeed, verticalDirection.y * followSpeed * 2000 + rb.velocity.y, 0);

        rb.velocity = moveVector;
    }

    private void FloatMovement()
    {
        // Calculate new Y position for floating movement
        float floatY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Calculate target Y position, which is a blend of the player's Y position and the floating Y position
        float targetY = Mathf.Lerp(player.position.y, floatY, 0.5f);

        // Smoothly interpolate the boss's Y position towards the target Y position
        Vector3 currentPosition = transform.position;
        currentPosition.y = Mathf.Lerp(currentPosition.y, targetY, .5f);
        transform.position = currentPosition;

        // Maintain the boss's X and Z velocities
        rb.velocity = new Vector3(rb.velocity.x, (transform.position.y - currentPosition.y) / Time.deltaTime, 0);
    }

    private void Attack()
    {
        groundSlamTimer -= Time.deltaTime;
        platformSwipeTimer -= Time.deltaTime;
        fallingDebrisTimer -= Time.deltaTime;

        if (groundSlamTimer <= 0)
        {
            GroundSlam();
            groundSlamTimer = groundSlamFrequency;
        }

        if (platformSwipeTimer <= 0)
        {
            StartCoroutine(PlatformSwipe());
            platformSwipeTimer = platformSwipeFrequency;
        }

        if (fallingDebrisTimer <= 0)
        {
            StartCoroutine(SpawnFallingDebris());
            fallingDebrisTimer = fallingDebrisFrequency;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("death"))
            StartCoroutine(DebrisChangeColorNTimes(GlobalValues.nrHitFlashes));

        if (other.gameObject.CompareTag("bolt"))
            StartCoroutine(ElectricChangeColorNTimes(GlobalValues.nrHitFlashes));
    }

    private void GroundSlam()
    {
        animator.SetTrigger("GroundSlam");

        PlayerClass playerClass = player.GetComponent<PlayerClass>();

        if (playerClass.IsGrounded())
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();

            rb.AddForce(Vector3.up * upwardAttackForce, ForceMode.Impulse);
        }
    }

    private IEnumerator PlatformSwipe()
    {
        animator.SetTrigger("PlatformSwipe");

        float swipeDistance = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new(
            startPosition.x + (player.position.x > transform.position.x ? platformSwipeDistance : -platformSwipeDistance),
            startPosition.y,
            startPosition.z
        );

        // Calculate the spawn position for the spawn position for the swipe cube
        Vector3 lavaballSpawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
            player.position.x > transform.position.x ? -spawnOffset : 1 + spawnOffset,
            0.55f,
            -Camera.main.transform.position.z
            ));
        lavaballSpawnPosition.z = 0;

        Vector3 lavaballEndPosition = Camera.main.ViewportToWorldPoint(new Vector3(
            player.position.x > transform.position.x ? 1 : 0,
            0.55f,
            -Camera.main.transform.position.z
            ));
        lavaballEndPosition.z = 0;

        // Debug.Log(cubeSpawnPosition + " " + cubeEndPosition);

        // Calculate the end position for the swipe cube at the edge of the screen
        GameObject swipeCube = Instantiate(swipeObjectPrefab, lavaballSpawnPosition, Quaternion.identity);
        LavaBallController lavaBallController = swipeCube.GetComponent<LavaBallController>();
        lavaBallController.Setup(lavaballEndPosition, lavaballSpeed);

        while (swipeDistance < 1)
        {
            swipeDistance += Time.deltaTime * swipeSpeedBoss;
            transform.position = Vector3.Lerp(startPosition, endPosition, swipeDistance);
            yield return null;
        }
    }

    private IEnumerator SpawnFallingDebris()
    {
        StartCoroutine(cameraController.Shake(shakeDuration, shakeMagnitude));

        for (int i = 0; i < debrisSpawnCount; ++i)
        {
            Transform spawnPoint = debrisSpawnPoints[Random.Range(0, debrisSpawnPoints.Count - 1)];
            GameObject debris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], spawnPoint.position, Quaternion.Euler(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            ));

            Rigidbody2D debrisRB = debris.GetComponent<Rigidbody2D>();
            debrisRB.velocity = new Vector2(0, -debrisFallSpeed);

            // Create a new cylinder at the desired location and orientation
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Destroy(cylinder.GetComponent<Collider>());
            cylinder.transform.position = new Vector3(spawnPoint.position.x, 12, spawnPoint.position.z);
            cylinder.transform.localScale = new Vector3(1, 15, 1); 
            cylinder.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Set the cylinder's material and adjust its alpha
            Material cylinderMaterial = new Material(blackMaterial);
            Color cylinderColor = cylinderMaterial.color;
            cylinderColor.a = 0.2f;
            cylinderMaterial.color = cylinderColor;
            cylinder.GetComponent<Renderer>().material = cylinderMaterial;

            Destroy(cylinder, 5f);
            Destroy(debris, 5f);

            yield return new WaitForSeconds(Random.Range(minDebrisInterval, maxDebrisInterval));
        }


        // Boss plane debris

        GameObject extraDebris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], debrisSpawnPoints[^1].position,
            Quaternion.Euler(
                Random.Range(0, 360),
                Random.Range(0, 360),
                Random.Range(0, 360)
            ));

        GameObject extraCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(extraCylinder.GetComponent<Collider>());
        extraCylinder.transform.position = new Vector3(debrisSpawnPoints[^1].position.x, 12, debrisSpawnPoints[^1].position.z);
        extraCylinder.transform.localScale = new Vector3(1, 15, 1);
        extraCylinder.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Set the cylinder's material and adjust its alpha
        Material extraCylinderMaterial = new Material(blackMaterial);
        Color extraCylinderColor = extraCylinderMaterial.color;
        extraCylinderColor.a = 0.2f;
        extraCylinderMaterial.color = extraCylinderColor;
        extraCylinder.GetComponent<Renderer>().material = extraCylinderMaterial;

        Rigidbody2D extraDebrisRB = extraDebris.GetComponent<Rigidbody2D>();
        extraDebrisRB.velocity = new Vector2(0, -debrisFallSpeed);

        Destroy(extraCylinder, 5f);
        Destroy(extraDebris, 5f);

        //
    }

    public IEnumerator ElectricChangeColorNTimes(int nrTimes)
    {
        runningCoroutines++;
        crHp.z--;

        // Instantiate the lightning bolt at the boss' position
        GameObject bolt = Instantiate(
            lightningBolt,
            transform.position + new Vector3(-1.1f, 6.3f, 0f),
            Quaternion.Euler(0, 0, 90)
            );

        // Scale the object
        bolt.transform.localScale = new Vector3(0.7f, 1f, 1f);

        // Make the lightning bolt a child of the boss
        bolt.transform.SetParent(this.transform);

        for (int i = 0; i < nrTimes; ++i)
        {
            // Change color to damageColor
            skinnedMeshRenderer.material.color = electricDamageColor;

            // Wait for flashDuration
            yield return new WaitForSeconds(flashDuration);

            // Change color back to the original color
            skinnedMeshRenderer.material.color = originalColor;

            // Wait for flashDuration seconds
            yield return new WaitForSeconds(flashDuration);
        }

        // Destroy the lightning bolt after the flashes have completed
        Destroy(bolt);
        runningCoroutines--;
    }

    public IEnumerator LaserChangeColorNTimes(int nrTimes)
    {
        runningCoroutines++;
        crHp.x--;

        // Instantiate the lightning bolt at the boss' position
        GameObject fire = Instantiate(
            burningFire,
            transform.position + new Vector3(1, 5.5f, -0.5f),
            Quaternion.identity
            );

        // Make the lightning bolt a child of the boss
        fire.transform.SetParent(this.transform);

        for (int i = 0; i < nrTimes; ++i)
        {
            // Change color to damageColor
            skinnedMeshRenderer.material.color = laserDamageColor;

            // Wait for flashDuration
            yield return new WaitForSeconds(flashDuration);

            // Change color back to the original color
            skinnedMeshRenderer.material.color = originalColor;

            // Wait for flashDuration seconds
            yield return new WaitForSeconds(flashDuration);
        }

        Destroy(fire);
        runningCoroutines--;
    }

    public IEnumerator DebrisChangeColorNTimes(int nrTimes)
    {
        if (!hitDebris)
        {
            //hitDebris = true;
            crHp.y--;
        }
        runningCoroutines++;

        for (int i = 0; i < nrTimes; ++i)
        {
            // Change color to damageColor
            skinnedMeshRenderer.material.color = debrisDamageColor;

            // Wait for flashDuration
            yield return new WaitForSeconds(flashDuration);

            // Change color back to the original color
            skinnedMeshRenderer.material.color = originalColor;

            // Wait for flashDuration seconds
            yield return new WaitForSeconds(flashDuration);
        }

        runningCoroutines--;
    }

    public override void RestartPosition()
    {
        base.RestartPosition();

        //crHp = new(2, 2, 2);
        runningCoroutines = 0;
        //hitDebris = false;
        dead = false;

        //laserLever.Restart();
        //electricLever.Restart();
    }
}
