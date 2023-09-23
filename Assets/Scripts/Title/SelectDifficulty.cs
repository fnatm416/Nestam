using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectDifficulty : MonoBehaviour
{
    [SerializeField] GameObject FocusFrame;

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void Select(int difficulty)
    {
        GameManager.Instance.difficulty = (GameManager.Difficulty)difficulty;
        SceneManager.LoadScene("Intro");
    }
}
