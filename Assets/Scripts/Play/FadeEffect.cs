using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public float fadeTime { get; set; }
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }


    public void FadeIn(float time)
    {
        //Á¡Á¡ ¹à¾ÆÁü
        this.fadeTime = time;
        StartCoroutine(Fade(1, 0));
    }

    public void FadeOut(float time)
    {
        //Á¡Á¡ ¾îµÎ¿öÁü
        this.fadeTime = time;
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

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }
}
