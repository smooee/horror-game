using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScript : MonoBehaviour
{
    public TMP_Text endText1;
    public TMP_Text endText2;
    public float fadeDuration = 3f;
    public float delayBetweenTexts = 5f;

    void Start()
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        // Write and fade in the first text
        endText1.text = "The End";
        yield return StartCoroutine(FadeInText(endText1));

        // Wait before showing the second text
        yield return new WaitForSeconds(delayBetweenTexts);

        // Write and fade in the second text
        endText2.text = "By Sam Moore";
        yield return StartCoroutine(FadeInText(endText2));

        // Wait for 10 seconds before switching scenes
        yield return new WaitForSeconds(10f);

        // Switch to the StartMenu scene
        SceneManager.LoadScene("StartMenu");
    }

    IEnumerator FadeInText(TMP_Text text)
    {
        float elapsed = 0f;
        Color textColor = text.color;
        textColor.a = 0f;
        text.color = textColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            textColor.a = Mathf.Clamp01(elapsed / fadeDuration);
            text.color = textColor;
            yield return null;
        }
    }
}
