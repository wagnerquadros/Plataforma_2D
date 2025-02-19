using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private Vector2 direction;
    public Animator anim;
    private bool isFront;
    public int health;
    public float stopDistance;
    public bool isRigth;
    public Transform point;
    public Transform behind;
    public float speed;
    public float maxVision;

    

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isRigth) // vira para direita
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else // vira para equerda
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    
    void Update()
    {

    }

    private void FixedUpdate()
    {
        GetPlayer();
        
        OnMove();
    }



    void OnMove()
    {
        if(isFront)
        {
            anim.SetInteger("transition", 1);
            if (isRigth) // vira para direita
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else // vira para equerda
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }


    }

    void GetPlayer()
    {
        // Raycast(origem, direção, distância)
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        if(hit.collider != null) // Se o rRayCast detectou algum colisor
        {
            if (hit.transform.CompareTag("Player")) // Se o colisor detectado é o PLayer
            {
                isFront = true; // Playes está na frente

                float distance = Vector2.Distance(transform.position, hit.transform.position); // verifica a distancia entre o inimigo e o player
                if (distance <= stopDistance)
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;
                    anim.SetInteger("transition", 2);
                    hit.transform.GetComponent<Player>().OnHit(); // ataca o player
                }

            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

        if(behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                isRigth = !isRigth;
                isFront = true;
            }
        }
    }

    public void OnHit()
    {
        anim.SetTrigger("hit"); // chama a animãção do hit
        health--; // diminui a vida

        if(health <= 0)  // verifica  se a vida  for menor ou igual  a =
        {
            speed = 0; // pára o inimigo
            anim.SetTrigger("dead"); // chama a aniamção de morte
            Destroy(gameObject,0.5f); // distroi o objeto
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }

}
