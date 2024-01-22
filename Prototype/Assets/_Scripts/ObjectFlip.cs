using UnityEngine;

public sealed class ObjectFlip
{
    private Transform transform;

    private bool isFacingRight;

    public ObjectFlip(Transform transform) => this.transform = transform;

    public void CheckOrFlip(float movementDirection)
    {
        bool shouldFlip = isFacingRight && movementDirection < 0 || !isFacingRight && movementDirection > 0;
        if (shouldFlip)
        {
            isFacingRight = !isFacingRight;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
