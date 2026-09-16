// FeedbackManager.cs
// Shows visual feedback when player sorts correctly or wrongly

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeedbackManager : MonoBehaviour
{
    [Header("Feedback Images")]
    public Image feedbackImage;

    [Header("Colors")]
    public Color correctColor = new Color(0f, 1f, 0f, 0.4f);
    public Color wrongColor = new Color(1f, 0f, 0f, 0.4f);

    [Header("Settings")]
    public float flashDuration = 0.3f;

    public void ShowCorrect()
    {
        StopAllCoroutines();
        StartCoroutine(Flash(correctColor));
    }

    public void ShowWrong()
    {
        StopAllCoroutines();
        StartCoroutine(Flash(wrongColor));
    }

    IEnumerator Flash(Color color)
    {
        feedbackImage.color = color;
        feedbackImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        feedbackImage.gameObject.SetActive(false);
    }
}