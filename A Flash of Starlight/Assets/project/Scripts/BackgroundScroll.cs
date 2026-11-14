using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Header("平移速度")]
    [SerializeField] float speed = 2f;

    [Header("循环")]
    [SerializeField] float loopWidth = 61.44f;  // 所有 Tile 的总宽度

    float offset;
    Transform[] tiles;
    Vector3[] startPositions;

    void Start()
    {
        int n = transform.childCount;
        tiles = new Transform[n];
        startPositions = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            tiles[i] = transform.GetChild(i);
            startPositions[i] = tiles[i].localPosition;
        }
    }

    void Update()
    {
        offset += speed * Time.deltaTime;

        // 超过循环宽度就重置
        if (offset > loopWidth)
            offset -= loopWidth;

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].localPosition = startPositions[i] + new Vector3(-offset, 0f, 0f);
        }
    }
}