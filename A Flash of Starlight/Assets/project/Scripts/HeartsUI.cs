using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [SerializeField] Image[] hearts;

    void Start()
    {
        if (PlayerHealth.Instance == null) return;

        PlayerHealth.Instance.OnHeartsChanged += Refresh;
        Refresh(PlayerHealth.Instance.Hearts, PlayerHealth.Instance.MaxHearts);
    }

    void Refresh(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].enabled = i < current;
        }
    }
}