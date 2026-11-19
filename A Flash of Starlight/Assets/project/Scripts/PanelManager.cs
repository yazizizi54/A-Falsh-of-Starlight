using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;

    void Awake() => Instance = this;

    void Start()
    {
        CloseAll();
    }

    public void OpenSettings()
    {
        CloseAll();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        CloseAll();
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseAll()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
}