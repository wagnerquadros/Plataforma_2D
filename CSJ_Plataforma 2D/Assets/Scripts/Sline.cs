using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sline : MonoBehaviour
{

    private Rigidbody2D rig;
    private Animator anim;

    public int health;
    public float speed;
    public float radius;
    public Transform point;
    public LayerMask layer;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);
        OnCollision();
    }

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);
        
        if (hit != null)
        {
            speed = -speed; //Inverte a direção de movimento do inimigo

            if(transform.eulerAngles.y ==0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            
        }
    }

    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if(health <= 0)
        {
            speed = 0;
            anim.SetTrigger("dead");
            Destroy(gameObject, 0.5f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

}
