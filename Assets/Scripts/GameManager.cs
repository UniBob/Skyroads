using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject asteroidPrefab;     //Prefab to spawn sometime
    [SerializeField] GameObject startMenu;          //Group of elements, that we see at the start
    [SerializeField] GameObject pauseMenu;          //Group of elements, that we see on game paused
    [SerializeField] GameObject gameOverMenu;       //Group of elements, that we see after game over
    [Header("UI Elements")]
    [SerializeField] Text scoreText;                //Textfield with current score
    [SerializeField] Text highScoreText;            //Textfield with highscore
    [SerializeField] Text passedAsteroidsText;      //Textfield with count of passed asteroids
    [SerializeField] Text sessionTimeText;          //Textfield with current session time
    [SerializeField] Slider slider;                 //Sound volume slider
    [Header("Ship")]
    [SerializeField] ShipScript spaceShip;
    [SerializeField] Vector3 shipStartPosition;
    [Header("Road Script")]
    [SerializeField] RoadScript road;
    [Header("Sound Manager")]
    [SerializeField] SoundManager sounds;

    [SerializeField] [TextArea(minLines: 1, maxLines: 10)] string openPhrase;
    [SerializeField] [TextArea(minLines: 1, maxLines: 10)] string gameOverPhrase;
    [SerializeField] [TextArea(minLines: 1, maxLines: 10)] string highScoreBited;

    bool gameDontRun;       //Used ones per game run. Waiting before Space pressed
    int score;
    int tickScore;          //If Space hold - 2 points, else - 1;
    int passedAsteroids;
    int sessionTime;

    private void Start()
    {

        if (PlayerPrefs.HasKey("Highscore"))
        {
            startMenu.GetComponentInChildren<Text>().text = openPhrase + PlayerPrefs.GetInt("Highscore").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        //Reset and set all parameters and set gameDontRun for waiting start
        gameDontRun = true;
        startMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        slider.gameObject.SetActive(true);
        slider.value = PlayerPrefs.GetFloat("Volume");
        passedAsteroids = 0;
        sessionTime = 0;
        spaceShip.transform.position = shipStartPosition;
        highScoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update()
    {
        if (gameDontRun)
        {
            //Wait untill Space down and start game
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartCoroutine("CaroutineStarter");
                score = 0;
                tickScore = 1;
                scoreText.text = score.ToString();
                spaceShip.StartMoving();
                gameDontRun = false;
                startMenu.SetActive(false);
                slider.gameObject.SetActive(false);
                sounds.StartMusic();
                highScoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
                passedAsteroidsText.text = passedAsteroids.ToString();
                sessionTimeText.text = sessionTime.ToString();
            }
        }
        //Change volume if slider value changed
        if (slider.value != PlayerPrefs.GetFloat("Volume")) sounds.ChangeVolume(slider.value);
    }
    public void PauseButton()
    {
        //Stop game, open pause menu, mute music
        pauseMenu.gameObject.SetActive(true);
        StopAllCoroutines();
        spaceShip.StopMoving();
        slider.gameObject.SetActive(true);
        sounds.ChangeVolume(PlayerPrefs.GetFloat("Volume") / 3);
    }

    public void GameOver()
    {
        //Stop game, open game over menu, mute music
        StopAllCoroutines();
        spaceShip.StopMoving();
        gameOverMenu.SetActive(true);
        sounds.ChangeVolume(PlayerPrefs.GetFloat("Volume") / 3);
        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
            gameOverMenu.GetComponentInChildren<Text>().text = highScoreBited + score.ToString();
        }
        else
        {
            gameOverMenu.GetComponentInChildren<Text>().text = gameOverPhrase + score.ToString();
        }
    }

    public void RestartButton()
    {
        //Reset all parameters and start game again

        //Search for all asteroids and destroy them
        AsteroidScript[] asteroids = FindObjectsOfType<AsteroidScript>();
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }
        spaceShip.transform.position = shipStartPosition;
        gameOverMenu.SetActive(false);
        StartCoroutine("CaroutineStarter");
        road.StartGameAgain();
        score = 0;
        tickScore = 1;
        scoreText.text = score.ToString();
        spaceShip.RestartGame();
        sounds.ChangeVolume(PlayerPrefs.GetFloat("Volume"));
    }

    public void ResumeButton()
    {
        //Close menu and start game
        pauseMenu.gameObject.SetActive(false);
        StartCoroutine("CaroutineStarter");
        spaceShip.StartMoving();
        sounds.ChangeVolume(PlayerPrefs.GetFloat("Volume"));
        slider.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReplaceTiles()
    {
        //When ship passed 10 points per Z, set closest road tile to the end
        //Used to connect ShipScript and RoadScript
        road.ReplaceTiles();
    }

    IEnumerator SpawnAsteroid()
    {
        //Coroutine that spaw asteroids with period
        float spawnPeriod;
        Vector3 asteroidCoordinates;
        while (1 == 1)
        {
            spawnPeriod = NextAsteroidTime();
            asteroidCoordinates = road.GetCoordinatesForAsteroid();
            asteroidCoordinates.x = Random.Range(-spaceShip.GetRoadBorderCoordinates(), spaceShip.GetRoadBorderCoordinates());
            asteroidCoordinates.y = 2.3f;
            asteroidCoordinates.z = Random.Range(asteroidCoordinates.z - 5, asteroidCoordinates.z + 5);
            Instantiate(asteroidPrefab, asteroidCoordinates, Quaternion.identity);
            spaceShip.ChangeSpeed(1);
            yield return new WaitForSeconds(Random.Range(spawnPeriod, spawnPeriod + 0.1f));
        }
    }

    IEnumerator ScoreUpdater()
    {
        //Coroutine that add some points per 1 second
        while (1 == 1)
        {
            score += tickScore;
            sessionTime++;
            sessionTimeText.text = sessionTime.ToString();
            scoreText.text = score.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator CaroutineStarter()
    {
        //Used to start coroutines we need
        StartCoroutine("ScoreUpdater");
        StartCoroutine("SpawnAsteroid");
        yield return null;
    }

    float NextAsteroidTime()
    {
        //With this formula asteroid will spawn more often with time passed
        return (-Mathf.Sin(sessionTime - 60) / 40) + 1.5f;
    }

    public bool CheckAsteroidCoordinates(Vector3 asteroidCoord)
    {
        //Used to destroy asteroins we passed 
        if (spaceShip.transform.position.z - 10f > asteroidCoord.z)
            return true;
        else
            return false;
    }

    public void AddScoreForAsteroid()
    {
        //Increase parameters we need when asteroid passed
        score += 5;
        scoreText.text = score.ToString();
        passedAsteroids++;
        passedAsteroidsText.text = passedAsteroids.ToString();
    }

    public void AddBonusForSpeed(int bonus)
    {
        //Add bonus for score per second
        tickScore += bonus;
    }

    /*
     * Дополнительный комментарий по работе. 
     * Признаюсь предельно честно, это первый опыт работы с 3d сценами. Поэтому с добавлением шейдеров и моделями разбирался находу.
     * Поэтому и чутка испугался количетсво всего для планеты и решил не рисковать.i
     * Очень надеюсь, что в вашей команде получится сделать код более продуманным и чистым и разобраться нормально с моделями, а то негоже так тупить в гугле.
     * Спасибо за прочтение бреда соискателя :)
     */
}
