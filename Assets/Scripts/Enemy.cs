using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float Speed = 15f;
    GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * Speed);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Paddle") || other.gameObject.CompareTag("AlternativePlayer"))
        {
            Destroy(other.gameObject);
            gameManager.GameOver();
        }
        else if (other.gameObject.CompareTag("Sensor"))
        {
            gameManager.SetEnemyIncoming(false);
        }
    }
}
