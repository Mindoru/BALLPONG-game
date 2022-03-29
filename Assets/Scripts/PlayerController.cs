using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // GameObjects
    [SerializeField] GameObject bulletPrefab;
    
    // Scripts
    GameManager gameManager;
    Ball ballScript;
    
    // Componentes
    Animator anim;
    
    // Valores por defecto
    [SerializeField] float Speed = 7f;
    [SerializeField] float BonusTime = 7f;
    bool isDodging;
    public bool HasBonus;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ballScript = GameObject.Find("Ball").GetComponent<Ball>();
        
        if (gameManager.isSetEnemyIncomingUI())
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (gameManager.isGameActive)
        {
            HandleMovement();
            HandleDodge();
            HandleFire();
            HandleBonus();
        }
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * Time.deltaTime * Speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * Time.deltaTime * Speed);
        }
    }

    void HandleDodge()
    {
        if (anim != null)
        {
            if (gameManager.enemyIncoming)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isDodging = !isDodging;
                    anim.SetBool("isDodge_b", !anim.GetBool("isDodge_b"));
                }
            }
            else
            {
                isDodging = false;
                anim.SetBool("isDodge_b", false);
            }
        }
    }

    void HandleFire()
    {
        if (HasBonus && !isDodging)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Fire());
            }
        }
    }

    void HandleBonus()
    {
        if (HasBonus)
        {
            StartCoroutine(DeleteBonus());
        }
    }

    IEnumerator Fire()
    {
        Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        yield return new WaitForSeconds(0.30f);
    }

    IEnumerator DeleteBonus()
    {
        yield return new WaitForSeconds(BonusTime);
        HasBonus = false;
        #if UNITY_EDITOR
            Debug.Log("El jugador no tiene más el Bonus.");
        #endif
    }
}
