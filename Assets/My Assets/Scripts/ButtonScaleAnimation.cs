using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ButtonScaleAnimation : MonoBehaviour
{
    private Button button;
    private Vector3 originalScale;
    public float scaleMultiplier = 1.2f;
    public float animationDuration = 0.2f;

    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        button.onClick.AddListener(OnButtonClick);
        StartCoroutine(AnimateButton());
    }

    private void OnButtonClick()
    {
        StopAllCoroutines();
        
    }

    private IEnumerator AnimateButton()
    {
        // Scale up
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleMultiplier, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale * scaleMultiplier;

        // Scale down
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale * scaleMultiplier, originalScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}


