using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    public const float Duration = 1.5f;

    public void FadeIn()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, 0.7f));
    }

    private static IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        var counter = 0.0f;

        while (counter < Duration)
        {
            counter += Time.fixedDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / Duration);

            yield return null;
        }
    }
}
