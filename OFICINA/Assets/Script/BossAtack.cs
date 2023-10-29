using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAtack : MonoBehaviour
{
    public Transform player; // O jogador a ser atacado
    public float attackRange = 2f;
    public float attackCooldown = 3f;
    public float attackDamage = 10;
    public Animator anim;
    private Rigidbody2D rig;
    public float speed;
    public float timer;
    public float walktime;
    public int LifeDemon = 30;
    public int damage = 1;

    public bool walkRight = true;

    public bool canAttack = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (LifeDemon <=0)
        {
            anim.SetTrigger("Morte");
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
        }

        if (LifeDemon > 0)
        {
            StarRoutin();
        }
    }

    private void FixedUpdate()
    {
        if (LifeDemon > 0)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BalaPlayer")
        {
            LifeDemon--;
        }
    }
}
