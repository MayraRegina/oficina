using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Atributos")] 
    public float jump;
    public float speed;
    public int life;
    public bool isjump;
    public bool doublejump;

    [Header("Componentes")] 
    public Rigidbody rig;
    public Animator anim;
    
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void move()
    {
        float moviment = Input.GetAxis("Horizontal");
        rig.velocity = new Vector3(moviment * speed, rig.velocity.y);
    }
}
