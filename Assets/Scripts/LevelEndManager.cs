using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    [SerializeField] GameObject levelEndUI;
    
    void Update()
    {
        if (GameManager.Instance.levelPassed)
        {
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.canPause = false;
            GameManager.Instance.MenuUI.SetActive(false);
            levelEndUI.SetActive(true);
        }
    }
}
