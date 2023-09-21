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
        startButton.SetActive(true);
        difficulty.SetActive(false);

        GameManager.Instance.DefeatedMonsters.Clear();
        for(int i=0; i<GameManager.Instance.DefaultCharacters.Length; i++)
        {
            GameManager.Instance.DefeatedMonsters.Add(GameManager.Instance.DefaultCharacters[i]);
        }
    }

    public void ClickStartButton()
    {
        startButton.SetActive(false);
        difficulty.SetActive(true);
    }
}