using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonNav : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Main"); 
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting..."); 
    }
}
