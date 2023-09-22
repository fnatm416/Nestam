using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDirector : MonoBehaviour
{
    [SerializeField] Transform CharacterPosition;

    void Start()
    {
        Character character = Instantiate(GameManager.Instance.PlayerCharacter, CharacterPosition, false);
        character.OnInterrupt();

        if (character.PhotoSize != null)
        {
            character.transform.localPosition += character.PhotoSize.Position;
            character.transform.localScale = (character.PhotoSize.Scale == 0) ?
                Vector3.one : Vector3.one * character.PhotoSize.Scale;
        }
    }

    public void MoveTitle()
    {
        GameManager.Instance.MoveScene(0);
    }
}
