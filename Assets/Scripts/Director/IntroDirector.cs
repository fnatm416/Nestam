using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroDirector : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] FadeEffect panel;
    bool isSkip = false;

    void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.Intro);

        panel.FadeIn(2.0f);
        playableDirector.stopped += PlayableDirector_Stopped;
        isSkip = false;
    }

    public void PlayableDirector_Stopped(PlayableDirector obj)
    {
        if (isSkip)
            return;

        isSkip = true;
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        panel.FadeOut(1.5f);
        yield return new WaitForSeconds(panel.fadeTime);
        playableDirector.stopped -= PlayableDirector_Stopped;
        SceneManager.LoadScene("Select");
    }
}