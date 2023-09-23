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

    void Start()
    {
        panel.FadeIn(2.0f);
        playableDirector.stopped += PlayableDirector_Stopped;
    }

    private void PlayableDirector_Stopped(PlayableDirector obj)
    {
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        panel.FadeOut(1.5f);
        yield return new WaitForSeconds(panel.fadeTime);
        playableDirector.stopped -= PlayableDirector_Stopped;
        SceneManager.LoadScene("Select");
    }
}