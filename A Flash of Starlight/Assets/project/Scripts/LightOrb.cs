using System.Collections;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    [Header("浮动动画")]
    [SerializeField] float bobAmplitude = 0.15f;
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] float rotateSpeed = 90f;

    [Header("拾取")]
    [SerializeField] float pickupDuration = 0.25f;

    Vector3 basePos;
    bool picked;

    void Start()
    {
        basePos = transform.position;
    }

    void Update()
    {
        if (picked) return;

        // 上下浮动（用 position 做随机相位，避免所有球同步）
        float phase = transform.position.x * 0.7f;
        float y = Mathf.Sin(Time.time * bobSpeed + phase) * bobAmplitude;

        transform.position = basePos + new Vector3(0f, y, 0f);
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;
        if (!other.CompareTag("Player")) return;

        picked = true;
        StartCoroutine(PickupRoutine());
    }

    IEnumerator PickupRoutine()
    {
        float t = 0f;
        Vector3 startScale = transform.localScale;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        while (t < pickupDuration)
        {
            t += Time.deltaTime;
            float k = t / pickupDuration;

            transform.localScale = startScale * (1f + k * 0.8f);

            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - k;
                sr.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}