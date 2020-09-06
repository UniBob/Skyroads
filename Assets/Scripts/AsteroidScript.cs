using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();   
    }

    private void Update()
    {
        //Wait while ship passed it and destroy itself
        if (gm.CheckAsteroidCoordinates(transform.position))
        {
            gm.AddScoreForAsteroid();
            Destroy(gameObject);
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            gm.GameOver();
        }
    }
}
