using System.Collections;
using UnityEngine;

public class Help : MonoBehaviour
{
    [SerializeField] float upToDownAnimTime = 0.5f;
    [SerializeField] float destroyTime = 1.5f;
    Animator anim;
    
    int PointsToAdd;
    [SerializeField] int minPointsToAdd = 1;
    [SerializeField] int maxPointsToAdd = 3;

    void Start()
    {
        anim = GetComponent<Animator>();
        ShowHelpObject();
        StartCoroutine(DestroyHelpObject());
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
