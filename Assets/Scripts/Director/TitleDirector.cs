using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject difficulty;

    void Start()
    {
        Init();
    }

    void Init()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Bgm.Title);

        startButton.SetActive(true);
        difficulty.SetActive(false);
        GameManager.Instance.Init();   
    }

    public void ClickStartButton()
    {
        startButton.SetActive(false);
        difficulty.SetActive(true);
    }
}