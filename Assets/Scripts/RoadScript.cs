using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    [SerializeField] List<GameObject> road = new List<GameObject>();
    Vector3 offset = new Vector3 (0,0, 240);
    int nearestTile = 0;

    public void ReplaceTiles()
    {
        road[nearestTile].transform.position += offset;
        nearestTile++;
        if (nearestTile >= road.Count)
        {
            nearestTile = 0;
        }
    }

    public Vector3 GetCoordinatesForAsteroid()
    {
        return road[nearestTile].gameObject.transform.position + offset;
    }

}
