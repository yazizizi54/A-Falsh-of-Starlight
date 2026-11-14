using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] bool isDeadly = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDeadly) return;

        // 检查是不是玩家
        if (other.CompareTag("Player"))
        {
            Debug.Log("撞到障碍物了！");
            // 后面接死亡逻辑
        }
    }
}