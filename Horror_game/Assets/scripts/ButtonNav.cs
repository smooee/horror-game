using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonNav : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("LoreScene"); 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Options"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); 
    }
}
