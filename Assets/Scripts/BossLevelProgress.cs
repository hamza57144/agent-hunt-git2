using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLevelProgress : MonoBehaviour
{
    [SerializeField] private List<Image> bossProgressImages;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite yellowSprite;

    public float scaleAmount = 1.2f;  // Scale size for the yellow bar
    public float scaleDuration = 0.5f; // Duration of the scale cycle (increase and decrease)

    // This method updates the boss progress bar at a given index
    public void UpdateBossProgressBar(int index)
    {
        // If the index is less than 0, make the first bar yellow and animate it
        if (index < 0)
        {
            // Make the first bar yellow and animate it
            bossProgressImages[0].sprite = yellowSprite;
            StartCoroutine(AnimateYellowBar(0));  // Start animating the first bar

            // Reset the rest of the bars to default sprite
            for (int i = 1; i < bossProgressImages.Count; i++)
            {
                bossProgressImages[i].sprite = defaultSprite;
                bossProgressImages[i].transform.localScale = Vector3.one;  // Reset scale
            }

            return; // Exit to avoid further processing
        }

        // Validate the index
        if (index < 0 || index >= bossProgressImages.Count)
        {
            Debug.LogError("Index out of bounds for bossProgressImages!");
            return;
        }

        // Reset all bars to the default sprite and reset their scale (no animation)
        for (int i = 0; i < bossProgressImages.Count; i++)
        {
            if (bossProgressImages[i] != null)
            {
                bossProgressImages[i].sprite = defaultSprite;
                bossProgressImages[i].transform.localScale = Vector3.one;  // Reset scale (no animation)
            }
        }

        // Set all previous bars to green sprite
        for (int i = 0; i < index; i++)
        {
            if (bossProgressImages[i] != null)
            {
                bossProgressImages[i].sprite = greenSprite;
            }
        }

        // Set the current bar to yellow
        if (bossProgressImages[index] != null)
        {
            bossProgressImages[index].sprite = yellowSprite;
           // StartCoroutine(AnimateYellowBar(index));  // Start the scaling animation ONLY for the yellow bar
        }

        // After 0.75 seconds, set the current yellow bar to green and animate the next bar
        StartCoroutine(ProgressToNextBar(index));
    }

    // Coroutine to animate ONLY the yellow bar (increase and decrease its Y scale)
    private IEnumerator AnimateYellowBar(int index)
    {
        Image currentBar = bossProgressImages[index];
        if (currentBar != null)
        {
            Vector3 originalScale = currentBar.transform.localScale;
            Vector3 targetScale = new Vector3(originalScale.x, scaleAmount, originalScale.z); // Increased Y-scale

            while (true)
            {
                // Scale up (increase Y-scale)
                float elapsed = 0f;
                while (elapsed < scaleDuration)
                {
                    elapsed += Time.deltaTime;
                    currentBar.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / scaleDuration);
                    yield return null;
                }

                // Reset elapsed time to scale back down
                elapsed = 0f;

                // Scale down (decrease Y-scale)
                while (elapsed < scaleDuration)
                {
                    elapsed += Time.deltaTime;
                    currentBar.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / scaleDuration);
                    yield return null;
                }
            }
        }
    }

    // Coroutine to progress to the next bar after 0.75 seconds
    private IEnumerator ProgressToNextBar(int index)
    {
        // Wait for 0.75 seconds before progressing to the next bar
        yield return new WaitForSeconds(0.75f);

        // Check if the next bar exists
        int nextIndex = index + 1;
        if (nextIndex < bossProgressImages.Count)
        {
            Image nextBar = bossProgressImages[nextIndex];

            if (nextBar != null)
            {
                // Set the current yellow bar to green (no animation)
                bossProgressImages[index].sprite = greenSprite;

                // Set the next bar to yellow and start animating
                nextBar.sprite = yellowSprite;
                StartCoroutine(AnimateYellowBar(nextIndex));  // Start animation for the next yellow bar
            }
        }
    }
}
