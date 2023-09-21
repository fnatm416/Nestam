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

    public void Init()
    {
        startButton.SetActive(true);
        difficulty.SetActive(false);
    }

    public void ClickStartButton()
    {
        startButton.SetActive(false);
        difficulty.SetActive(true);
    }
}