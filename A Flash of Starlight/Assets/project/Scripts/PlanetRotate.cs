using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] Transform planetCenter;

    [Header("旋转")]
    [Tooltip("度/秒，正数为逆时针（地面向左流）")]
    [SerializeField] float rotateSpeed = 2f;

    void Update()
    {
        if (planetCenter == null) return;

        transform.RotateAround(
            planetCenter.position,
            Vector3.forward,
            rotateSpeed * Time.deltaTime);
    }
}