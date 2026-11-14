using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("跟随")]
    [SerializeField] Transform cam;
    [SerializeField, Range(0f, 1f)] float factor = 0.1f;

    [Header("无限滚动")]
    [SerializeField] float tileWidth = 21.72f;   // 一张图的世界宽度

    Transform[] tiles;
    float totalOffset;     // 累计滚动距离
    float lastCamX;

    void Start()
    {
        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;

        lastCamX = cam != null ? cam.position.x : 0f;

        // 收集所有子图
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            tiles[i] = transform.GetChild(i);
    }

    void LateUpdate()
    {
        if (cam == null || tiles.Length == 0 || tileWidth <= 0f) return;

        // 1. 累计相机位移，乘上视差系数
        float delta = cam.position.x - lastCamX;
        lastCamX = cam.position.x;

        // factor 越小，背景移动越少
        totalOffset += delta * factor;

        // 2. 整个父物体跟着相机走（保证不会跑出视野）
        Vector3 pos = transform.position;
        transform.position = new Vector3(cam.position.x, pos.y, pos.z);

        // 3. 逐块计算位置：从相机左侧开始，每块间隔 tileWidth
        //    并用取模把它“夹”在 [-tileWidth, +tileWidth] 范围内
        float startX = -tileWidth;

        for (int i = 0; i < tiles.Length; i++)
        {
            // 每块的基础位置
            float baseX = startX + i * tileWidth;

            // 加上滚动偏移，再用 Repeat 保证始终在有效的循环区间
            float offset = Mathf.Repeat(totalOffset, tileWidth);

            tiles[i].localPosition = new Vector3(baseX - offset, 0f, 0f);
        }
    }
}