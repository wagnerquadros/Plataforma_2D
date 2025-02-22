using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class PuzzleButton : MonoBehaviour
{

    private Animator anim;
    public Animator barrierAnim;
    public LayerMask layer;
    public float radius;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        OnCollision();
    }

    void OnPressed()
    {
        anim.SetBool("isPressed", true);
        barrierAnim.SetBool("isPressed", true);

    }

    void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("isPressed", false);
    }

    //Verifica enquanto um colisor está detectando colisão
    //private void OnCollisionStay2D(Collision2D collision) 
    //{
    //    if(collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnPressed();
    //   }
    //}

    // Retorna quando para a colisão
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnExit();
    //    }
    //}

    //Outra forma de fazer a colisão com o botão é utilizando o Physis2D

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, layer);

        if (hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
