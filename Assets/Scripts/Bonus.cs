using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] float upToDownAnimTime = 0.5f;
    [SerializeField] float destroyTime = 3f;
    PlayerController playerScript;
    Animator anim;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        anim.SetBool("isBonusTime_b", true);
        StartCoroutine(DestroyBonus());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            #if UNITY_EDITOR
                Debug.Log("El jugador obtuvo un Bonus!");
            #endif
            playerScript.HasBonus = false;
        }
    }

    IEnumerator DestroyBonus()
    {
        yield return new WaitForSeconds(destroyTime - upToDownAnimTime);
        anim.SetBool("isBonusTime_b", false);
        yield return new WaitForSeconds(upToDownAnimTime);
        Destroy(gameObject);
    }
}
