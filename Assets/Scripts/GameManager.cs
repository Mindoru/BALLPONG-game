using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // GameObjects prefabs
    [SerializeField] GameObject[] powerups;
    [SerializeField] GameObject player;
    [SerializeField] GameObject AIEnemy;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject helpPrefab;

    // Valores iniciales de los GameObjects
    Vector2 playerInitialPos;
    Quaternion playerInitialRotation;
    Vector2 AIEnemyInitialPos;
    Vector2 ballInitialPos;

    // Interfaz gráfica
    [SerializeField] GameObject noMenuUI;
    [SerializeField] GameObject menuUIIncludingOptions;
    public GameObject MenuUI { get => menuUIIncludingOptions; }
    [SerializeField] GameObject scoreUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TextMeshProUGUI playerOneScoreText;
    [SerializeField] TextMeshProUGUI playerTwoScoreText;
    [SerializeField] Button restartButton;
    [SerializeField] GameObject enemyIncomingUI;
    public GameObject EnemyIncomingUI { get => enemyIncomingUI; private set => enemyIncomingUI = value; }

    // Animación
    Animator enemyIncomingAnim;
    
    // Sonidos
    [SerializeField] AudioClip pingSound;
    AudioSource audioSource;
     
    // Valores por defecto
    public int levelIndex;
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    public int maxScore = 3;
    public bool levelPassed = false;
    public bool canPause = true;
    bool isGamePaused;
    public bool isGameActive = true;
    public bool enemyIncoming;
    public bool isPositionReset;

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
        UseSound("bgMusic");
        SetScoreTextToScore();
        SaveInitialPos();
        
        HandlePowerup();
        HandleEnemy();
        HandleHelp();

        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        CheckScore();
        ToggleMenu();
    }

    void SetScoreTextToScore()
    {
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
    }

    void HandlePowerup()
    {
        Dictionary<string, float> powerupTimes = new Dictionary<string, float>()
        {
            { "startFrom", 5f },
            { "startTo", 15f },
            { "rateFrom", 5f },
            { "rateTo", 15f }
        };
        float[] powerupRandTimes = {
            Random.Range(powerupTimes["startFrom"], powerupTimes["startTo"]),
            Random.Range(powerupTimes["rateFrom"], powerupTimes["rateTo"]),
        };
        InvokeRepeating("SpawnPowerup", powerupRandTimes[0], powerupRandTimes[1]);
    }

    void HandleEnemy()
    {
        if (EnemyIncomingUI != null)
        {
            Dictionary<string, float> enemyTimes = new Dictionary<string, float>()
            {
                { "startFrom", 7f },
                { "startTo", 20f },
                { "rateFrom", 10f },
                { "rateTo", 15f }
            };
            float[] enemyRandTimes = {
                Random.Range(enemyTimes["startFrom"], enemyTimes["startTo"]),
                Random.Range(enemyTimes["rateFrom"], enemyTimes["rateTo"]),
            };
            enemyIncomingAnim = EnemyIncomingUI.GetComponent<Animator>();
            InvokeRepeating("SpawnEnemy", enemyRandTimes[0], enemyRandTimes[1]);
        }
    }

    void HandleHelp()
    {
        if (helpPrefab != null)
        {
            Dictionary<string, float> helpTimes = new Dictionary<string, float>()
            {
                { "startFrom", 20f },
                { "startTo", 30f },
                { "rateFrom", 40f },
                { "rateTo", 90f }
            };
            float[] helpRandTimes = {
                Random.Range(helpTimes["startFrom"], helpTimes["startTo"]),
                Random.Range(helpTimes["rateFrom"], helpTimes["rateTo"]),
            };
            InvokeRepeating("SpawnHelp", helpRandTimes[0], helpRandTimes[1]);
        }
    }

    void SpawnHelp()
    {
        if (isGameActive && Ball.Instance.IsBallMoving())
        {
            Instantiate(helpPrefab);
        }
    }

    void SpawnPowerup()
    {
        if (isGameActive && Ball.Instance.IsBallMoving())
        {
            int index = Random.Range(0, powerups.Length);
            Instantiate(powerups[index], RandomPosition("powerup"), powerups[index].transform.rotation);
        }
    }

    void SpawnEnemy()
    {
        if (isGameActive && Ball.Instance.IsBallMoving())
        {
            EnemyIncomingUI.SetActive(true);
            enemyIncomingAnim.SetBool("enemyIncoming_b", true);
            enemyIncoming = true;
            StartCoroutine(HideEnemyIncomingText());
            StartCoroutine(ShowEnemy());
        }
    }

    void SaveInitialPos()
    {
        playerInitialPos = player.transform.position;
        playerInitialRotation = player.transform.rotation;
        AIEnemyInitialPos = AIEnemy.transform.position;
        ballInitialPos = ball.transform.position;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToggleMenu()
    {
        if (menuUIIncludingOptions != null && noMenuUI != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && canPause)
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
    
    void CheckScore()
    {
        SetScoreTextToScore();
        if (isGameActive)
        {
            if (playerOneScore >= maxScore)
            {
                // TODO: Mostrar opciones para pasar al siguiente nivel, seleccionar nivel o ir al menú principal
                levelPassed = true;
            }
            else if (playerTwoScore >= maxScore)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        scoreUI.SetActive(false);
        if (EnemyIncomingUI != null)
        {
            EnemyIncomingUI.SetActive(false);
        }
        gameOverUI.SetActive(true);
    }

    public void UseSound(string name)
    {
        switch (name)
        {
            case "pingSound":
                audioSource.PlayOneShot(pingSound, 1f);
                break;
            default:
                break;
        }
    }

    public void AddScore(string addTo, int amount)
    {
        if (addTo == "playerOne")
        {
            playerOneScore += amount;
            return;
        }
        else if (addTo == "playerTwo")
        {
            playerTwoScore += amount;
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
        Ball.Instance.StopBall();
        PowerupManager.hasPowerup.Remove("shoot");
        AlternativePlayerController.shootPowerup = false;
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

    public void GoalTriggerEnter(Collider2D other, bool isBullet = false)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            GameManager.Instance.ResetPosition();
            if (!isBullet)
            {
                if (other.gameObject.name == "Left Goal")
                {
                    GameManager.Instance.AddScore("playerTwo", 1);
                    return;
                }
            }
            
            if (other.gameObject.name == "Right Goal")
            {
                GameManager.Instance.AddScore("playerOne", 1);
                return;
            }
        }
    }

    IEnumerator HideEnemyIncomingText()
    {
        yield return new WaitForSeconds(1f);
        enemyIncomingAnim.SetBool("enemyIncoming_b", false);
        EnemyIncomingUI.SetActive(false);
    }

    IEnumerator ShowEnemy()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(enemy);
    }

    Vector2 RandomPosition(string getFor)
    {
        if (getFor == "powerup")
        {
            float powerupXBound = 5.0f;
            float powerupYBound = 2.5f;
            float xPosition = Random.Range(-powerupXBound, powerupXBound);
            float yPosition = Random.Range(-powerupYBound, powerupYBound);
            return new Vector2(xPosition, yPosition);
        }
        return new Vector2(0, 0);
    }
}
