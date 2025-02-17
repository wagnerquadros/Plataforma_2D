using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    public Transform point;
    private bool isFront;

    public float speed;
    public float maxVision;

    

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        // Raycast(origem, direção, distância)
        RaycastHit2D hit = Physics2D.Raycast(point.position, Vector2.right, maxVision);

        if(hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Vendo o Player");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(point.position, Vector2.right * maxVision);
    }
}
