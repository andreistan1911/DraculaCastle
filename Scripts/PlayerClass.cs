using System.Collections;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public GameObject shapeshiftParticle;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private LevelClass level;

    private HumanClass human;
    private WolfClass wolf;
    private DragonClass dragon;
    private DiverClass diver;
    private ShapeClass shape;
    private ShapeClass[] shapes;

    private bool facingRight;
    private bool grounded;

    private int availableJumps;
    private bool isMovementEnabled;
    private const float rollTime = 1.0f;
    private int maxJumps = 2;
    private bool godMode;

    private float move, running, crouch, climb;
    private bool lastMove;
    private bool rolling = false;
    private bool canMoveObject, interactButton;

    private bool isChecking = false;

    private Vector3 startPosition;
    private RigidbodyConstraints defaultRBConstraints;

    public bool InteractButton => interactButton;
    public bool IsMovementEnabled => isMovementEnabled;
    public float WalkSpeed => shape.walkSpeed;
    public Rigidbody Rb => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        level = FindObjectOfType<LevelClass>();

        defaultRBConstraints = rb.constraints;

        facingRight = true;
        grounded = true;

        godMode = false;

        canMoveObject = false;
        interactButton = false;
        isMovementEnabled = true;
        lastMove = true;

        human = GetComponentInChildren<HumanClass>();
        wolf = GetComponentInChildren<WolfClass>();
        dragon = GetComponentInChildren<DragonClass>();
        diver = GetComponentInChildren<DiverClass>();
        shape = human;
        shapes = new ShapeClass[] { human, wolf, dragon, diver };
        wolf.Active(false);
        dragon.Active(false);
        diver.Active(false);

        startPosition = rb.position;
        availableJumps = maxJumps;
    }

    private void Update()
    {
        NoFlightCheck();
        JumpCheck();

        ShapeshiftCheck();

        if (!isChecking && (Input.GetKeyDown(GlobalValues.godModeKey1) || Input.GetKeyDown(GlobalValues.godModeKey2) || Input.GetKeyDown(GlobalValues.godModeKey3)))
        {
            isChecking = true;
            StartCoroutine(GodModeCheck());
        }
    }

    private void NoFlightCheck()
    {
        if (!GlobalValues.dragonAvailable && shape == dragon)
        {
            maxJumps = 2;
            Shapeshift(human);
        }
    }

    private IEnumerator GodModeCheck()
    {
        bool pressed_1 = false;
        bool pressed_2 = false;
        bool pressed_3 = false;

        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            if (Input.GetKey(GlobalValues.godModeKey1))
                pressed_1 = true;
            if (Input.GetKey(GlobalValues.godModeKey2))
                pressed_2 = true;
            if (Input.GetKey(GlobalValues.godModeKey3))
                pressed_3 = true;

            if (pressed_1 && pressed_2 && pressed_3)
            {
                godMode = !godMode;
                Debug.Log("GodMode: " + godMode);
                isChecking = false;
                yield break;
            }
        }

        isChecking = false;
    }

    private void ShapeshiftCheck()
    {
        if (shape == diver)
            return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            maxJumps = 2;
            Shapeshift(wolf);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            maxJumps = 2;
            Shapeshift(human);
        }
        else if (Input.GetKeyDown(KeyCode.J) && GlobalValues.dragonAvailable)
        {
            maxJumps = int.MaxValue;
            availableJumps = maxJumps;
            Shapeshift(dragon);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
            grounded = false;

        if (collision.gameObject.CompareTag("ladder"))
        {
            climb = 0;

            rb.constraints = defaultRBConstraints;
        }

        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ladder"))
        {
            climb = Input.GetAxis("Vertical");

            if (climb == 0)
                rb.constraints |= RigidbodyConstraints.FreezePositionY;
            else
                rb.constraints = defaultRBConstraints;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("a intrat pe " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("ground"))
        {
            grounded = true;
            availableJumps = maxJumps;
        }

        if (collision.gameObject.CompareTag("death"))
            Die();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("death"))
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("water"))
        {
            maxJumps = 2;
            Shapeshift(diver);
        }

        if (other.gameObject.CompareTag("bolt"))
            Die();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("water"))
            Shapeshift(human);
    }

    private void FixedUpdate()
    {
        WalkInput();
        RunInput();

        CrouchCheck();
        CrouchActions();

        MoveCharacterVertical();
        MoveCharacterHorizontal();

        InteractInput();

        level.CheckToIncreaseCrCheckpoint();

        FlipCheck();
        
    }

    private GameObject ActivateParticles()
    {
        GameObject particleInstance = Instantiate(shapeshiftParticle, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        particleInstance.SetActive(true);

        foreach (Transform child in particleInstance.transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }

        return particleInstance;
    }

    private void DestroyParticles(GameObject particleInstance)
    {
        // Calculate the maximum duration of all particle systems
        float maxDuration = 0f;
        foreach (Transform child in particleInstance.transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                if (ps.main.duration > maxDuration)
                {
                    maxDuration = ps.main.duration;
                }
            }
        }

        // Destroy the particle GameObject after the max duration
        Destroy(particleInstance, maxDuration);
    }


    private void Shapeshift(ShapeClass newShape)
    {
        GameObject particleInstance = ActivateParticles();
        DestroyParticles(particleInstance);

        shape = newShape;

        foreach (ShapeClass sh in shapes)
            if (sh == shape)
            {
                sh.Active(true);
                SetShapeCollider(sh);
            }
            else
                sh.Active(false);
    }

    private void InteractInput()
    {
        if (Input.GetKey(GlobalValues.interactKey))
        {
            interactButton = true;
        }
        else
        {
            interactButton = false;
        }
    }

    private void CrouchActions()
    {
        if (crouch != 0 && move == 0)
        {
            shape.Crouch();
        }

        if (crouch != 0 && move != 0 && running == 0)
        {
            shape.Crouch();
        }

        if (crouch != 0 && move != 0 && running != 0)
        {
            shape.Roll();

            if (!rolling)
                StartCoroutine(Roll());
        }

        if (crouch == 0)
        {
            shape.StopCrouch();
            shape.StopRoll();
        }

        if (!rolling)
        {
            if (crouch != 0)
                SetColliderCrouch();
            else
                SetShapeCollider(shape);
        }
    }

    private void SetShapeCollider(ShapeClass sh)
    {
        boxCollider.size = sh.colliderSizeDefault;
        boxCollider.center = sh.colliderCenterDefault;
    }

    private void SetColliderCrouch()
    {
        boxCollider.size = shape.colliderSizeCrouch;
        boxCollider.center = shape.colliderCenterCrouch;
    }

    private void CrouchCheck()
    {
        crouch = Input.GetAxisRaw("Fire1");

    }

    private void FlipCheck()
    {
        if (lastMove && !facingRight)
            Flip();
        else if (!lastMove && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;

        foreach (ShapeClass sh in shapes)
        {
            Vector3 theScale = sh.transform.localScale;
            theScale.z *= -1;
            sh.transform.localScale = theScale;
        }
    }

    private void MoveCharacterVertical()
    {
        if (climb != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, climb * shape.climbSpeed, 0);
        }
            
    }

    private void MoveCharacterHorizontal()
    {
        if (!isMovementEnabled)
            return;

        if (rolling)
            rb.velocity = new Vector3((facingRight ? 1 : -1) * shape.rollSpeed, rb.velocity.y, 0);
        else if (running > 0)
            rb.velocity = new Vector3(move * shape.runSpeed, rb.velocity.y, 0);
        else
        {
            if (crouch > 0)
                rb.velocity = new Vector3(move * shape.crouchWalkSpeed, rb.velocity.y, 0);
            else
                rb.velocity = new Vector3(move * shape.walkSpeed, rb.velocity.y, 0);
        }
    }

    private IEnumerator Roll()
    {
        SetColliderCrouch();
        rolling = true;

        //Debug.Log("inceput roll " + rolling);

        yield return new WaitForSeconds(rollTime);

        SetShapeCollider(shape);
        rolling = false;

        //Debug.Log("sfarsit roll " + rolling);
    }

    private void WalkInput()
    {
        move = Input.GetAxis("Horizontal");

        if (move > 0)
            lastMove = true;
        else if (move < 0)
            lastMove = false;

        shape.Walk(Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    private void RunInput()
    {
        running = Input.GetAxisRaw("Fire3");
        shape.Run(running);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * shape.jumpForce, ForceMode.Impulse);
    }

    private void JumpCheck()
    {
        if(Input.GetKeyDown(KeyCode.Space) && availableJumps > 0)
        {
            Jump();
            if (!godMode)
                availableJumps--;
        }

        shape.SetGrounded(grounded);
    }

    private void Die()
    {
        if (godMode)
            return;

        level.RestartAtCheckpoint();
    }

    private void RestartLevel()
    {
        rb.position = startPosition;
        level.RestartLevel();
    }

    public void GoToCheckpoint()
    {
        PlayerPrefs.SetFloat("Timer", GlobalValues.totalTImer);
        rb.position = level.GetCrCheckpointPosition();
    }

    public bool IsMovingObject()
    {
        return canMoveObject && interactButton;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void IncrementNrKeys()
    {
        int nrKeys = PlayerPrefs.GetInt("Keys");
        nrKeys++;
        PlayerPrefs.SetInt("Keys", nrKeys);
    }

    public bool DecrementNrKeys()
    {
        int nrKeys = PlayerPrefs.GetInt("Keys");
        nrKeys--;

        PlayerPrefs.SetInt("Keys", nrKeys >= 0 ? nrKeys : 0);

        if (nrKeys >= 0)
            return true;
        
        nrKeys = 0;
        return false;
    }

    public void IncrementNrCoins()
    {
        int nrCoins = PlayerPrefs.GetInt("Coins");
        nrCoins++;
        PlayerPrefs.SetInt("Coins", nrCoins);
    }

    public bool IsInteracting()
    {
        return interactButton;
    }
    
    public void SetIsMovementEnabled(bool value)
    {
        isMovementEnabled = value;
    }

    public void SetRigidBody(Rigidbody rb)
    {
        this.rb = rb;
    }

    public bool IsGrounded()
    {
        return grounded;
    }
}

