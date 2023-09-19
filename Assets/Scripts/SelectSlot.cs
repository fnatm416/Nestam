using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSlot : MonoBehaviour
{
    [SerializeField] Character DefaultCharacter;
    [SerializeField] GameObject Thumbnail;
    [SerializeField] GameObject FocusFrame;

    void Start()
    {
        ShowSlot();
    }

    void ShowSlot()
    {
        if (GameManager.Instance.PlayerCharacter != null)
        {
            Thumbnail.GetComponent<Image>().sprite = GameManager.Instance.PlayerCharacter.Thumbnail;
        }
        else
        {
            Thumbnail.GetComponent<Image>().sprite = this.DefaultCharacter.Thumbnail;
        }
    }

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void OnSelect()
    {
        GameManager gm = GameManager.Instance;

        if (gm.Stage <= 0)
        {
            GameManager.Instance.PlayerCharacter = this.DefaultCharacter;
        }
        else
        {
            GameManager.Instance.PlayerCharacter = gm.PlayerCharacter;
        }

        SceneManager.LoadScene(GameManager.Instance.Scenes[2].name);
    }
}
