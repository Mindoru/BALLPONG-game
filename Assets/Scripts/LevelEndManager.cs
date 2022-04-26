using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    [SerializeField] bool lastLevel = false;
    [SerializeField] GameObject levelEndUI;
    [SerializeField] GameObject NextLevelTextBox;
    [SerializeField] GameObject MainMenuTextBox;

    void Update()
    {
        if (GameManager.Instance.levelPassed)
        {
            GameManager.Instance.levelPassed = false;
            if (lastLevel)
            {
                NextLevelTextBox.SetActive(false);
                MainMenuTextBox.SetActive(true);
            }
            GameManager.Instance.isGameActive = false;
            GameManager.Instance.canPause = false;
            GameManager.Instance.MenuUI.SetActive(false);
            levelEndUI.SetActive(true);
        }
    }
}
