using UnityEngine;

[ExecuteInEditMode]
public class PlanetRingBuilder : MonoBehaviour
{
    [Header("配置")]
    [SerializeField] Transform planetCenter;
    [SerializeField] GameObject groundPrefab;
    [SerializeField] float radius = 200f;
    [SerializeField] float chunkWidth = 20.48f;

    [Header("覆盖范围（度）")]
    [SerializeField] float startAngle = 0f;
    [SerializeField] float endAngle = 360f;

    [ContextMenu("生成圆环地面")]
    void BuildRing()
    {
        if (planetCenter == null)
        {
            Debug.LogWarning("[RingBuilder] PlanetCenter 未赋值");
            return;
        }
        if (groundPrefab == null)
        {
            Debug.LogWarning("[RingBuilder] Ground Prefab 未赋值");
            return;
        }

        // 清理旧块
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform c = transform.GetChild(i);
            if (c.name.StartsWith("RingChunk"))
                DestroyImmediate(c.gameObject);
        }

        float angleStep = Mathf.Rad2Deg * (chunkWidth / radius);
        int count = Mathf.CeilToInt((endAngle - startAngle) / angleStep);

        for (int i = 0; i < count; i++)
        {
            float angleDeg = startAngle + i * angleStep;
            float rad = angleDeg * Mathf.Deg2Rad;

            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 pos = planetCenter.position + dir * radius;

            GameObject go = Instantiate(groundPrefab, pos,
                Quaternion.Euler(0f, 0f, -angleDeg), transform);
            go.name = $"RingChunk_{i}";
        }

        Debug.Log($"[RingBuilder] 生成 {count} 块，{startAngle}°~{endAngle}°，每块 {angleStep:F2}°");
    }

    [ContextMenu("清除圆环地面")]
    void ClearRing()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform c = transform.GetChild(i);
            if (c.name.StartsWith("RingChunk"))
                DestroyImmediate(c.gameObject);
        }
        Debug.Log("[RingBuilder] 已清除");
    }
}