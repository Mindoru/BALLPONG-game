using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance;

    [SerializeField] float Speed = 3f;
    [SerializeField] float SpeedMultiplier = 1.05f;
    
    [HideInInspector]
    public bool isBallMoving;

    Rigidbody2D rb;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isBallMoving = false;
    }

    void Update()
    {
        if (GameManager.Instance.isGameActive && !IsBallMoving())
        {        
            if (Input.GetKeyDown(KeyCode.V))
            {
                PushBall();
            }
        }
    }

    public void PushBall()
    {
        float xVelocity = Random.Range(0, 2) == 0 ? 1 : -1;
        float yVelocity = Random.Range(0, 2) == 0 ? 1 : -1;
        rb.velocity = new Vector2(xVelocity * Speed, yVelocity * Speed);
        var impulse = (360f * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }

    public void StopBall()
    {
        
        rb.velocity = Vector2.zero;
    }

    public bool IsBallMoving()
    {
        isBallMoving = rb.velocity.x != 0.0f || rb.velocity.y != 0.0f;
        return isBallMoving;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.GoalTriggerEnter(other);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            rb.velocity *= SpeedMultiplier;
            GameManager.Instance.UseSound("pingSound");
        }
    }
}
