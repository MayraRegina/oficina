using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 7;
    public float speed;
    public float jumpForce;

    public GameObject Bala;
    public GameObject Bala2;
    public Transform firePoint;
    
    public bool isJumping;
    public bool doubleJump;
    public bool isFire;
    public bool stage1 = true;
    public bool stage2;
    public bool stage3;
    

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
        Jump();
        balaFire();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition",3);
            }
            
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 

        if (movement < 0 )
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition",3);   
            }
            
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(movement == 0 && !isJumping && !isFire)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("Transition",1);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if(doubleJump && stage2 == true)
                {
                    anim.SetInteger("Transition",1);
                    rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    void balaFire()
    {
       StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isFire = true;
            anim.SetInteger("Transition", 4);
            if (stage1 == true)
            {
                GameObject bala = Instantiate(Bala, firePoint.position, firePoint.rotation);
            }

            if (stage3 == true)
            {
                GameObject bala2 = Instantiate(Bala2, firePoint.position, firePoint.rotation);
            }
            yield return new WaitForSeconds(0.3f);
            isFire = false;
            anim.SetInteger("Transition",0);
        }
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
}
