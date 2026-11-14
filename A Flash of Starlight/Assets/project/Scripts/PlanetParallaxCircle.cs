using UnityEngine;

public class PlanetParallaxCircle : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] Transform planetCenter;   // 圆心
    [SerializeField] Transform groundRoot;     // 地面根节点（提供转速参考）

    [Header("视差")]
    [SerializeField, Range(0f, 1f)] float factor = 0.1f;

    [Header("背景参数")]
    [SerializeField] float bgRadius = 55f;     // 背景绕圆的半径（越大越慢/越远）
    [SerializeField] float tilesAngleStep = 5f; // 每个 tile 占的角度

    [Header("平铺")]
    [SerializeField] float tileWidth = 20.48f;

    float startAngle;
    float currentAngle;
    int tileCount;

    void Start()
    {
        // 记录初始角度（根据背景初始位置算）
        Vector3 dir = transform.position - planetCenter.position;
        startAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        tileCount = transform.childCount;
        ArrangeTiles(0f);
    }

    void Update()
    {
        if (groundRoot == null || tileCount == 0) return;

        // 1. 读取地面的旋转角度（eulerAngles.z）
        float groundZ = groundRoot.eulerAngles.z;

        // 2. 背景按 factor 比例跟随
        currentAngle = startAngle - groundZ * factor;

        // 3. 把整个背景父物体摆到圆上
        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = planetCenter.position
            + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * bgRadius;

        // 4. 子 tile 沿切线方向排开
        ArrangeTiles(currentAngle);
    }

    void ArrangeTiles(float baseAngle)
    {
        // 每块 tile 在圆周上的位置
        for (int i = 0; i < tileCount; i++)
        {
            float offsetAngle = (i - (tileCount - 1) / 2f) * tilesAngleStep;
            float a = baseAngle + offsetAngle;
            float r = bgRadius;

            Vector3 pos = planetCenter.position
                + new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad), 0f) * r;

            Transform t = transform.GetChild(i);
            t.position = new Vector3(pos.x, pos.y, t.position.z);

            // 让 tile 的朝向跟着弧线
            t.rotation = Quaternion.Euler(0f, 0f, a - 90f);
        }
    }
}