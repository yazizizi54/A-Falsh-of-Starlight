using UnityEngine;

public class OrbCounter : MonoBehaviour
{
    public static OrbCounter Instance;

    [Header("目标")]
    [Tooltip("捡够多少个光球过关")]
    [SerializeField] int requiredCount = 30;

    int current;

    public int Current => current;
    public int Required => requiredCount;
    public float Progress => Mathf.Clamp01((float)current / requiredCount);

    public System.Action<int, int> OnChanged;
    public System.Action OnCleared;

    void Awake()
    {
        Instance = this;
    }

    public void Add(int amount = 1)
    {
        if (current >= requiredCount) return;

        current = Mathf.Min(current + amount, requiredCount);

        OnChanged?.Invoke(current, requiredCount);

        if (current >= requiredCount)
            OnCleared?.Invoke();
    }
}