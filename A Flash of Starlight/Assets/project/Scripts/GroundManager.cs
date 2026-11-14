using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("≈‰÷√")]
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] Transform player;
    [SerializeField] int chunkCount = 3;
    [SerializeField] float chunkWidth = 20f;
    [SerializeField] float startX = 0f;
    [SerializeField] float recycleAhead = 1.5f;

    readonly List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
        if (chunkPrefab == null)
        {
            Debug.LogError("[GroundManager] chunkPrefab Œ¥∏≥÷µ");
            return;
        }

        for (int i = 0; i < chunkCount; i++)
        {
            var go = Instantiate(
                chunkPrefab,
                new Vector3(startX + i * chunkWidth, 0f, 0f),
                Quaternion.identity,
                transform);
            chunks.Add(go);
        }
    }

    void Update()
    {
        if (player == null || chunks.Count == 0) return;

        var leftMost = chunks[0];

        if (player.position.x > leftMost.transform.position.x + chunkWidth * recycleAhead)
        {
            var rightMost = chunks[chunks.Count - 1];

            leftMost.transform.position = new Vector3(
                rightMost.transform.position.x + chunkWidth,
                rightMost.transform.position.y,
                rightMost.transform.position.z);

            chunks.RemoveAt(0);
            chunks.Add(leftMost);
        }
    }
}