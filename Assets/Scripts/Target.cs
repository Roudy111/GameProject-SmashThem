using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private GameManager gameManager; 
    public ParticleSystem explosionParticle;
    



    private float maxTorque = 1;
    private float maxSpeed = 16;
    private float minSpeed = 12;
    private float xRange = 4;
    private float ySpawnPos = -3f;
    public int pointValue; 


    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

 

        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);


        transform.position = RandomSpawnPos();

  

    }


   
    private void OnMouseDown()
    {
        if (gameManager.gameIsOn)
        {
            Destroy(gameObject);
            gameManager.playerAudio.PlayOneShot(gameManager.clickSfx);
           
            gameManager.updateScore(pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            if (pointValue == 2)
            {
                pointValue = Random.Range(-2, 10);
            }
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {

        Destroy(gameObject);
        
        
        

        if (gameObject.CompareTag("Main Targets") && gameManager.gameIsOn)
        {
            
            gameManager.updateLives(-1);
            Debug.Log(gameManager.lives);

            gameManager.playerAudio.PlayOneShot(gameManager.loosLiveSfx);



        }
        

    }


    // Creating Random values for target Physic Forces
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
       return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

}
