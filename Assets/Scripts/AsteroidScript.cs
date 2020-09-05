using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] GameManager gm;
    
    private void Update()
    {
        if (Time.time % 2 ==0)
        {
            if (gm.CheckAsteroidCoordinates(transform.position)) Destroy(gameObject);
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
