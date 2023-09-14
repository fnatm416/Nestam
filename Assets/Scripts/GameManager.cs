using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public const float GetHitDamage = 20.0f;
    public const float HitResetTime = 3.0f;
    public const float RecoveryTime = 2.0f;

    public SceneAsset[] scenes;

    public Player player;
    public Character playerCharacter;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == scenes[2].name)
        {
            player = Instantiate(player);
            playerCharacter = Instantiate(playerCharacter, player.transform, false);
            player.transform.position = GameObject.Find("PlayerPosition").transform.position;
            GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = player.transform.Find("CameraRoot");
        }
    }
    #region PlayScene

    #endregion

    #region SelectScene
    public void SelectCharacter(Character character)
    {
        playerCharacter = character;
    }
    #endregion


    public void MoveScene(int index)
    {
        SceneManager.LoadScene(scenes[index].name);
    }
}
