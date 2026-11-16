using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("ÉúÃü")]
    [SerializeField] int maxHearts = 3;
    [SerializeField] float invincibleDuration = 1.5f;

    [Header("ÒýÓÃ")]
    [SerializeField] SpriteRenderer bodyRenderer;

    int hearts;
    bool invincible;

    public int Hearts => hearts;
    public int MaxHearts => maxHearts;
    public bool IsInvincible => invincible;

    public System.Action<int, int> OnHeartsChanged;
    public System.Action OnDied;

    void Awake()
    {
        Instance = this;
        hearts = maxHearts;

        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int amount = 1)
    {
        if (invincible || hearts <= 0) return;

        hearts = Mathf.Max(0, hearts - amount);
        OnHeartsChanged?.Invoke(hearts, maxHearts);

        if (hearts <= 0)
        {
            OnDied?.Invoke();
            return;
        }

        StartCoroutine(InvincibleRoutine());
    }

    IEnumerator InvincibleRoutine()
    {
        invincible = true;

        float elapsed = 0f;
        while (elapsed < invincibleDuration)
        {
            if (bodyRenderer != null)
                bodyRenderer.enabled = !bodyRenderer.enabled;

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (bodyRenderer != null)
            bodyRenderer.enabled = true;

        invincible = false;
    }
}