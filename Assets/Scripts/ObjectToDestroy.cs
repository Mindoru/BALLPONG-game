using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToDestroy : MonoBehaviour
{
    [SerializeField] float secondsForDestroy = 5.0f;
    [SerializeField] float animTime = 1f;
    GameManager gameManager;
    Animator anim;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyAfterSeconds(secondsForDestroy));
    }

    void Update()
    {
        if (gameManager.isPositionReset)
        {
            Destroy(gameObject);
            gameManager.SetIsPositionReset(false);
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds - animTime);
        anim.SetBool("isDestroying_b", true);
        yield return new WaitForSeconds(animTime);

        if (gameManager.isGameActive)
        {
            gameManager.AddScore("playerTwo", 1);
            gameManager.ResetPosition();
        }
    }
}
