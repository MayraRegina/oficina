using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAtack : MonoBehaviour
{
    public Transform player;
    public float attackRange;
    public float attackCooldown = 1.5f;
    public Animator anim;
    private Rigidbody2D rig;
    public float speed;
    public float timer;
    public float walktime;
    public int LifeDemon = 30;
    public int stagio = 1;
    public AudioClip[] audio;
    public AudioSource source;
    public Player playerdameg;
    public int S;

    public bool walkRight = true;

    public bool canAttack = true;

    private void Start()
    {
        playerdameg = GetComponent<Player>();
        source = GetComponent<AudioSource>();
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
            Destroy(gameObject, 3f);
        }

        if (LifeDemon > 0)
        {
            StarRoutin();
        }

        if (LifeDemon <= 25)
        {
            stagio = 2;
            speed = 6f;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BalaPlayer")
        {
            LifeDemon--;
            Destroy(collision.gameObject);
            Play(2);
        }

        if (collision.gameObject.tag == "Bala2")
        {
            LifeDemon -= 2;
            Destroy(collision.gameObject);
        }
    }

    void Play(int value)
    {
        if (value >= 0 && value < audio.Length)
        {
            source.clip = audio[value];
            source.Play();
        }
    }
}
