using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public void LoadScene(int index) => SceneManager.LoadScene(index);
}