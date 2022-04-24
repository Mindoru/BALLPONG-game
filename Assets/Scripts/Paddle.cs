using UnityEngine;

public class Paddle : MonoBehaviour
{
    float topBound = 3.15f;
    float bottomBound = 3.30f;

    void Update()
    {
        if (transform.position.y <= -bottomBound)
        {
            transform.position = new Vector2(transform.position.x, -bottomBound);
        }
        else if (transform.position.y >= topBound)
        {
            transform.position = new Vector2(transform.position.x, topBound);
        }
    }
}
