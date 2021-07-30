using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isFront;
    private Vector2 direction;
    private Animator anim;

    public bool isRight;
    public float stopDistance;
    public float speed;
    public float maxVision;
    public Transform point;
    public Transform behind;
    public float health;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    private void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);
        RaycastHit2D hitBehind = Physics2D.Raycast(behind.position, direction * -1, maxVision);

        //Raycast in front of the goblin
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance)
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;

                    anim.SetInteger("transition", 2);

                    hit.transform.GetComponent<Player>().OnHit();
                }
            }
        }

        //Raycas on the back of the goblin
        if (hitBehind.collider != null)
        {
            if (hitBehind.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                isFront = true;
            }
        }

    }

    void OnMove()
    {
        if (isFront)
        {
            anim.SetInteger("transition", 1);
            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);
            }
        }
    }

    public void OnHit()
    {
        anim.SetTrigger("hit");
        health--;

        if (health <= 0)
        {
            speed = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }
}
