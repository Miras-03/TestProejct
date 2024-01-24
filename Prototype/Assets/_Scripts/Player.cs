using System;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    public Action OnMove;
    public EntityHealth health = new EntityHealth();

    [SerializeField] private PlayerHealthHandler healthDisplay;
    [SerializeField] private Transform footPoint;
    private Stick stick;
    private ObjectFlip flip;
    private Rigidbody2D rb;

    [Space(10)]
    [SerializeField] private LayerMask groundLayer;

    [Space(20)]
    [Header("Player properties")]
    [SerializeField] private int speed = 10;
    [SerializeField] private int jumpForce = 7;
    [SerializeField] private int hitAttempts = 3;
    private int attemptCount = 0;
    private float horizontal;
    private bool canAttack = false;
    private bool hasMoved = false;
    private bool hasAttacked = false;

    private void Awake()
    {
        stick = GetComponentInChildren<Stick>();
        rb = GetComponent<Rigidbody2D>();

        flip = new ObjectFlip(transform);
    }

    private void OnEnable() => healthDisplay.OnDie += () => Destroy(gameObject);

    private void OnDestroy() => healthDisplay.OnDie -= () => Destroy(gameObject);

    private void Update()
    {
        CheckOrJump();
        CheckOrAttack();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (IsMoving())
        {
            CheckOrWakeEnemy();
            transform.Translate(Vector2.right * horizontal * speed * Time.fixedDeltaTime);
            flip.CheckOrFlip(horizontal);
        }
    }

    private void CheckOrJump()
    {
        bool canJump = IsPressingSpace() && IsGrounded();
        if (canJump)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsMoving() => horizontal != 0;

    private void CheckOrWakeEnemy()
    {
        if (!hasMoved)
        {
            OnMove();
            hasMoved = !hasMoved;
        }
    }

    private bool IsPressingSpace() => Input.GetKeyDown(KeyCode.Space);

    private bool IsGrounded() => Physics2D.OverlapCircle(footPoint.position, 0.2f, groundLayer);

    private void CheckOrAttack()
    {
        if (IsPressingLMouse() && canAttack)
        {
            attemptCount++;
            Attack();
        }
    }

    private bool IsPressingLMouse() => Input.GetMouseButtonDown(0);

    private void Attack()
    {
        if (attemptCount < hitAttempts)
        {
            HitAndAllow(true);
            hasAttacked = true;
        }
        else
        {
            attemptCount = 0;
            HitAndAllow(false);
        }
    }

    private void HitAndAllow(bool flag) => stick.HitAndAllow(flag);

    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public bool HasAttacked { get => hasAttacked; set => hasAttacked = value; }
}