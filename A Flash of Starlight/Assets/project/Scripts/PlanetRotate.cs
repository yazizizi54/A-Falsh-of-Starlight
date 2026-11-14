using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    [SerializeField] Transform planetCenter;
    [SerializeField] float rotateSpeed = 10f;   // 度/秒

    void Update()
    {
        // 绕圆心旋转，会让地面往左流
        transform.RotateAround(
            planetCenter.position,
            Vector3.forward,
            rotateSpeed * Time.deltaTime);
    }
}