using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Help : MonoBehaviour
{
    [SerializeField] float upToDownAnimTime = 0.5f;
    [SerializeField] float destroyTime = 1.5f;
    // PlayerController playerScript;
    Animator anim;
    
    int PointsToAdd;
    [SerializeField] int minPointsToAdd = 1;
    [SerializeField] int maxPointsToAdd = 3;

    void Start()
    {
        // playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        ShowHelpObject();
        Profiler.BeginSample("Destroying Bonus Object");
        StartCoroutine(DestroyHelpObject());
        Profiler.EndSample();
        PointsToAdd = Random.Range(minPointsToAdd, maxPointsToAdd);
    }

    void Update()
    {
        StartCoroutine(LetGetHelpAfterAnim());
    }

    void HandleHelp()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            GameManager.Instance.AddScore("playerOne", PointsToAdd);
            GameManager.Instance.ResetPosition();
            HideHelpObject();
            #if UNITY_EDITOR
                Debug.Log("El jugador obtuvo una ayuda!");
            #endif
            // playerScript.HasBonus = true;
        }
    }

    IEnumerator LetGetHelpAfterAnim()
    {
        yield return new WaitForSeconds(upToDownAnimTime);
        HandleHelp();
        yield break;
    }

    void ShowHelpObject()
    {
        anim.SetBool("isBonusTime_b", true);
    }

    void HideHelpObject()
    {
        anim.SetBool("isBonusTime_b", false);
    }

    IEnumerator DestroyHelpObject()
    {
        yield return new WaitForSeconds(destroyTime - upToDownAnimTime);
        HideHelpObject();
        yield return new WaitForSeconds(upToDownAnimTime);
        Destroy(gameObject);
    }
}
