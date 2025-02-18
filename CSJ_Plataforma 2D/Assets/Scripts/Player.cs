using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
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


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
            } 
            else if(doubleJump) 
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
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
            if (hit != null)
            {
                hit.GetComponent<Sline>().OnHit();
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            OnHit();
        }
    }
}
