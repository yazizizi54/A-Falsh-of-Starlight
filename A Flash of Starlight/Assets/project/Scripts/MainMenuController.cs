using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string gameplaySceneName = "Gameplay";

    public void OnStartClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnQuitClicked()
    {
        Debug.Log("ÍË³öÓÎÏ·");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}