using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance;

    [Header("面板")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelClearPanel;

    [Header("场景")]
    [Tooltip("过关后进入的场景，留空则显示面板")]
    [SerializeField] string nextSceneName = "";

    bool ended;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (levelClearPanel != null) levelClearPanel.SetActive(false);

        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnDied += OnPlayerDied;

        if (OrbCounter.Instance != null)
            OrbCounter.Instance.OnCleared += OnLevelCleared;
    }

    void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.OnDied -= OnPlayerDied;

        if (OrbCounter.Instance != null)
            OrbCounter.Instance.OnCleared -= OnLevelCleared;
    }

    void OnPlayerDied()
    {
        if (ended) return;
        ended = true;

        Debug.Log("[GameFlow] 玩家死亡");

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnLevelCleared()
    {
        if (ended) return;
        ended = true;

        Debug.Log("[GameFlow] 过关");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        if (levelClearPanel != null) levelClearPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ---------- 按钮调用 ----------
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}