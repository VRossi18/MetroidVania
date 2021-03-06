using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerAudio playerAudio;

    //Force variables
    public float speed;
    public float jumpForce;
    private Health healthSystem;

    //Checking bools
    public bool isJumping;
    private bool doubleJump;
    private bool isAttacking;

    //Animator
    public Animator anim;

    //Attacking
    public Transform point;
    public float radius;
    public LayerMask enemyLayer;
    private bool recovery;

    private static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        healthSystem = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //Se n?o pressionar nada ,retorna 0. Else retorna 1
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        //GetButtonDown using buttons on unity configuration
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                playerAudio.PlaySFX(playerAudio.jumpSound);
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            }
            else if (doubleJump)
            {
                playerAudio.PlaySFX(playerAudio.jumpSound);
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
    }

    //Attacking function
    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerAudio.PlaySFX(playerAudio.hitSound);
            isAttacking = true;
            anim.SetInteger("transition", 3);

            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);
            if (hit != null)
            {
                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();
                }

                if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();
                }
            }

            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.333f);
        isAttacking = false;
    }

    float recoveryCount;
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;
        if (recoveryCount >= 2f)
        {
            anim.SetTrigger("hit");
            healthSystem.health--;

            recoveryCount = 0f;
        }

        if (healthSystem.health <= 0 && !recovery)
        {
            recovery = true;
            anim.SetTrigger("death");
            GameController.instance.ShowGameOver();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
        }


        if (collision.gameObject.layer == 10)
        {
            PlayerPos.instance.Checkpoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            GameController.instance.GetCoin();
            collision.GetComponent<Animator>().SetTrigger("hit");
            Destroy(collision.gameObject, 0.41f);
        }

        if (collision.gameObject.layer == 9)
        {
            GameController.instance.NextLevel();
        }
    }
}
