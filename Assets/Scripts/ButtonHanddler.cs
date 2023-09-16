using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHanddler : MonoBehaviour
{
   public void SelectCharacter(Character character)
    {
        GameManager.Instance.SelectCharacter(character);
        SceneManager.LoadScene(GameManager.Instance.Scenes[2].name);
    }
}
