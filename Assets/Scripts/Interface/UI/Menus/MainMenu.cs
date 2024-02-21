using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home", LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Persistents", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        Destroy(GameManager.Instance);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
