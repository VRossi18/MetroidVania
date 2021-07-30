using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    private Animator anim;

    public Animator barrierAnim;
    public LayerMask layer;

    void Start()
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

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1, layer);
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
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
