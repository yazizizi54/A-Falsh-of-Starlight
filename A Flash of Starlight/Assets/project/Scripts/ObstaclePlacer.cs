using UnityEngine;

public class ObstaclePlacer : MonoBehaviour
{
    [SerializeField] Transform planetCenter;
    [SerializeField] float radius = 50f;

    [ContextMenu("把障碍物摆到弧面上")]
    void PlaceObstacles()
    {
        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("Obstacle")) continue;

            // 用 X 偏移推算角度
            float x = child.localPosition.x;
            float angleRad = Mathf.Asin(Mathf.Clamp(x / radius, -1f, 1f));
            float angleDeg = angleRad * Mathf.Rad2Deg;

            Vector3 dir = new Vector3(Mathf.Sin(angleRad), Mathf.Cos(angleRad), 0f);
            Vector3 groundPos = planetCenter.position + dir * radius;

            // 障碍物底部贴地 → 抬高半个障碍物高度
            float halfHeight = 0.75f;
            child.position = groundPos + Vector3.up * halfHeight;
            child.rotation = Quaternion.Euler(0, 0, -angleDeg);

            Debug.Log($"{child.name}: 角度 {angleDeg:F1}°, 位置 {child.position}");
        }
    }
}