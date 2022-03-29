using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed = 30f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector2.right * Speed);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ObjectToDestroy"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<AIEnemy>())
        {
            Destroy(gameObject);
        }
    }
}
