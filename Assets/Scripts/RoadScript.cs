using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    [SerializeField] List<GameObject> road = new List<GameObject>();
    Vector3 offset = new Vector3 (0,0, 240);    
    int nearestTile = 0;                        //Nearest tile index

    public void ReplaceTiles()
    {
        //Replace closest to ship road tile to the end of road
        road[nearestTile].transform.position += offset;
        nearestTile++;
        if (nearestTile >= road.Count)
        {
            nearestTile = 0;
        }
    }

    //Used by Asteroid spawner
    public Vector3 GetCoordinatesForAsteroid()
    {
        return road[nearestTile].gameObject.transform.position + offset;
    }

    //Reset all parameters
    public void StartGameAgain()
    {
        Vector3 coord = new Vector3(0, 0, 0);
        foreach (var tile in road)
        {
            tile.transform.position = coord;
            coord.z += 10;
        }
        nearestTile = 0;
    }
}
