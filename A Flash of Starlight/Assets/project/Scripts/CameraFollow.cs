using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0f, 2f, -10f);
    [SerializeField] float smoothTime = 0.15f;

    Vector3 velocity;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 wanted = target.position + offset;
        wanted.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(
            transform.position, wanted, ref velocity, smoothTime);
    }
}