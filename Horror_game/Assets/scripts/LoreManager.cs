using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoreManager : MonoBehaviour
{
    public TextMeshProUGUI loreText; // Drag your TextMeshPro text here
    public float fadeDuration = 1.5f; // How long each text fades in
    public float delayBetweenLines = 2f; // Delay before next line starts

    private string[] loreLines = {
        "The night is cold... the wind whispers secrets through the cracks in the walls.",
        "This house... something is off. It doesn’t feel empty, even when you're alone.",
        "Every creak in the floorboards feels... deliberate. Like something is watching.",
        "Maybe it's just your mind playing tricks on you... but you swear, you heard breathing last night.",
        "Tonight... you need answers."
    };

    private string currentText = ""; // Stores the full lore text

    void Start()
    {
        StartCoroutine(ShowLoreText());
    }

    IEnumerator ShowLoreText()
    {
        foreach (string line in loreLines)
        {
            currentText += line + "\n"; // Append new line below previous ones
            yield return StartCoroutine(FadeInText(currentText));
            yield return new WaitForSeconds(delayBetweenLines);
        }

        yield return new WaitForSeconds(2f); // Short pause before scene transition
        SceneManager.LoadScene("Main"); // Replace with your actual game scene name
    }

    IEnumerator FadeInText(string text)
    {
        loreText.text = text;
        loreText.alpha = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            loreText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        loreText.alpha = 1f;
    }
}
