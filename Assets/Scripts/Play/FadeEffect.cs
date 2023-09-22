using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [Range(0.01f, 10.0f)]
    public float fadeTime;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }


    public void FadeIn()
    {
        //���� �����
        StartCoroutine(Fade(1, 0));
    }

    public void FadeOut()
    {
        //���� ��ο���
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
