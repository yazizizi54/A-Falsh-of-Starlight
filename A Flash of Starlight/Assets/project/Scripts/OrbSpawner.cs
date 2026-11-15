using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] Transform planetCenter;
    [SerializeField] Transform planetRing;      // 光球挂到哪个父物体下
    [SerializeField] GameObject orbPrefab;

    [Header("圆环参数")]
    [SerializeField] float radius = 200f;

    [Header("生成范围（度）")]
    [Tooltip("从圆顶开始，正负各多少度")]
    [SerializeField] float angleRange = 180f;

    [Header("数量与间距")]
    [SerializeField] int clusterCount = 12;     // 生成多少簇
    [SerializeField] int orbsPerCluster = 5;    // 每簇几个球
    [SerializeField] float clusterSpacing = 1.5f; // 簇内球的间距（世界单位）
    [SerializeField] float orbHeight = 1.3f;    // 离地高度

    [Header("随机")]
    [SerializeField] int seed = 0;

    readonly List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        Generate();
    }

    [ContextMenu("生成光球（可重复调用）")]
    public void Generate()
    {
        Clear();
        if (planetCenter == null || orbPrefab == null) return;

        if (seed != 0) Random.InitState(seed);

        for (int c = 0; c < clusterCount; c++)
        {
            // 这一簇在圆周上的角度
            float angleDeg = Random.Range(-angleRange, angleRange);

            for (int i = 0; i < orbsPerCluster; i++)
            {
                // 簇内偏移（沿弧线方向）
                float offsetArc = (i - (orbsPerCluster - 1) * 0.5f) * clusterSpacing;
                float offsetDeg = Mathf.Rad2Deg * (offsetArc / radius);

                float a = angleDeg + offsetDeg;
                float rad = a * Mathf.Deg2Rad;

                Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
                Vector3 pos = planetCenter.position + dir * (radius + orbHeight);

                GameObject go = Instantiate(orbPrefab, pos, Quaternion.identity,
                    planetRing != null ? planetRing : transform);
                go.name = $"Orb_{c}_{i}";
                spawned.Add(go);
            }
        }

        Debug.Log($"[OrbSpawner] 生成 {spawned.Count} 个光球");
    }

    void Clear()
    {
        foreach (var g in spawned)
        {
            if (g != null) DestroyImmediate(g);
        }
        spawned.Clear();
    }

    [ContextMenu("清除光球")]
    void ClearAll()
    {
        Clear();
    }
}