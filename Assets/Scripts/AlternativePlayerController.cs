using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativePlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1.0f;
    Ball ballScript;
    Rigidbody2D rb;
    GameManager gameManager;
    float xBound = 8.25f;

    void Start()
    {
        ballScript = GameObject.Find("Ball").GetComponent<Ball>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        
        if (gameManager.isGameActive && ballScript.IsBallMoving())
        {
            MovePlayer();
        }
        LimitBounds();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector2.right);
        }
    }

    void Move(Vector2 force)
    {
        rb.AddForce(force * movementSpeed, ForceMode2D.Force);
    }

    void LimitBounds()
    {
        if (transform.position.x >= xBound)
        {
            transform.position = new Vector2(xBound, transform.position.y);
            Move(Vector2.left);
        }
        if (transform.position.x <= -xBound)
        {
            transform.position = new Vector2(-xBound, transform.position.y);
            Move(Vector2.right);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ObjectToDestroy"))
        {
            Destroy(other.gameObject);
        }
    }
}
