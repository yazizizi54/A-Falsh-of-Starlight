using System.Collections.Generic;
using UnityEngine;

public class PlanetGroundSpawner : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] Transform planetCenter;
    [SerializeField] GameObject chunkPrefab;

    [Header("参数")]
    [SerializeField] float radius = 50f;
    [SerializeField] float chunkWidth = 20.48f;
    [SerializeField] int chunkCount = 5;

    [Header("回收阈值（度）")]
    [Tooltip("超过这个角度就挪到队列另一端")]
    [SerializeField] float recycleAngle = 60f;

    readonly List<Transform> chunks = new List<Transform>();
    float angleStep;

    void Start()
    {
        angleStep = Mathf.Rad2Deg * (chunkWidth / radius);

        for (int i = 0; i < chunkCount; i++)
        {
            float angleDeg = (i - (chunkCount - 1) / 2f) * angleStep;
            Transform t = SpawnAt(angleDeg);
            chunks.Add(t);
        }

        Debug.Log($"[Spawner] 生成 {chunks.Count} 块，每块角度 {angleStep:F2}°");
    }

    void Update()
    {
        if (chunks.Count == 0) return;

        for (int i = 0; i < chunks.Count; i++)
        {
            Transform t = chunks[i];

            // 当前这块地相对圆心的角度（0 = 正上方）
            Vector3 dir = (t.position - planetCenter.position).normalized;
            float currentAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            // 超出回收阈值 → 挪到队列最前沿
            if (Mathf.Abs(currentAngle) > recycleAngle)
            {
                float farAngle = GetFarEdgeAngle();
                float newAngle = farAngle + angleStep;

                // 防止角度无限累加，超出范围就绕回
                if (newAngle > recycleAngle)
                {
                    newAngle = -recycleAngle - angleStep * 0.5f;
                }

                PlaceAt(t, newAngle);
            }
        }
    }

    // 队列里角度最大的那块（最右侧）
    float GetFarEdgeAngle()
    {
        float maxAngle = -999f;
        foreach (var t in chunks)
        {
            Vector3 dir = (t.position - planetCenter.position).normalized;
            float a = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            if (a > maxAngle) maxAngle = a;
        }
        return maxAngle;
    }

    // 把已有的 Transform 摆到指定角度
    void PlaceAt(Transform t, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        t.position = planetCenter.position
            + new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;
        t.rotation = Quaternion.Euler(0f, 0f, -angleDeg);
    }

    // 创建一块新地面
    Transform SpawnAt(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 pos = planetCenter.position
            + new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        GameObject go = Instantiate(chunkPrefab, pos,
            Quaternion.Euler(0f, 0f, -angleDeg), transform);

        return go.transform;
    }

    // 编辑器里预览排布（不需要 Play）
    [ContextMenu("生成地面（预览）")]
    void PreviewInEditor()
    {
        if (planetCenter == null || chunkPrefab == null)
        {
            Debug.LogWarning("[Spawner] PlanetCenter 或 ChunkPrefab 未赋值");
            return;
        }

        angleStep = Mathf.Rad2Deg * (chunkWidth / radius);

        for (int i = 0; i < chunkCount; i++)
        {
            float angleDeg = (i - (chunkCount - 1) / 2f) * angleStep;
            SpawnAt(angleDeg);
        }

        Debug.Log($"[Spawner] 预览生成 {chunkCount} 块，每块角度 {angleStep:F2}°");
    }
}