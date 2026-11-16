using UnityEngine;
using TMPro;

public class OrbUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI orbText;

    void Start()
    {
        if (OrbCounter.Instance == null) return;

        OrbCounter.Instance.OnChanged += Refresh;
        Refresh(OrbCounter.Instance.Current, OrbCounter.Instance.Required);
    }

    void Refresh(int current, int required)
    {
        if (orbText != null)
            orbText.text = $"{current} / {required}";
    }
}