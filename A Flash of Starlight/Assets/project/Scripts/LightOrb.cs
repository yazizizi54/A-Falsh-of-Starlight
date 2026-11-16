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

    Vector3 baseLocalPos;   // 改成记录 localPosition
    bool picked;

    void Start()
    {
        baseLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (picked) return;

        // 用 localPosition 浮动，不干扰父物体的移动
        float phase = baseLocalPos.x * 0.7f;
        float y = Mathf.Sin(Time.time * bobSpeed + phase) * bobAmplitude;

        transform.localPosition = baseLocalPos + new Vector3(0f, y, 0f);
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;
        if (!other.CompareTag("Player")) return;

        picked = true;

        if (OrbCounter.Instance != null)
            OrbCounter.Instance.Add(1);

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