using DG.Tweening;
using System.Collections;
using UnityEngine;

public sealed class Stick : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float hitSpeed = 0.2f;
    [SerializeField] private float frequency = 0.8f;

    private Vector3 startRotation;
    private float horizontal;
    private float startZ;
    private float endZ;
    private bool isReleased = true;
    private bool isFacingRight;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isReleased)
        {
            isReleased = false;
            float currentYRotation = transform.rotation.eulerAngles.y;
            float currentZRotation = transform.rotation.eulerAngles.z;
            startRotation = new Vector3(0, currentYRotation, currentZRotation);
            if (playerController.isFacingRight)
                endZ = currentZRotation + 100;
            else
                endZ = currentZRotation - 100;

            transform.DORotate(new Vector3(0, currentYRotation, endZ), hitSpeed).
                SetEase(Ease.Linear).
                OnComplete(() => transform.DORotate(startRotation, hitSpeed));
            StartCoroutine(ReleaseWthDelay());
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D()
    {
        Debug.Log("Hitting!");
    }

    private IEnumerator ReleaseWthDelay()
    {
        yield return new WaitForSeconds(frequency);
        isReleased = true;
    }
}