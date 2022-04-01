using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // GameObjects prefabs
    [SerializeField] GameObject[] blockPrefabs;
    [SerializeField] GameObject player;
    [SerializeField] GameObject AIEnemy;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bonusPrefab;

    // Valores iniciales de los GameObjects
    Vector2 playerInitialPos;
    Quaternion playerInitialRotation;
    Vector2 AIEnemyInitialPos;
    Vector2 ballInitialPos;

    // Scripts
    Ball ballScript;

    // Interfaz gráfica
    [SerializeField] GameObject noMenuUI;
    [SerializeField] GameObject menuUIIncludingOptions;
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject enemyIncomingUI;
    [SerializeField] TextMeshProUGUI playerOneScore;
    [SerializeField] TextMeshProUGUI playerTwoScore;
    [SerializeField] Button restartButton;

    // Animación
    Animator enemyIncomingAnim;
    
    // Sonidos
    [SerializeField] AudioClip bgMusic;
    [SerializeField] AudioClip pingSound;
    AudioSource audioSource;
     
    // Valores por defecto
    public int levelIndex;
    public int playerOneCurrentScore;
    public int playerTwoCurrentScore;
    public int maxScore = 3;
    [SerializeField] float spawnEnemyTimeStart = 7f;
    [SerializeField] float spawnEnemyTimeRate = 20f;
    [SerializeField] float spawnBonusTimeStartFrom = 10f;
    [SerializeField] float spawnBonusTimeStartTo = 15f;
    [SerializeField] float spawnBonusTimeRateFrom = 30f;
    [SerializeField] float spawnBonusTimeRateTo = 40f;
    bool isGamePaused;
    public bool isGameActive = true;
    public bool enemyIncoming;
    public bool isPositionReset;
    float blockXBound = 5.0f;
    float blockYBound = 2.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ballScript = ball.GetComponent<Ball>();
        isGameActive = true;
        Profiler.BeginSample("Using Music");
        UseSound("bgMusic");
        Profiler.EndSample();

        Profiler.BeginSample("Getting Initial Positions");
        playerInitialPos = player.transform.position;
        playerInitialRotation = player.transform.rotation;
        AIEnemyInitialPos = AIEnemy.transform.position;
        ballInitialPos = ball.transform.position;
        Profiler.EndSample();

        InvokeRepeating("SpawnBlocks", 2f, 5f);
        
        if (enemyIncomingUI != null)
        {
            enemyIncomingAnim = enemyIncomingUI.GetComponent<Animator>();
            InvokeRepeating("SpawnEnemy", spawnEnemyTimeStart, spawnEnemyTimeRate);
        }

        if (bonusPrefab != null)
        {
            float spawnBonusTimeStart = Random.Range(spawnBonusTimeStartFrom, spawnBonusTimeStartTo);
            float spawnBonusTimeRate = Random.Range(spawnBonusTimeRateFrom, spawnBonusTimeRateTo);
            InvokeRepeating("SpawnBonus", spawnBonusTimeStart, spawnBonusTimeRate);
        }

        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        Profiler.BeginSample("Checking Score");
        CheckScore();
        Profiler.EndSample();

        Profiler.BeginSample("Toggling Menu");
        ToggleMenu();
        Profiler.EndSample();
    }

    public void GameOver()
    {
        isGameActive = false;
        scoreUI.SetActive(false);
        if (enemyIncomingUI != null)
        {
            enemyIncomingUI.SetActive(false);
        }
        gameOverUI.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UseSound(string name)
    {
        switch (name)
        {
            case "pingSound":
                audioSource.PlayOneShot(pingSound, 1f);
                break;
            case "bgMusic":
                audioSource.PlayOneShot(bgMusic, 1f);
                break;
            default:
                break;
        }
    }

    void CheckScore()
    {
        if (isGameActive)
        {
            playerOneCurrentScore = Int32.Parse(playerOneScore.text);
            playerTwoCurrentScore = Int32.Parse(playerTwoScore.text);
            if (playerOneCurrentScore >= maxScore)
            {
                // TODO: Mostrar opciones para pasar al siguiente nivel, seleccionar nivel o ir al menú principal
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (playerTwoCurrentScore >= maxScore)
            {
                GameOver();
            }
        }
    }

    public void AddScore(string addTo, int amount)
    {
        if (addTo == "playerOne")
        {
            int score = Int32.Parse(playerOneScore.text);
            score += amount;
            playerOneScore.text = score.ToString();
            return;
        }
        else if (addTo == "playerTwo")
        {
            int score = Int32.Parse(playerTwoScore.text);
            score += amount;
            playerTwoScore.text = score.ToString();
            return;
        }
    }

    public void ResetPosition()
    {
        isPositionReset = true;
        if (player != null)
        {
            player.transform.position = playerInitialPos;
            player.transform.rotation = playerInitialRotation;
        }
        AIEnemy.transform.position = AIEnemyInitialPos;
        ball.transform.position = ballInitialPos;
        ballScript.StopBall();
    }

    public void SetIsPositionReset(bool value)
    {
        isPositionReset = value;
    }

    void SpawnBonus()
    {
        if (isGameActive && ballScript.IsBallMoving())
        {
            Instantiate(bonusPrefab);
        }
    }

    void SpawnBlocks()
    {
        if (isGameActive && ballScript.IsBallMoving())
        {
            int index = Random.Range(0, blockPrefabs.Length);
            Instantiate(blockPrefabs[index], RandomPosition("block"), blockPrefabs[index].transform.rotation);
        }
    }

    void SpawnEnemy()
    {
        if (isGameActive && ballScript.IsBallMoving())
        {
            enemyIncomingUI.SetActive(true);
            enemyIncomingAnim.SetBool("enemyIncoming_b", true);
            SetEnemyIncoming(true);
            StartCoroutine(HideEnemyIncomingText());
            StartCoroutine(ShowEnemy());
        }
    }

    // Cambia el valor de enemyIncoming
    public void SetEnemyIncoming(bool value)
    {
        enemyIncoming = value;
    }

    IEnumerator HideEnemyIncomingText()
    {
        yield return new WaitForSeconds(1f);
        enemyIncomingAnim.SetBool("enemyIncoming_b", false);
        enemyIncomingUI.SetActive(false);
    }

    IEnumerator ShowEnemy()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(enemy);
    }

    Vector2 RandomPosition(string getFor)
    {
        if (getFor == "block")
        {
            float xPosition = Random.Range(-blockXBound, blockXBound);
            float yPosition = Random.Range(-blockYBound, blockYBound);
            return new Vector2(xPosition, yPosition);
        }
        return new Vector2(0, 0);
    }

    public bool isSetEnemyIncomingUI()
    {
        return enemyIncomingUI ? true : false;
    }

    void ToggleMenu()
    {
        if (menuUIIncludingOptions != null && noMenuUI != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        ChangeAudioState("pause");
        Time.timeScale = 0f;
        noMenuUI.SetActive(false);
        menuUIIncludingOptions.SetActive(true);
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        ChangeAudioState("resume");
        Time.timeScale = 1f;
        noMenuUI.SetActive(true);
        menuUIIncludingOptions.SetActive(false);
        isGamePaused = false;
    }

    void ChangeAudioState(string toState)
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        if (toState == "pause")
        {
            foreach (AudioSource audio in audios)
            {
                audio.Pause();
            }
        }
        else if (toState == "resume")
        {
            foreach (AudioSource audio in audios)
            {
                audio.UnPause();
            }
        }
    }
}
