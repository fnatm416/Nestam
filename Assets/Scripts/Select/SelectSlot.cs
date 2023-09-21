using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSlot : MonoBehaviour
{
    [SerializeField] Character DefaultCharacter;
    [SerializeField] GameObject Thumbnail;
    [SerializeField] GameObject FocusFrame;

    public Slider health;
    public Slider power;
    public Slider speed;

    void Start()
    {
        ShowSlot();
    }

    void ShowSlot()
    {
        Character character = null;

        if (GameManager.Instance.PlayerCharacter != null && GameManager.Instance.Stage > 1) 
            character = GameManager.Instance.PlayerCharacter;
        else { character = this.DefaultCharacter; }

        Thumbnail.GetComponent<Image>().sprite = character.Thumbnail;
        health.value = character.Health / 100.0f;
        power.value = character.Power / 10.0f;
        speed.value = character.Speed / 10.0f;
    }

    public void OnFocus(bool focus)
    {
        FocusFrame.SetActive(focus);
    }

    public void OnSelect()
    {
        GameManager gm = GameManager.Instance;

        if (gm.Stage <= 1)
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
