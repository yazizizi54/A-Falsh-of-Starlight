using UnityEngine;

[ExecuteInEditMode]
public class PlanetRingBuilder : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("圆心，地面绕它排列")]
    [SerializeField] Transform planetCenter;

    [Tooltip("每一段的 Prefab（一整段地面）")]
    [SerializeField] GameObject segmentPrefab;

    [Header("圆环参数")]
    [Tooltip("圆的半径")]
    [SerializeField] float radius = 200f;

    [Tooltip("分成几段，12 = 每段 30 度")]
    [SerializeField] int segmentCount = 12;

    [Header("段的大小")]
    [Tooltip("每段的宽度（世界单位）")]
    [SerializeField] float segmentWidth = 0f;

    [Tooltip("勾上则用 segmentWidth 自动缩放 Prefab")]
    [SerializeField] bool autoScale = true;

    [Header("调试")]
    [SerializeField] string segmentPrefix = "RingSegment";

    [ContextMenu("生成圆环地面")]
    void BuildRing()
    {
        if (planetCenter == null)
        {
            Debug.LogWarning("[RingBuilder] Planet Center 未赋值");
            return;
        }
        if (segmentPrefab == null)
        {
            Debug.LogWarning("[RingBuilder] Segment Prefab 未赋值");
            return;
        }
        if (segmentCount < 3)
        {
            Debug.LogWarning("[RingBuilder] segmentCount 至少为 3");
            return;
        }

        ClearRing();

        float angleStep = 360f / segmentCount;

        // 每段的弦长（贴在圆上相邻两点之间的距离）
        float chord = 2f * radius * Mathf.Sin(angleStep * 0.5f * Mathf.Deg2Rad);

        // 实际要用的宽度
        float targetWidth = segmentWidth > 0f ? segmentWidth : chord;

        for (int i = 0; i < segmentCount; i++)
        {
            float angleDeg = i * angleStep;
            float rad = angleDeg * Mathf.Deg2Rad;

            // 圆心正上方为 0 度，顺时针为正
            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 pos = planetCenter.position + dir * radius;

            GameObject go = Instantiate(segmentPrefab, pos,
                Quaternion.Euler(0f, 0f, -angleDeg), transform);

            go.name = $"{segmentPrefix}_{i}";

            if (autoScale)
            {
                ScaleToWidth(go, targetWidth);
            }
        }

        Debug.Log($"[RingBuilder] 生成 {segmentCount} 段，每段 {angleStep:F2}°，" +
                  $"弦长 {chord:F2}，实际宽 {targetWidth:F2}，圆周 {2f * Mathf.PI * radius:F1}");
    }

    // 把对象缩放到指定世界宽度
    void ScaleToWidth(GameObject go, float targetWidth)
    {
        float currentWidth = GetWorldWidth(go);

        if (currentWidth <= 0.001f)
        {
            Debug.LogWarning($"[RingBuilder] {go.name} 宽度为 0，跳过缩放");
            return;
        }

        float factor = targetWidth / currentWidth;
        Vector3 s = go.transform.localScale;
        go.transform.localScale = new Vector3(s.x * factor, s.y, s.z);
    }

    // 读取对象的当前世界宽度（优先用 SpriteRenderer 的 bounds）
    float GetWorldWidth(GameObject go)
    {
        // 1. SpriteRenderer 的 bounds 最准
        var sr = go.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
            return sr.bounds.size.x;

        // 2. 退而求其次用 Collider
        var col = go.GetComponentInChildren<Collider2D>();
        if (col != null)
            return col.bounds.size.x;

        return 0f;
    }

    [ContextMenu("清除圆环")]
    void ClearRing()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform c = transform.GetChild(i);
            if (c.name.StartsWith(segmentPrefix))
                DestroyImmediate(c.gameObject);
        }
    }
}