using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    void Start()
    {
        // Load saved settings
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        
        // Apply initial values
        UpdateSensitivity();
        UpdateVolume();
    }

    public void UpdateSensitivity()
    {
        float sensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
        Debug.Log("Sensitivity set to: " + sensitivity);
    }

    public void UpdateVolume()
    {
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
        Debug.Log("Volume set to: " + volume);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); // Replace with your actual main menu scene name
    }
}
