using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;

    void Start()
    {
        // Load saved sensitivity or default to 2
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
        sensitivitySlider.value = savedSensitivity;

        // Listen to slider changes
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    public void UpdateSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
        Debug.Log("Sensitivity saved: " + value);
    }
}
