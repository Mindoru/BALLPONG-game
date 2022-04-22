using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Bullet speed. Recommended value is 10")]
    [SerializeField] float Speed = 10f;
    Rigidbody2D rb;

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
        GameObject otherObj = other.gameObject;
        var validCollision = otherObj.GetComponent<PlayerController>() == null && !otherObj.CompareTag("Wall") || otherObj.CompareTag("Goal") || otherObj.CompareTag("AlternativePlayer");
        if (validCollision)
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
