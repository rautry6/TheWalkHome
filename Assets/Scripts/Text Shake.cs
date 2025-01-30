using UnityEngine;
using TMPro;

public class TextShake : MonoBehaviour
{
    public float shakeIntensity = 2f; // How much the text moves
    public float shakeSpeed = 50f;    // How fast it shakes

    private Vector3 originalPosition;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2 - 1;

        rectTransform.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0) * shakeIntensity;
    }

    public void StartShake(float duration)
    {
        StartCoroutine(ShakeForDuration(duration));
    }

    private System.Collections.IEnumerator ShakeForDuration(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }
}
