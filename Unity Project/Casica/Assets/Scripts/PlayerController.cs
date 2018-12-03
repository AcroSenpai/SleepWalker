﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 axis;
    private Vector2 lastAxis;
    private Vector2 pullAxis;
    private bool push;
    private CharacterController controller;
    private Vector3 moveDirection;
    public float speed;
    public float tspeed;
    private float forceToGround = Physics.gravity.y;
    public float gravityMagnitude;
    public bool jump;
    public float jumpSpeed;
    public bool tocandoSuelo;
    public bool speedMod;
    public Vector3 tranformDirection;
    public UIControler hud;
    public bool primerSuelo;

    private SoundPlayer sound;

    //mecanica pull/push
    public bool cerca;
    public float distance;
    public LayerMask mask;
    private RaycastHit hit;
    private Ray ray;
    private float speedPP;
    private bool fspeedPP = false;
    private Vector3 direccion_rayo;
    private int pos = 0;
    private int objSelec = 0;
    private float x;
    private float z;
    private Vector3 origen;
    public Vector3 DistanciaRayo;
    public bool trepar;
    private RaycastHit Cubito;
    private Animator anim;
    
    //Muerte por altura
    public float puntoMasAlto;
    public float maximoAltura;
    public bool fAltura;

    //Volar
    public float vy;

    //Estados
    public bool planear;
    public bool inmune;
    public bool interactuar;
    public bool bossFight;

    //Pruebas
    public GameObject objetoColisionado;
    public bool Realentizado;

    //Linterna
    public GameObject linterna;
    public bool fLinterna;
    public bool pLinterna;
    public int cLinterna;
    public GameObject espada;


    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<CharacterController>();
        tspeed = speed;
        speedMod = false;
        puntoMasAlto = 0;
        maximoAltura = 10;
        fAltura = false;
        fLinterna = false;
        pLinterna = false;
        cLinterna ++;
        Realentizado = false;
        bossFight = false;
        push = false;
        anim = GetComponent<Animator>();
        sound = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundPlayer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        
        tocandoSuelo = controller.isGrounded;
         if(!primerSuelo && tocandoSuelo)
         {
             primerSuelo = true;
         }
        if (tocandoSuelo && !jump)//Dice si el controler esta tocando el suelo
        {
            moveDirection.y = forceToGround;
            gravityMagnitude = 5;
            planear = false;
        }
        else if(!tocandoSuelo && jump)
        {
            gravityMagnitude = 5;
            moveDirection.y = forceToGround;
            planear = false;
        }
        else
        {
            jump = false;
            moveDirection.y += Physics.gravity.y * gravityMagnitude * Time.deltaTime;
        }
        //transforma el movimiento del moundo al del local
        tranformDirection = axis.x * transform.right + axis.y * transform.forward;



        if(!trepar)
        {
            moveDirection.x = tranformDirection.x * speed;
            moveDirection.z = tranformDirection.z * speed;
        }
        if(inmune)
        {
            moveDirection.y = vy * speed;
        }

        if(axis.x == 0 && axis.y == 0)
        {
            anim.SetBool("walk", false);

        }
        else
        {
            lastAxis = axis;
            anim.SetBool("walk", true);
        }

        controller.Move(moveDirection * Time.deltaTime);//Mueve el controller

        anim.SetFloat("axisX", axis.x);
        anim.SetFloat("axisY", axis.y);

        anim.SetFloat("lastAxisX", lastAxis.x);
        anim.SetFloat("lastAxisY", lastAxis.y);

        if(!push)
        {
            pullAxis = axis;
        }

        anim.SetBool("pull", push);

        anim.SetFloat("pullAxisX", pullAxis.x);
        anim.SetFloat("pullAxisY", pullAxis.y);

        #region Cojer objeto 
        hit = new RaycastHit();
        

        if(objSelec == 0)
        {

            int num1 = 0;
            int num2 = 0;
            
            if(axis.x > 0) 
            {
                num1 = 1;
                pos = 1;
                linterna.transform.rotation = Quaternion.Euler(37, 90, 0);
                espada.transform.rotation = Quaternion.Euler(0, 180, 0);
                Debug.Log("Derecha");
            }   
            else if(axis.x < 0)
            {
                num1 = -1;
                num2 = 0;
                pos = 2;
                linterna.transform.rotation = Quaternion.Euler(37, -90, 0);
                espada.transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("Izquierda");
            }

            if(axis.y > 0) 
            {
                num2 = 1;
                pos = 3;
                linterna.transform.rotation = Quaternion.Euler(37, 0, 0);
                espada.transform.rotation = Quaternion.Euler(0, 90, 0);
                Debug.Log("Alante");
                
            }   
            else if(axis.y < 0)
            {
                num2 = -1;
                num1 = 0;
                pos = 4;
                linterna.transform.rotation = Quaternion.Euler(37, 180, 0);
                espada.transform.rotation = Quaternion.Euler(0, -90, 0);
                Debug.Log("Atras");
            }

            if(axis.x == 0 && axis.y == 0)
            {
                Debug.Log("Parado");
                switch(pos)
                {
                    case 1:
                        num1 = 1;
                        num2 = 0;
                        break;
                    case 2:
                        num1 = -1;
                        num2 = 0;
                        break;
                    case 3:
                        num1 = 0;
                        num2 = 1;
                        break;
                    case 4:
                        num1 = 0;
                        num2 = -1;
                        break;
                }
            }

            x = 1.1f * axis.x + num1;
            z = 1.1f * axis.y + num2;
        }   
        
       origen = transform.position;
       int sing = 1;
        for(int i = 0; i < 3; i++)
        {
            direccion_rayo = new Vector3( x, 0, z);
            ray = new Ray(origen, direccion_rayo);
            if (Physics.Raycast(ray, out hit, distance, mask))
            {
                Debug.Log("Estoy cerca");
                Debug.Log(hit.transform.name);
                objetoColisionado = hit.transform.gameObject;
                //Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.red, 1);
                switch(i)
                {
                    case 0:
                            if(hit.collider.tag == "Object")
                            {
                                Debug.Log("Es una caja movible");
                                cerca = true;
                            }
                            else if(hit.collider.tag == "Trepar")
                            {
                                Debug.Log("Es una pared escalabre");
                                trepar = true;
                            }
                            else if(hit.collider.tag == "Palanca")
                            {
                                Debug.Log("Es un objeto activable");
                                interactuar = true;
                            }
                            else
                            {
                                cerca = false;
                                trepar = false;
                                interactuar = false;
                            }
                            break;
                    case 1:

                    case 2:

                    break;

                }
                
            }
            else
            {
                if(i == 0)
                {
                    cerca = false;
                    
                }
                if(i == 2)
                {
                    trepar = false;
                }
                
            }
            origen.y += sing * (i + 1) * DistanciaRayo.y;
            sing *= -1;
            //interactuar = false;
        }
        
    #endregion

        if(!tocandoSuelo && !planear)
        {
            if(!fAltura)
            {
                fAltura = true;
                puntoMasAlto = transform.position.y;
            }
            else
            {
                if(puntoMasAlto - transform.position.y >= maximoAltura)
                {
                    Dead();
                }
            }
        }
        else
        {
            fAltura = false;
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.attachedRigidbody != null && !hit.collider.attachedRigidbody.isKinematic)
        {
            hit.collider.attachedRigidbody.velocity += (controller.velocity*2) * Time.deltaTime;
        }
    }

    public void Step()
    {
        sound.Play(0, .5f);
    }

    public void SetAxis(Vector2 naxis)
    {
        axis = naxis;
    }

    public void StartJump()
    {
        if(!controller.isGrounded)
        {
            gravityMagnitude = 1f;
            speed = 15;
            Debug.Log("Planear");
            planear = true;
        }
        else
        {
            speed = 15;
            jump = true;
            moveDirection.y = jumpSpeed;
            Debug.Log("Saltar");
            anim.SetTrigger("Jump");
        }            
        
    }

    public void PullPush()
    {
        if(hit.transform != null)
        {
            Cubito = hit;
            Cubito.transform.parent = transform;
            objSelec = 1;
            push = true;
            sound.Play(1, 1);

            if(!fspeedPP)
            {
                speedPP = speed;
                speed = speed / 2;
                fspeedPP = true;
                Realentizado = true;
            }
        }
    }

    public void NoPullPush()
    {
        Cubito.transform.parent = null;
        objSelec = 0;
        push = false;
        if (fspeedPP)
        {
            speed = speedPP;
            fspeedPP = false;
        }

    }

    public void run()
    {
        if(controller.isGrounded && !Realentizado)
        {
            speed = tspeed + 2f;
            speedMod = true;
        }
    }

    public void walk()
    {
        if(controller.isGrounded && !jump && !Realentizado)
        {
            Debug.Log("ando");
            speed = tspeed;
            speedMod = false;
            anim.SetBool("crouch", false);
        }
    }

    public void sneeky()
    {
        if(controller.isGrounded && !Realentizado)
        {
            if(axis.x == 0 && axis.y == 0)
            {
                anim.SetBool("crouch", false);
            }
            else
            {
                anim.SetBool("crouch", true);
            }
            speed = tspeed - 2f;
            speedMod = true;
        }
    }

    public void Escalar()
    {
        if(pos == 1 || pos == 2)//Derecha izquierda
        {
            moveDirection.y = tranformDirection.x * speed;
        }
        else
        {
            moveDirection.y = tranformDirection.z * speed;
        }
    }

    private void OnDrawGizmos()
    {
        origen = transform.position;
        int sing = 1;
        for (int i = 0; i < 3; i++)
        {
            Gizmos.color = Color.blue;
            //Printamos un gizmo con la forma del rayo
            Gizmos.DrawRay(origen, direccion_rayo * distance);
            
            origen.y += sing * (i + 1) * DistanciaRayo.y;
            sing *= -1;
        }    
    }

    public void Dead()
    {
        if(!inmune && primerSuelo)
        {
            Debug.Log("Me ha dado un infartito");
            hud.OpenLosePanel();
        }
        
    }

    public void Win()
    {
        hud.OpenWinPanel();
    }

    public void Inmune()
    {
        inmune = !inmune;
        if(inmune)
        {
            Debug.Log("Eres jisus");
            moveDirection.y = vy;
        }
        else
        {
            Debug.Log("Ya no eres jisus");
        }
    }

    public void Interactuar()
    {
        Debug.Log("Interactuo");
        objetoColisionado.GetComponent<Interactive>().Activar();
    }

    public void Linterna()
    {
        if(pLinterna)
        {
            fLinterna = !fLinterna;
            if(fLinterna)
            {
                linterna.SetActive(true);
            }
            else
            {
                linterna.SetActive(false);
            }
        }
        
    }

    public void cojerObj(int num)
    {
        if(num == 1)
        {
            Debug.Log("Cojo la linternita");
            /*Animaicon recogiendo la linterna y que desde ahora aparezca */
            cLinterna ++;
        }
        else
        {
            /*Recojer pilas */
            cLinterna ++;

            if(cLinterna == 4)
            {
                /*Linterna operativa */
                pLinterna = true;
            }
        }
    }


    public void SetBossFight()
    {
        bossFight = !bossFight;
        if(bossFight)
        {
            espada.SetActive(true);
        }
        else
        {
            espada.SetActive(false);
        }
    }
    public void Attack()
    {
        Debug.Log("Estoy atacando");
        anim.SetTrigger("Ataque");
    }


}
