using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gm;

    [Header("Speed count")]
    [SerializeField] private float turnSpeed;                   //Speed by X
    [SerializeField] private float movementSpeed;               //Speed by Z

    [Header("Speed effect")]
    [SerializeField] float normalFieldOfView = 40.0f;           //Camera field of view with normal speed
    [SerializeField] float speedFieldOfView = 60.0f;            //Camera field of view with speed boost

    [Header("Other")]
    [SerializeField] private float roadBorderCoordinates;       //Borders for ship
    [SerializeField] private GameObject spaceShip;              //Link to model
    [SerializeField] ParticleSystem[] engines;

    int lastTileZCoord;                                         //Last road tile coordinate
    bool isMoving;

    private void Start()
    {
        isMoving = false;   
        lastTileZCoord = 15;    
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //Turn control
            if (Mathf.Abs(turnSpeed * (Input.GetAxis("Horizontal")) + transform.position.x) <= roadBorderCoordinates)
            {
                transform.position += new Vector3(turnSpeed * (Input.GetAxis("Horizontal")), 0, 0);
            }
            //Boost on control
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += new Vector3(0, 0, movementSpeed * 2);
                gm.AddBonusForSpeed(1);
                foreach (var engine in engines)
                {
                    engine.startLifetime = 2;
                }
            }
            //Boost off control
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity -= new Vector3(0, 0, movementSpeed * 2);                
                gm.AddBonusForSpeed(-1);
                foreach (var engine in engines)
                {
                    engine.startLifetime = 1;
                }
            }
            //Change camera field of view
            if (Input.GetKey(KeyCode.Space))
            {
                if (Camera.main.fieldOfView > speedFieldOfView) Camera.main.fieldOfView -= 5;   
            }
            else
            {
                if (Camera.main.fieldOfView < normalFieldOfView) Camera.main.fieldOfView += 5;
            }
            //Border control
            if (transform.position.z >= lastTileZCoord)
            {
                gm.ReplaceTiles();
                lastTileZCoord += 10;
            }
            //Rotation control
            spaceShip.transform.up = transform.up + (new Vector3(Input.GetAxis("Horizontal"), 0, 0));
        }
    }

    //User by GameManager to understand borders of asteroid spawm
    public float GetRoadBorderCoordinates()
    {
        return roadBorderCoordinates;
    }

    //Start game
    public void StartMoving()
    {
        rb.velocity = (new Vector3(0, 0, movementSpeed));
        isMoving = true;
    }

    //Pause game
    public void StopMoving()
    {
        rb.velocity = (new Vector3(0, 0, 0));
        isMoving = false;
    }

    //Reset all parameters and start move
    public void RestartGame()
    {
        lastTileZCoord = 15;
        StartMoving();
    }

    //Change speed with time by GameManager
    public void ChangeSpeed(int additionalSpeed)
    {
        rb.velocity += new Vector3(0, 0, additionalSpeed);
        movementSpeed += additionalSpeed;
    }
}
