using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TrailRenderer))]
public class ClickAndSwap : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;


    
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        trail.enabled = false;
        col.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameIsOn)
        {
            if(Input.GetMouseButtonDown(0))
            {
                swiping = true;
                updateCompontent();
            }
            if(Input.GetMouseButtonUp(0))
            {
                swiping = false;
                updateCompontent();
            }
            if(swiping)
            {
                UpdateMousePos();
            }

        }


    }
    

    void UpdateMousePos()
    {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.position = mousePos;
    }

    void updateCompontent()
    {
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bad")
        {
            Destroy(collision.gameObject);
            gameManager.updateScore(-5);
            gameManager.playerAudio.PlayOneShot(gameManager.clickSfx);


        }
        if(collision.gameObject.tag == "Main Targets")
    {
        // Get the particle system from the Target component
        var targetComponent = collision.gameObject.GetComponent<Target>();
        if (targetComponent != null && targetComponent.explosionParticle != null)
        {
            Instantiate(targetComponent.explosionParticle, collision.transform.position, 
                       targetComponent.explosionParticle.transform.rotation);
        }
        
        Destroy(collision.gameObject);
        gameManager.playerAudio.PlayOneShot(gameManager.clickSfx);
        gameManager.updateScore(2);
    }
    }
}
