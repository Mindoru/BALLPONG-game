using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string Tag;
    [SerializeField] float secondsForDestroy = 5.0f;
    [SerializeField] float animTime = 1f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyAfterSeconds(secondsForDestroy));
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds - animTime);
        anim.SetBool("isDestroying_b", true);
        yield return new WaitForSeconds(animTime - (10 * animTime / 100)); // Espera a que la animación esté al 90%
        Destroy(gameObject);
    }
}