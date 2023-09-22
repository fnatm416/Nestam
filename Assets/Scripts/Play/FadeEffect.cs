using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [Range(0.01f, 10.0f)]
    public float fadeTime;
    Image panel;

    void Awake()
    {
        panel = GetComponent<Image>();
    }


    public void FadeIn()
    {
        //Á¡Á¡ ¹à¾ÆÁü
        StartCoroutine(Fade(1, 0));
    }

    public void FadeOut()
    {
        //Á¡Á¡ ¾îµÎ¿öÁü
        StartCoroutine(Fade(0, 1));
    }

    IEnumerator Fade(float start, float end)
    {
        float currentTime = 0;
        float percent = 0;

        while( percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = panel.color;
            color.a = Mathf.Lerp(start, end, percent);
            panel.color = color;

            yield return null;
        }
    }
}
