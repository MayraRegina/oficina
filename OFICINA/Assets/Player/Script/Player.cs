using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 7;
    public float speed;
    public float jumpForce;
    public float bulletspeed = 15f;

    public GameObject Bala;
    public GameObject Bala2;
    public Transform firePoint;

    public bool isJumping;
    public bool doubleJump;
    public bool isFiring;
    public bool stage1 = true;
    public bool stage2;
    public bool stage3;
    public bool isdead;

    private Rigidbody2D rig;
    private Animator anim;

    private float movement;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isdead == false)
        {
            dead();
            Jump();
            Fire();
        }

    }

    void FixedUpdate()
    {
        if (isdead == false)
        {
            Move();
        }
    }

    void Move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 3);
            }

            bulletspeed = 15f;

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 3);
            }

            bulletspeed = -15f; 

            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isFiring)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 1);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if (doubleJump && stage2)
                {
                    anim.SetInteger("Transition", 1);
                    rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isFiring)
        {
            if (stage1 || stage2)
            {
                StartCoroutine(FireRoutine(0.5f));
            }
            else if (stage3)
            {
                StartCoroutine(FireRoutine(1.0f));
            }
        }
    }

    IEnumerator FireRoutine(float fireDelay)
    {
        if (!isFiring)
        {
            isFiring = true;
            if (stage1 == true)
            {
                anim.SetInteger("Transition", 4);
                bullet1(bulletspeed);
                yield return new WaitForSeconds(fireDelay);
                isFiring = false;
            }
            if (stage3 == true)
            {
                anim.SetInteger("Transition", 4);
                bullet2(bulletspeed);
                yield return new WaitForSeconds(fireDelay);
                isFiring = false;
            }
            anim.SetInteger("Transition", 0);
        }
    }

    void bullet1(float speed)
    {
        GameObject b1 = Instantiate(Bala, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = b1.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        Destroy(b1, 2f);
    }

    void bullet2(float speed)
    {
        GameObject b2 = Instantiate(Bala2, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = b2.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        Destroy(b2, 2f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 6)
        {
            isJumping = false;
        }

        if (coll.gameObject.tag == "espada")
        {
            health--;
        }
    }

    void dead()
    {
        if (health <= 0)
        {
            isdead = true;
            anim.SetTrigger("Morte");
            Destroy(gameObject, 2f);
        }
    }
}
