using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float Speed = 15f;

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * Speed);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Paddle") || other.gameObject.CompareTag("AlternativePlayer"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.GameOver();
        }
        else if (other.gameObject.CompareTag("Sensor"))
        {
            GameManager.Instance.enemyIncoming = false;
        }
    }
}
