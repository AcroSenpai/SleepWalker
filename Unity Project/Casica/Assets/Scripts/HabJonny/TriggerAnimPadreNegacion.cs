﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimPadreNegacion : MonoBehaviour
{
    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(manager.GetProgresion() == 2)
        {
            if (other.tag == "Player")
            {
                manager.SetProgresion(3);
            }
        }
        
    }
}
