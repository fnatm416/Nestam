using Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public const float GetHitDamage = 20.0f;
    public const float HitResetTime = 3.0f;
    public const float RecoveryTime = 2.0f;

    [Header("Prefab")]
    public SceneAsset[] Scenes;
    public Player PlayerPrefab;
    public Monster MonsterPrefab;

    [Header("State")]   
    public Character PlayerCharacter;
    public int Stage;
    public StageData[] StageDatas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == Scenes[2].name) 
    //    { 
    //    }
    //}
    #region PlayScene
    
    #endregion

    #region SelectScene
    public void SelectCharacter(Character character)
    {
        PlayerCharacter = character;
    }
    #endregion


    public void MoveScene(int index)
    {
        SceneManager.LoadScene(Scenes[index].name);
    }
}
