using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    public Animator animator;
    public Transform point;

    public float radius;
    public float speed;
    public float jumpForce;

    private bool isAttacking;
    private bool isJumping;
    private bool doubleJump;


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
                animator.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,0,0);
        }

        if(movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                animator.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if(movement == 0 && !isJumping && !isAttacking)
        {
            animator.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                animator.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            } 
            else if(doubleJump) 
            {
                animator.SetInteger("transition", 2);
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
            animator.SetInteger("transition", 3);
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius);
            if (hit != null)
            {
                Debug.Log(hit.name);
            }
            StartCoroutine(OnAttack()); 
        }
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
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
}
