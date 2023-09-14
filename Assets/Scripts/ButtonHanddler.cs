using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHanddler : MonoBehaviour
{
   public void SelectCharacter(Character character)
    {
        GameManager.instance.SelectCharacter(character);
        SceneManager.LoadScene(GameManager.instance.scenes[2].name);
    }
}
