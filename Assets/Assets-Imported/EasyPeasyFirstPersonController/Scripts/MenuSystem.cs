
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // Resume gameplay and remove the menu.
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    // Close the menu without changing scenes.
    public void CloseMenu()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    // Load a scene by its name.
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    // Quit the game.
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

        Debug.Log("Quit Game button pressed.");
    }
}
