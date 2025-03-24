using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] textBlocks; // Assign each TextMeshPro block in order
    public float fadeDuration = 1.5f;    // Duration for fading in each block
    public float delayBetweenLines = 2f; // Delay before fading the next block

    private string[] loreLines = {
        "The news mentioned something bizarre — a supposed ‘alien landing’ occured nearby",
        "The government are not willing to share any information",
        "Could this be just another Hoax.."
    };

    void Start()
    {
        StartCoroutine(ShowLoreText());
    }

    IEnumerator ShowLoreText()
    {
        // Make sure we have the same number of lines as text blocks
        if (textBlocks.Length != loreLines.Length)
        {
            Debug.LogError("Mismatch between textBlocks and loreLines. Check the Inspector.");
            yield break;
        }

        // Fade in each block one by one
        for (int i = 0; i < textBlocks.Length; i++)
        {
            yield return StartCoroutine(FadeInText(textBlocks[i], loreLines[i]));
            yield return new WaitForSeconds(delayBetweenLines);
        }

        yield return new WaitForSeconds(2f); // Small pause before transition
        SceneManager.LoadScene("Main"); // Replace with your actual game scene
    }

    IEnumerator FadeInText(TextMeshProUGUI textBlock, string line)
    {
        textBlock.text = line; // Set the text for this block
        textBlock.alpha = 0f;  // Start fully transparent

        float elapsedTime = 0f;

        // Smoothly fade in the text
        while (elapsedTime < fadeDuration)
        {
            textBlock.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textBlock.alpha = 1f; // Ensure full visibility at the end
    }
}
