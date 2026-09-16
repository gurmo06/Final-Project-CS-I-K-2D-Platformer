using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Physics : MonoBehaviour
{
    Rigidbody2D playerRigidBody;
    Collider2D playerCollider;
    SpriteRenderer playerRenderer;
    public Transform playerTransform;
    public float speed;
    public float jumpForce;
    bool isGrounded;
    public Transform groundCheckerTransform;
    public float checkGroundRadius;
    public float checkResetRadius;
    public float checkCrystalRadius;
    public float checkEndGameRadius;
    public LayerMask groundLayer;
    public LayerMask resetLayer;
    public LayerMask crystalLayer;
    public LayerMask crystalResetLayer;
    public LayerMask endGameLayer;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;
    public int defaultAdditionalJumps = 1;
    int additionalJumps;
    public int level = 0;
    public float levelCooldown = 0.0f;
    float resetX = 0.0f;
    float resetY = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        levelCooldown = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();
        Reflection();
        ResetPlayer();
        Level();
        EndLevel();
    }

    void Move()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheckerTransform.position, checkGroundRadius, groundLayer);
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy;
        if (x != 0)
        {
            moveBy = x * speed;
            playerRigidBody.linearVelocity = new Vector2(moveBy, playerRigidBody.linearVelocity.y);
        }
        else if (collider != null)
        {
            moveBy = playerRigidBody.linearVelocity.x * 0.97f;
            playerRigidBody.linearVelocity = new Vector2(moveBy, playerRigidBody.linearVelocity.y);
        }
        else
        {
            moveBy = playerRigidBody.linearVelocity.x;
            playerRigidBody.linearVelocity = new Vector2(moveBy, playerRigidBody.linearVelocity.y);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0))
        {
            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
            additionalJumps--;
        }
    }

    void BetterJump()
    {
        if (playerRigidBody.linearVelocity.y < 0)
        {
            playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRigidBody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            playerRigidBody.linearVelocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheckerTransform.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;   
            }
            isGrounded = false;
        }
    }

    void Reflection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerRenderer.flipX = false;
        }
    }

    void ResetPlayer()
    {
        Collider2D collider2 = Physics2D.OverlapCircle(playerTransform.position, checkResetRadius, resetLayer);
        if (collider2 != null)
        {
            playerTransform.position = new Vector3(resetX, resetY, 0);
        }
    }

    void Level()
    {
        Collider2D collider3 = Physics2D.OverlapCircle(playerTransform.position, checkCrystalRadius, crystalLayer);
        Collider2D collider4 = Physics2D.OverlapCircle(playerTransform.position, checkCrystalRadius, crystalResetLayer);
        if (collider3 != null && Time.time - levelCooldown > 1.0f)
        {
            level++;
            levelCooldown = Time.time;
            Debug.Log("Level = " + level);
        }
        else if (collider4 != null && Time.time - levelCooldown >1.0f)
        {
            level++;
            levelCooldown = Time.time;
            resetX = playerTransform.position.x;
            resetY = playerTransform.position.y;
            Debug.Log("Level = " + level);
        }
    }

    void EndLevel()
    {
        Collider2D collider5 = Physics2D.OverlapCircle(playerTransform.position, checkEndGameRadius, endGameLayer);
        if (level >= 8 && collider5 != null)
        {
            Debug.Log("Enter Portal");
            SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
        }
    }
}
