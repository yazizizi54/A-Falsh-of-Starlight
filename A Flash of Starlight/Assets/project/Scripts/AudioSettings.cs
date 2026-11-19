using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle fullscreenToggle;

    [Header("值（运行时）")]
    [Range(0f, 1f)] public float musicVolume = 0.8f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;
    public bool isFullscreen = true;

    void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        }

        Apply();
    }

    void OnMusicChanged(float v)
    {
        musicVolume = v;
    }

    void OnSFXChanged(float v)
    {
        sfxVolume = v;
    }

    void OnFullscreenChanged(bool v)
    {
        isFullscreen = v;
        ApplyScreen();
    }

    void Apply()
    {
        ApplyScreen();
    }

    void ApplyScreen()
    {
        if (isFullscreen)
        {
            Screen.SetResolution(
                Display.main.systemWidth,
                Display.main.systemHeight,
                FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }

        Debug.Log($"[Settings] 全屏={isFullscreen}, 分辨率={Screen.width}x{Screen.height}, 模式={Screen.fullScreenMode}");
    }
}