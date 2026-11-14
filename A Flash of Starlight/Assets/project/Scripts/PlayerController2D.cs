using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("自动奔跑")]
    [SerializeField] float runSpeed = 6f;

    [Header("跳跃")]
    [SerializeField] float jumpForce = 14f;

    [Header("滑行")]
    [SerializeField] bool enableSlide = true;
    [SerializeField] float slideDuration = 0.6f;
    [SerializeField] float slideCooldown = 0.3f;
    [SerializeField] float slideColliderHeight = 1.4f;

    [Header("地面检测")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    [Header("手感")]
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;

    Rigidbody2D rb;
    BoxCollider2D boxCol;

    bool isGrounded;
    bool isSliding;
    float slideTimer;
    float slideCooldownTimer;
    float coyoteTimer;
    float jumpBufferTimer;

    Vector2 normalSize;
    Vector2 normalOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        normalSize = boxCol.size;
        normalOffset = boxCol.offset;
    }

    void Update()
    {
        // 计时器
        if (slideCooldownTimer > 0f) slideCooldownTimer -= Time.deltaTime;
        if (jumpBufferTimer > 0f) jumpBufferTimer -= Time.deltaTime;

        // 地面检测
        bool groundedNow = groundCheck != null &&
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (groundedNow) coyoteTimer = coyoteTime;
        else if (coyoteTimer > 0f) coyoteTimer -= Time.deltaTime;

        isGrounded = groundedNow;

        // 滑行计时
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f) EndSlide();
        }

        // 跳跃输入
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;

        // 跳跃执行
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            if (isSliding) EndSlide();

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // 滑行输入
        if (enableSlide)
        {
            bool slidePressed =
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftControl);

            if (slidePressed) TrySlide();

            // 按住时保持滑行（可选）
            bool slideHeld =
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.LeftControl);

            if (isSliding && slideHeld) slideTimer = Mathf.Max(slideTimer, 0.1f);
        }
    }

    void FixedUpdate()
    {
        // 水平方向：恒定速度自动前进
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    // ---------- 滑行 ----------
    void TrySlide()
    {
        if (isSliding) return;
        if (slideCooldownTimer > 0f) return;
        if (!isGrounded) return;

        isSliding = true;
        slideTimer = slideDuration;

        // 碰撞体变矮，底部保持贴地
        float shrink = normalSize.y - slideColliderHeight;

        boxCol.size = new Vector2(normalSize.x, slideColliderHeight);
        boxCol.offset = new Vector2(normalOffset.x, normalOffset.y - shrink * 0.5f);

        // 视觉压缩
        transform.localScale = new Vector3(1f, 0.7f, 1f);
    }

    void EndSlide()
    {
        isSliding = false;
        slideCooldownTimer = slideCooldown;

        boxCol.size = normalSize;
        boxCol.offset = normalOffset;

        transform.localScale = Vector3.one;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}