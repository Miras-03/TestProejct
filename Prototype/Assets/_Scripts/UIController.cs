using UnityEngine;

public sealed class UIController : MonoBehaviour
{
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private GameObject menuContainer;

    private void Start() => menuContainer.SetActive(false);

    private void OnEnable() => playerHealthHandler.OnDie += () => menuContainer.SetActive(true);

    private void OnDestroy() => playerHealthHandler.OnDie -= () => menuContainer.SetActive(false);
}