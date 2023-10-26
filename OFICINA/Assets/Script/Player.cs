using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 7;
    public float speed;
    public float jumpForce;

    public GameObject Bala;
    public Transform firePoint;
    
    private bool isJumping;
    private bool doubleJump;
    private bool isFire;
    private bool stage1 = true;
    private bool stage2;
    private bool stage3;
    

    private Rigidbody2D rig;
    private Animator anim;

    private float movement;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
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
                anim.SetInteger("transicao",1);
            }
            
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 

        if (movement < 0 )
        {
            if (!isJumping)
            {
                anim.SetInteger("transicao",1);   
            }
            
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(movement == 0 && !isJumping && !isFire)
        {
            anim.SetInteger("transicao", 0);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transicao",2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if(doubleJump)
                {
                    anim.SetInteger("transicao",2);
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
            anim.SetInteger("transicao", 3);
            GameObject bala = Instantiate(Bala, firePoint.position, firePoint.rotation);

            if(transform.rotation.y == 0)
            {
                
            }

            if(transform.rotation.y == 180)
            {
            }
            
            yield return new WaitForSeconds(0.3f);
            isFire = false;
            anim.SetInteger("transicao",0);
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
