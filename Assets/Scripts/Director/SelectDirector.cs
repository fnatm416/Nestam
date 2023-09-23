using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectDirector : MonoBehaviour
{
    public Image[] NextMonsters;
    public TextMeshProUGUI StageText;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        GameManager.Instance.IsPlay = false;
        //몬스터 썸네일 적용
        ShowMonsters();
        //스테이지 수 표시
        StageText.text = string.Format("Stage {0}", GameManager.Instance.Stage);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    void ShowMonsters()
    {
        StageData data = GameManager.Instance.StageDatas[GameManager.Instance.Stage-1];
        for (int i = 0; i < NextMonsters.Length; i++)
        {
            if (data.monsters[i] != null)
            {
                NextMonsters[i].sprite = data.monsters[i].Thumbnail;
                NextMonsters[i].color = Color.white;
            }
        }
    }
}