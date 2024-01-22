using UnityEngine;

public sealed class HealthPatternHandler : MonoBehaviour
{
    [Header("Healthable Entities")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Enemy enemy;
    private Health health;

    private void Awake()
    {
        health = new Health();
    }

    private void OnEnable()
    {
        health.Add(playerController);
        health.Add(enemy);
    }
}