using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Bullet speed. Recommended value is 0.75")]
    [SerializeField] float Speed = 0.75f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector2.right * Speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.GoalTriggerEnter(other, true);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name != "Player Animation" && !other.gameObject.CompareTag("AlternativePlayer"))
        {
            Debug.Log("La bala colisionó con algo que no es Player");
            SimplePool.Despawn(gameObject);
        }
    }
}
