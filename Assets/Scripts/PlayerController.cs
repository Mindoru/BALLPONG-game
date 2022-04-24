using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Componentes
    Animator anim;
    Rigidbody2D rb;
    
    // Valores por defecto
    public float Speed = 40f;
    bool isDodging;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance.isGameActive)
        {
            HandleDodge();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameActive)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector2.up * Speed, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(Vector2.down * Speed, ForceMode2D.Impulse);
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
