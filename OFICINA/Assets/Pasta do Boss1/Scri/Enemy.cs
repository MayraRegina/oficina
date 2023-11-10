using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 3f;
    public Animator anim;
    private Rigidbody2D rig;
    public float speed = 5f;
    public float timer = 4f;
    public float walktime = 4f;
    public int Life = 30;
    public int stagio = 1;

    public bool walkRight = true;

    public bool canAttack = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Life <=0)
        {
            anim.SetTrigger("morrendo");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(gameObject, 3f);
            SceneManager.LoadScene(1);
        }

        if (Life > 0)
        {
            StarRoutin();
        }

        if (Life <= 15)
        {
            stagio = 2;
            speed = 10f;
            walktime = 2;
        }
    }

    private void FixedUpdate()
    {
        if (Life > 0)
        {
            move();
        }
    }

    private IEnumerator PerformMeleeAttack()
    {
        if (attackCooldown <= 0)
        {
            attackCooldown = 3f;
            canAttack = false;
            anim.SetInteger("Transition", 2);
            yield return new WaitForSeconds(1f);
            Debug.Log("Atacou");
            canAttack = true;
        }
    }

    private void StarRoutin()
    {
        if (attackCooldown <= 3)
        {
            attackCooldown -= Time.deltaTime;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(PerformMeleeAttack());
        }
    }

    void move()
    {
        if (stagio == 1)
        {
            timer += Time.deltaTime;
        
            if (timer >= walktime)
            {
                walkRight = !walkRight;
                timer = 0f;
            }
            if (walkRight && canAttack == true)
            {
                anim.SetInteger("Transition", 0);
                transform.eulerAngles = new Vector2(0,0);
                rig.velocity = Vector2.right * speed;
            }
            if(!walkRight && canAttack == true)
            {
                anim.SetInteger("Transition", 0);
                transform.eulerAngles = new Vector2(0,180);
                rig.velocity = Vector2.left * speed;
            }   
        }

        if (stagio == 2)
        {
            timer += Time.deltaTime;
        
            if (timer >= walktime)
            {
                walkRight = !walkRight;
                timer = 0f;
            }
            if (walkRight && canAttack == true)
            {
                anim.SetInteger("Transition", 1);
                transform.eulerAngles = new Vector2(0,0);
                rig.velocity = Vector2.right * speed;
            }
            if(!walkRight && canAttack == true)
            {
                anim.SetInteger("Transition", 1);
                transform.eulerAngles = new Vector2(0,180);
                rig.velocity = Vector2.left * speed;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BalaPlayer")
        {
            Life--;
            Destroy(collision.gameObject);
        }
    }
}
