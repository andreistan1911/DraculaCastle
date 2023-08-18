using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    public float duration = 5f;
    public RawImage image;
    public TextMeshProUGUI text;

    private float screenTime = 3.0f;

    public float ScreenTime => screenTime;
    public RawImage Image => image;
    public TextMeshProUGUI Text => text;

    private void Start()
    {
        text.color = new(255, 255, 255, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger the fade-in when a game object with the tag 'Player' enters the trigger
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(FadeInGameComplete());
        }
    }

    public IEnumerator FadeInGameComplete()
    {
        float elapsedTime = 0f;

        Color imageColor = image.color;
        Color textColor = text.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, elapsedTime / duration);

            // Fade in RawImage
            imageColor.a = newAlpha;
            image.color = imageColor;

            // Fade in TMP_Text
            textColor.a = newAlpha;
            text.color = textColor;

            yield return null;
        }

        // Ensure they are fully visible
        imageColor.a = 1;
        image.color = imageColor;
        textColor.a = 1;
        text.color = textColor;

        yield return new WaitForSeconds(5f);

        // Return to main menu
        SceneManager.LoadScene("Scenes/Menu");
    }
}
