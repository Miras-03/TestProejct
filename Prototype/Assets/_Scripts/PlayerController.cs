using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform footPoint;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int speed = 10;
    [SerializeField] private int jumpForce = 8;

    private float horizontal;
    public bool isFacingRight;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            transform.Translate(Vector2.right * horizontal * speed * Time.fixedDeltaTime);
            CheckOrFlip();
        }
    }

    private void CheckOrFlip()
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(footPoint.position, 0.2f, groundLayer);
}