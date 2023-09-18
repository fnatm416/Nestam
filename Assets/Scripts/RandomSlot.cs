using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSlot : MonoBehaviour
{
    [SerializeField] Character DefaultCharacter;
    [SerializeField] GameObject DefaultThumbnail;
    [SerializeField] GameObject RandomThumbnail;
    [SerializeField] GameObject FocusFrame;

    void Start()
    {
        ShowSlot();
    }

    void ShowSlot()
    {
        if (GameManager.Instance.Stage <= 0)
        {
            DefaultThumbnail.SetActive(true);
            RandomThumbnail.SetActive(false);
        }
        else
        {
            DefaultThumbnail.SetActive(false);
            RandomThumbnail.SetActive(true);
        }
    }

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void OnSelect()
    {
        GameManager gm = GameManager.Instance;

        if ( gm.Stage <= 0)
        {
            GameManager.Instance.PlayerCharacter = this.DefaultCharacter;
        }    
        else
        {
            int random = Random.Range(0, gm.DefeatedMonsters.Count);
            GameManager.Instance.PlayerCharacter = gm.DefeatedMonsters[random];
        }
        
        SceneManager.LoadScene(GameManager.Instance.Scenes[2].name);
    }
}
