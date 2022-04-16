using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Bullet speed. Recommended value is 10")]
    [SerializeField] float Speed = 10f;
    Rigidbody2D rb;
    string playerCollisionName = "Player Animation";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsMoving())
        {
            PushBullet();
        }
    }

    bool IsMoving()
    {
        return rb.velocity.magnitude != 0.0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.GoalTriggerEnter(other, true);
    }

    void OnCollisionEnter2D(Collision2D other) {
        var validCollision = other.gameObject.name != playerCollisionName && !other.gameObject.CompareTag("Wall");
        if (validCollision || other.gameObject.CompareTag("Goal") || other.gameObject.CompareTag("AlternativePlayer"))
        {
            Debug.Log("La bala colisionó con algo que no es Player");
            SimplePool.Despawn(gameObject);
        }
    }

    void PushBullet()
    {
        int yVelocity = Random.Range(0, 2) == 0 ? 1 : -1;
        rb.velocity = new Vector2(1f * Speed, yVelocity * Speed);
    }
}
