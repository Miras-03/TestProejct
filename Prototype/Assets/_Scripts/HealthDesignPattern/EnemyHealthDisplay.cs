using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class EnemyHealthDisplay : MonoBehaviour, IHealthObserver
{
    public Action OnDie;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Image circle;
    private Stack<Image> circles = new Stack<Image>();

    [SerializeField] private int circleCount = 3;

    private void Start() => FillCircles();

    private void FillCircles()
    {
        for (int i = 0; i < circleCount; i++)
            circles.Push(Instantiate(circle, transform));
    }

    private void OnEnable() => enemy.health.CheckOrAdd(this);

    private void OnDestroy() => enemy.health.CheckOrRemove(this);

    public void ExecuteDamageOrDie()
    {
            Destroy(circles.Pop());
        if (circles.Count < 1)
        {
            OnDie();
        }
    }
}