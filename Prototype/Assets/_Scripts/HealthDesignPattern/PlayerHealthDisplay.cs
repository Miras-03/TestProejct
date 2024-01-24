using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerHealthDisplay : MonoBehaviour, IHealthObserver
{
    public Action OnDie;
    [SerializeField] private Player player;
    [SerializeField] private Image circleImage;
    private Coroutine blinkCoroutine;
    private Stack<Image> circles = new Stack<Image>();

    [Range(1f, 10f)]
    [SerializeField] private int circleCount = 3;

    [Space(20)]
    [Header("Blink properties")]
    [Range(1f, 10f)]
    [SerializeField] private int blinkCountPerFrame = 6;
    [Range(0f, 1f)]
    [SerializeField] private float blinkTimePerFrame = 0.5f;

    private int attackedCount = 0;

    private void Start() => FillCircles();

    private void FillCircles()
    {
        for (int i = 0; i < circleCount; i++)
            circles.Push(Instantiate(circleImage, transform));
    }

    private void OnEnable() => player.health.CheckOrAdd(this);

    private void OnDestroy() => player.health.CheckOrRemove(this);

    public void ExecuteDamageOrDie()
    {
        attackedCount++;
        if (circles.Count > 0)
        {
            if (attackedCount < 2)
                blinkCoroutine = StartCoroutine(BlinkAndDamage());
            else
            {
                if (blinkCoroutine != null)
                    StopCoroutine(blinkCoroutine);
                CheckOrTakeDamage();
            }
        }
        else
            OnDie();
    }

    private IEnumerator BlinkAndDamage()
    {
        int i = 0;
        while (i < blinkCountPerFrame)
        {
            SetLastCircle(false);
            yield return new WaitForSeconds(blinkTimePerFrame);
            SetLastCircle(true);
            yield return new WaitForSeconds(blinkTimePerFrame);
            SetLastCircle(false);
            i++;
        }

        CheckOrTakeDamage();
    }

    private void CheckOrTakeDamage()
    {
        SetLastCircle(true);
        if (!player.HasAttacked && attackedCount >= 2)
        {
            Destroy(circles.Pop());
            if (circles.Count > 0)
                Destroy(circles.Pop());
        }
        else if (!player.HasAttacked)
            Destroy(circles.Pop());
        attackedCount = 0;
    }

    private void SetLastCircle(bool flag) => circles.Peek()?.gameObject.SetActive(flag);
}