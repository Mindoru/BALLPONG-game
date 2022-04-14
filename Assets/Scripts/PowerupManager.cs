using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [Header("Speed Powerup")]
    [SerializeField] PlayerController Player;
    [SerializeField] float speedAmount = 3f;
    [SerializeField] float speedTimeLimit = 2.5f;

    [Header("Shoot Powerup")]
    [SerializeField] float shootRate = 2f;
    [SerializeField] [Range(3, 10)] float shootTimeLimit = 3f;

    [Header("Shoot Pooling")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize = 10;
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
        if (bulletPrefab != null)
        {
            SimplePool.Preload(bulletPrefab, bulletPoolSize);
        }
    }

    public void SpeedPowerup()
    {
        if (!hasPowerup.ContainsKey("speed"))
        {
            Player.Speed += speedAmount;
            hasPowerup.Add("speed", true);
            StartCoroutine(RemoveSpeedPowerup());
        }
    }

    IEnumerator RemoveSpeedPowerup()
    {
        yield return new WaitForSeconds(speedTimeLimit);
        Player.Speed -= speedAmount;
        hasPowerup.Remove("speed");
    }

    public void ShootPowerup()
    {
        if (!hasPowerup.ContainsKey("shoot"))
        {
            hasPowerup.Add("shoot", true);
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
        SimplePool.Spawn(bulletPrefab, Player.transform.position + offset, bulletPrefab.transform.rotation);
        yield return new WaitForSeconds(shootRate);
        allowFire = true;
    }

    IEnumerator RemoveShootPowerup()
    {
        yield return new WaitForSeconds(shootTimeLimit);
        AlternativePlayerController.shootPowerup = false;
        if (hasPowerup.ContainsKey("shoot"))
        {
            hasPowerup.Remove("shoot");
        }
    }
}
