using System.Collections.Generic;
using UnityEngine;

public class SegmentManager : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] List<GameObject> segmentPrefabs = new List<GameObject>();

    [Header("参数")]
    [Tooltip("同时存在几段")]
    [SerializeField] int activeSegments = 4;

    [Tooltip("每段宽度（Prefab 的实际宽度）")]
    [SerializeField] float segmentWidth = 20f;

    [Tooltip("每段起始生成的 X")]
    [SerializeField] float startX = 20f;

    [Tooltip("往左移动的速度")]
    [SerializeField] float scrollSpeed = 6f;

    readonly List<GameObject> active = new List<GameObject>();
    float nextSpawnX;

    void Start()
    {
        if (segmentPrefabs == null || segmentPrefabs.Count == 0)
        {
            Debug.LogError("[SegmentManager] 没有配置路段 Prefab");
            return;
        }

        nextSpawnX = startX;

        for (int i = 0; i < activeSegments; i++)
            SpawnSegment();
    }

    void Update()
    {
        if (active.Count == 0) return;

        float move = scrollSpeed * Time.deltaTime;

        // 所有路段往左移
        for (int i = 0; i < active.Count; i++)
        {
            Transform t = active[i].transform;
            t.position += Vector3.left * move;
        }

        // 最左边那段移出视野 → 回收
        GameObject leftMost = active[0];
        if (leftMost.transform.position.x < -segmentWidth * 1.5f)
        {
            active.RemoveAt(0);
            Destroy(leftMost);

            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        // 找当前最右边的路段位置
        float spawnX = startX;

        if (active.Count > 0)
        {
            float maxX = float.MinValue;
            foreach (var g in active)
            {
                if (g.transform.position.x > maxX)
                    maxX = g.transform.position.x;
            }
            spawnX = maxX + segmentWidth;
        }

        GameObject prefab = segmentPrefabs[Random.Range(0, segmentPrefabs.Count)];

        GameObject go = Instantiate(prefab,
            new Vector3(spawnX, 0f, 0f),
            Quaternion.identity,
            transform);

        go.name = prefab.name + "_inst";
        active.Add(go);
    }
}