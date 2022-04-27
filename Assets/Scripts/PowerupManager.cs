using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] PlayerController Player;
    [SerializeField] GameObject PlayerAnimation;
    
    [Header("Speed Powerup")]
    [SerializeField] float speedAmount = 35f;
    [SerializeField] float speedTimeLimit = 2.5f;
    [SerializeField] GameObject speedPowerupHud;
    Timer speedTimerScript;
    [SerializeField] TextMeshProUGUI speedTimerText;
    [SerializeField] ParticleSystem speedParticle;

    [Header("Grow Powerup")]
    [SerializeField] [Tooltip("Grow time in seconds")] float scaleTime = 1f;
    [SerializeField] float scaleAmount = 0.35f;
    [SerializeField] float scaleTimeLimit = 4f;
    [SerializeField] GameObject growPowerupHud;
    Timer growTimerScript;
    [SerializeField] TextMeshProUGUI growTimerText;
    Vector2 playerInitialScale;
    Vector2 targetScale;

    [Header("Shoot Powerup")]
    [SerializeField] float shootRate = 2f;
    [SerializeField] [Range(3, 10)] float shootTimeLimit = 3f;
    [SerializeField] GameObject shootPowerupHud;
    Timer shootTimerScript;
    [SerializeField] TextMeshProUGUI shootTimerText;

    [Header("Shoot Pooling")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize = 5;
    [SerializeField] List<GameObject> bulletPool;
    [Space(20)]
    
    public static Dictionary<string, bool> hasPowerup = new Dictionary<string, bool>();

    bool allowFire = true;
    Vector3 offset = new Vector3(0.05f, 0, 1);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        playerInitialScale = Player.transform.localScale;
        targetScale = new Vector2(Player.transform.localScale.x, Player.transform.localScale.y + scaleAmount);
        if (bulletPrefab != null)
        {
            SimplePool.Preload(bulletPrefab, bulletPoolSize);
        }

        speedTimerScript = speedPowerupHud.GetComponent<Timer>();
        growTimerScript = growPowerupHud.GetComponent<Timer>();
        shootTimerScript = shootPowerupHud.GetComponent<Timer>();
    }

    // Speed powerup
    [ContextMenu("SpeedPowerup")]
    public void SpeedPowerup()
    {
        if (!hasPowerup.ContainsKey("speed"))
        {
            hasPowerup.Add("speed", true);
            Player.Speed += speedAmount;
            speedParticle.Play();
            speedPowerupHud.SetActive(true);
            speedTimerScript.Countdown(speedTimerText, speedTimeLimit);
            StartCoroutine(RemoveSpeedPowerup());
        }
    }

    IEnumerator RemoveSpeedPowerup()
    {
        yield return new WaitForSeconds(speedTimeLimit);
        Player.Speed -= speedAmount;
        speedParticle.Stop();
        speedPowerupHud.SetActive(false);
        hasPowerup.Remove("speed");
    }

    // Grow powerup
    [ContextMenu("GrowPowerup")]
    public void GrowPowerup()
    {
        if (!hasPowerup.ContainsKey("grow"))
        {
            hasPowerup.Add("grow", true);
            StartCoroutine(ChangeScale(Player.gameObject, playerInitialScale, targetScale, scaleTime));
            growPowerupHud.SetActive(true);
            growTimerScript.Countdown(growTimerText, scaleTimeLimit);
            StartCoroutine(RemoveGrowPowerup());
        }
    }

    IEnumerator RemoveGrowPowerup()
    {
        yield return new WaitForSeconds(scaleTimeLimit);
        StartCoroutine(ChangeScale(Player.gameObject, targetScale, playerInitialScale, scaleTime));
        growPowerupHud.SetActive(false);
        hasPowerup.Remove("grow");
    }

    // Shoot powerup
    [ContextMenu("ShootPowerup")]
    public void ShootPowerup()
    {
        if (!hasPowerup.ContainsKey("shoot"))
        {
            hasPowerup.Add("shoot", true);
            shootPowerupHud.SetActive(true);
            shootTimerScript.Countdown(shootTimerText, shootTimeLimit);
        }
        if (hasPowerup.ContainsKey("shoot"))
        {
            StartCoroutine(RemoveShootPowerup());
            if (Input.GetKeyDown(KeyCode.E) && allowFire)
            {
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        allowFire = false;
        SimplePool.Spawn(bulletPrefab, PlayerAnimation.transform.position + offset, PlayerAnimation.transform.rotation);
        yield return new WaitForSeconds(shootRate);
        allowFire = true;
    }

    IEnumerator RemoveShootPowerup()
    {
        yield return new WaitForSeconds(shootTimeLimit);
        AlternativePlayerController.shootPowerup = false;
        shootPowerupHud.SetActive(false);
        if (hasPowerup.ContainsKey("shoot"))
        {
            hasPowerup.Remove("shoot");
        }
    }

    // Other
    IEnumerator ChangeScale(GameObject _objectToScale, Vector2 _initialScale, Vector2 _targetScale, float _scaleTime)
    {
        float _timer = 0f;
        do
        {
            // timer / scaleTime get a value from 0 to 1 for scaling from initialScale to targetScale over time.
            // Example: (0 / 2 = 0. No scale), (1 / 2 = 0.5. Medium scale), (2 / 2 = 1. Target scale)
            _objectToScale.transform.localScale = Vector2.Lerp(_initialScale, _targetScale, _timer / _scaleTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        while (_timer <= _scaleTime);
    }
}
