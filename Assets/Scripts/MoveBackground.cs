using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] float Speed = 0.25f;
    Vector2 startPos;
    float sizeX;

    void Start()
    {
        sizeX = GetComponent<BoxCollider2D>().size.x / 2;
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        if (transform.position.x <= startPos.x - sizeX)
        {
            transform.position = startPos;
        }
    }
}