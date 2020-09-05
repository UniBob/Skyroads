using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ShipScript spaceShip;
    [SerializeField] RoadScript road;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject pauseMenu;

    int score;

    private void Start()
    {
        StartCoroutine("SpawnAsteroid");
        spaceShip.StartMoving();
    }

    public void PauseButton()
    {
        pauseMenu.gameObject.SetActive(true);
        StopAllCoroutines();
        spaceShip.StopMoving();
    }

    public void GameOver()
    {
        StopAllCoroutines();
        spaceShip.StopMoving();

    }

    public void ResumeButton()
    {
        pauseMenu.gameObject.SetActive(false);
        StartCoroutine("SpawnAsteroid");
        spaceShip.StartMoving();
    }

    public void Quit()
    {

    }

    public void ReplaceTiles()
    {
        road.ReplaceTiles();
    }

    IEnumerator SpawnAsteroid()
    {
        float spawnPeriod;
        Vector3 asteroidCoordinates;
        while (1 == 1)
        {
            spawnPeriod = NextAsteroidTime();
            asteroidCoordinates = road.GetCoordinatesForAsteroid();
            asteroidCoordinates.x = Random.Range(-spaceShip.GetRoadBorderCoordinates(), spaceShip.GetRoadBorderCoordinates());
            asteroidCoordinates.y = 1;
            Instantiate(asteroidPrefab, asteroidCoordinates, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnPeriod, spawnPeriod + 0.5f));
        }
    }

    float NextAsteroidTime()
    {
        return (-Mathf.Sin(score - 60) / 40) + 1.5f;
    }

    public bool CheckAsteroidCoordinates(Vector3 asteroidCoord)
    {
        if (spaceShip.transform.position.z > asteroidCoord.z) 
            return true;
        else 
            return false;
    }
}
