using System.Collections;
using UnityEngine;

public sealed class Enemy : MonoBehaviour
{
    public EntityHealth health = new EntityHealth();
    [SerializeField] private Player playerController;
    [SerializeField] private EnemyHealthDisplay healthDisplay;
    private Stick stick;
    private ObjectFlip flip;
    private Transform target;

    [Space(20)]
    [Header("Enemy's property")]
    [SerializeField] private int chaseSpeed = 10;
    private const float hitDistance = 1.5f;

    private void Awake()
    {
        stick = GetComponentInChildren<Stick>();
        target = FindObjectOfType<Player>().transform;

        flip = new ObjectFlip(transform);
    }

    private void OnEnable()
    {
        playerController.OnMove += () => StartCoroutine(UpdateCoroutine());
        healthDisplay.OnDie += () => Destroy(gameObject);
    }

    private void OnDestroy()
    {
        playerController.OnMove -= () => StartCoroutine(UpdateCoroutine());
        healthDisplay.OnDie -= () => Destroy(gameObject);
    }

    private IEnumerator UpdateCoroutine()
    {
        while (target != null)
        {
            AttackOrChase();
            CheckOrFlip();
            yield return new WaitForFixedUpdate();
        }
    }

    private void AttackOrChase()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > hitDistance)
            transform.Translate(CalculateDirection() * Time.fixedDeltaTime);
        if (distance < hitDistance + 1)
            stick.HitAndAllow(true);
    }

    private Vector2 CalculateDirection() => target.position - transform.position;

    private void CheckOrFlip()
    {
        float horizontalInput = Mathf.Sign(target.position.x - transform.position.x);
        flip.CheckOrFlip(horizontalInput);
    }
}