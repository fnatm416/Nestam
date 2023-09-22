using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDifficulty : MonoBehaviour
{
    [SerializeField] GameObject FocusFrame;

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void Select(int difficulty)
    {
        if(GameManager.Instance.Stage == 1)
        {
            //��Ʈ�ξ����� �̵�
        }
        else
            //����Ʈ������ �̵�

        GameManager.Instance.difficulty = (GameManager.Difficulty)difficulty;
        GameManager.Instance.MoveScene(1);
    }
}
