using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerAudio playerAudio;

    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    public int health;
    public float radius;
    public float speed;
    public float jumpForce;

    private bool isAttacking;
    private bool isJumping;
    private bool doubleJump;
    private float recoveryCount;
    private bool dead;

    private static Player instance;

    private void Awake()
    {
        DontDestroyOnLoad(this); // mantem um objeto entre cenas

        if(instance == null) // checar se já existe um player na cena
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    
    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if(movement > 0) 
        {
            if (!isJumping && !isAttacking) // Caso não esteja pulando executa a animação de walk. Caso esteja pulando mantem a execução do jump.
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,0,0);
        }

        if(movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if(movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            } 
            else if(doubleJump) 
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("transition", 3);
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {
                if (hit.GetComponent<Sline>())
                {
                    hit.GetComponent<Sline>().OnHit();
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
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
    }

    public void OnHit()
    {
        recoveryCount += Time.deltaTime; // tempo de recuperaçao
        if(recoveryCount >= 2f) // Lógica para o player tenha um tempo de recuperação de 2 segundos
        {
            anim.SetTrigger("hit");
            health--;
            recoveryCount = 0;
        }

        if (health <= 0 && !dead)
        {
            dead = true;
            anim.SetTrigger("dead");
            //game over
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            isJumping = false;
        }

        if(collision.gameObject.layer == 11)
        {
            PlayerPosition.instance.CheckPoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9) // layer 9 -> Enemy
        {
            OnHit();
            Debug.Log("wdadadad");
        }

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameManeger.instance.GetCoin();
            Destroy(collision.gameObject, 0.5f);
        }

        if(collision.gameObject.layer == 10)
        {
            GameManeger.instance.NextLevel();   
        }
    }
}
