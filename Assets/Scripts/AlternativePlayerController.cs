using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativePlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1.0f;
    Rigidbody2D rb;
    float xBound = 8.25f;
    public static bool shootPowerup = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        if (GameManager.Instance.isGameActive && Ball.Instance.IsBallMoving())
        {
            MovePlayer();
        }
        LimitBounds();
    }

    void Update()
    {
        if (shootPowerup)
        {
            PowerupManager.Instance.ShootPowerup();
        }
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
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);

            string tag = other.gameObject.GetComponent<Powerup>().Tag;
            switch (tag)
            {
                case "SpeedPowerup":
                    PowerupManager.Instance.SpeedPowerup();
                    break;
                case "ShootPowerup":
                    shootPowerup = true;
                    break;
            }
        }
    }
}
