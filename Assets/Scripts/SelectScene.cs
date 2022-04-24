using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => LoadScene(sceneName));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
