using DG.Tweening;
using System.Collections;
using UnityEngine;

public sealed class Stick : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Transform parent;

    [Space(20)]
    [Header("Stick properties")]
    [SerializeField] private float hitSpeed = 0.2f;
    [SerializeField] private float frequency = 0.8f;
    private float currentSide;
    private bool isReleased = true;

    private void Awake() => parent = GetComponentInChildren<Transform>();

    public void HitAndAllow(bool canAttack)
    {
        if (isReleased)
        {
            isReleased = false;
            playerController.canAttack = canAttack;
            StartCoroutine(ReleaseWithDelay());
            AnimateAttack();
        }
    }

    private IEnumerator ReleaseWithDelay()
    {
        yield return new WaitForSeconds(frequency);
        isReleased = true;
    }

    private void AnimateAttack()
    {
        float currentZRotation = transform.rotation.eulerAngles.z;
        currentSide = parent.transform.localScale.x;
        float endZ = currentSide > 0 ? currentZRotation + 90 : currentZRotation - 90;

        transform.DORotate(new Vector3(0, -10, endZ), hitSpeed).
            SetEase(Ease.Linear).
            OnComplete(() => ResetRotation());
    }

    private void ResetRotation()
    {
        float currentZRotation = transform.rotation.eulerAngles.z;
        currentSide = parent.transform.localScale.x;
        int endZ = currentZRotation < 0 ? -20 : 20;
        transform.DORotate(new Vector3(0, 180, endZ), hitSpeed);
    }
}