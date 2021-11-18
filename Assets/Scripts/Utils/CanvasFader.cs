using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    public const float Duration = 1.5f;

    public void Fade()
    {
        
        var canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, .7f));
    }

    private IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0.0f;

        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / Duration);

            yield return null;
        }
    }
}
