using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerController : MonoBehaviour
{
    // Componentes
    Animator anim;
    
    // Valores por defecto
    public float Speed = 7f;
    bool isDodging;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            HandleMovement();
            Profiler.BeginSample("Handling Dodge");
            HandleDodge();
            Profiler.EndSample();
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
            else if (isDodging && !GameManager.Instance.enemyIncoming) // No uso un else {} para poder togglear la animación desde el editor
            {
                isDodging = false;
                anim.SetBool("isDodge_b", false);
            }
        }
    }
}
