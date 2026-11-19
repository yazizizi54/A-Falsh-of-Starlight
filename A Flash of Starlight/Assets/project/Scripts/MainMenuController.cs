using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string storySceneName = "Story";

    public void OnStartClicked()
    {
        if (ScreenFader.Instance != null)
            ScreenFader.Instance.LoadScene(storySceneName);
        else
            SceneManager.LoadScene(storySceneName);
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}