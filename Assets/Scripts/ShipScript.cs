using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{

    [SerializeField] private float roadBorderCoordinates;
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gm;
    Vector3 forwardLast;
    int lastTileZCoord;
    bool isMoving;

    private void Start()
    {
        forwardLast = spaceShip.transform.forward;
        //rb.AddForce(new Vector3(0, 0, movementSpeed*50));
        isMoving = false;
        
        lastTileZCoord = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (Mathf.Abs(turnSpeed * (Input.GetAxis("Horizontal")) + transform.position.x) <= roadBorderCoordinates)
            {
                transform.position += new Vector3(turnSpeed * (Input.GetAxis("Horizontal")), 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += new Vector3(0, 0, movementSpeed * 2);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.velocity -= new Vector3(0, 0, movementSpeed * 2);
            }
            if (transform.position.z >= lastTileZCoord)
            {
                gm.ReplaceTiles();
                lastTileZCoord += 10;
            }
        }
        //float directionAngel = Vector3.Angle(this.forwardLast, this.transform.forward);
        //forwardLast = Vector3.Lerp(forwardLast, spaceShip.transform.forward, Time.deltaTime);
        //spaceShip.transform.forward += new Vector3(directionAngel,0,0);
    }

    public float GetRoadBorderCoordinates()
    {
        return roadBorderCoordinates;
    }

    public void StartMoving()
    {
        rb.velocity = (new Vector3(0, 0, movementSpeed));
        isMoving = true;
    }

    public void StopMoving()
    {
        rb.velocity = (new Vector3(0, 0, 0));
        isMoving = false;
    }
}
