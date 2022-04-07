using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerController : MonoBehaviour
{
    // GameObjects
    // [SerializeField] GameObject bulletPrefab;
    
    // Componentes
    Animator anim;
    
    // Valores por defecto
    // [SerializeField] float BonusTime = 7f;
    public float Speed = 7f;
    bool isDodging;
    // public bool HasBonus;

    void Start()
    {
        if (GameManager.Instance.EnemyIncomingUI)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            HandleMovement();
            Profiler.BeginSample("Handling Dodge");
            HandleDodge();
            Profiler.EndSample();
            // Profiler.BeginSample("Handling Fire");
            // HandleFire();
            // Profiler.EndSample();
            // Profiler.BeginSample("Handling Bonus");
            // HandleBonus();
            // Profiler.EndSample();
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
            if (GameManager.Instance.enemyIncoming)
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

    // void HandleFire()
    // {
    //     if (HasBonus && !isDodging)
    //     {
    //         if (Input.GetKeyDown(KeyCode.E))
    //         {
    //             StartCoroutine(Fire());
    //         }
    //     }
    // }

    // void HandleBonus()
    // {
    //     if (HasBonus)
    //     {
    //         StartCoroutine(RemoveBonus());
    //     }
    // }

    // IEnumerator Fire()
    // {
    //     Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
    //     yield return new WaitForSeconds(0.30f);
    // }

    // IEnumerator RemoveBonus()
    // {
    //     yield return new WaitForSeconds(BonusTime);
    //     HasBonus = false;
    //     #if UNITY_EDITOR
    //         Debug.Log("El jugador no tiene más el Bonus.");
    //     #endif
    // }
}
