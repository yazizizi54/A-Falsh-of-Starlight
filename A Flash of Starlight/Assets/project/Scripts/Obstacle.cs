using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("伤害")]
    [SerializeField] int damage = 1;

    [Header("撞到后")]
    [Tooltip("撞到后障碍物是否消失")]
    [SerializeField] bool destroyOnHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.TakeDamage(damage);

        if (destroyOnHit)
            gameObject.SetActive(false);
    }
}