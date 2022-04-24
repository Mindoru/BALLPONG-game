using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D ballRb;
    float leftBoundMiddle = -4.075f;

    void FixedUpdate()
    {
        bool ballHasMovement = ballRb.velocity.x != 0.0f || ballRb.velocity.y != 0.0f;
        if (ballRb != null && ballHasMovement && GameManager.Instance.isGameActive)
        {
            if (ballRb.position.x > leftBoundMiddle)
            {
                HandleMovement();
            }
        }
    }

    void HandleMovement()
    {
        if (ballRb.position.y > transform.position.y)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
    }
}
