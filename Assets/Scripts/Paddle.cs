using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    float yBound = 3.15f;

    void Update()
    {
        if (transform.position.y <= -yBound)
        {
            transform.position = new Vector2(transform.position.x, -yBound);
        }
        else if (transform.position.y >= yBound)
        {
            transform.position = new Vector2(transform.position.x, yBound);
        }
    }
}
