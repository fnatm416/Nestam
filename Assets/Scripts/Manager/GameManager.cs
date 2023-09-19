using Cinemachine;
using System.Collections.Generic;
using System.Linq;
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
    public List<Character> DefeatedMonsters;

    public int MonsterCount { get; set; }

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


    public void MoveScene(int index)
    {
        SceneManager.LoadScene(Scenes[index].name);
    }

    public void AddDefeatedMonsters(StageData data)
    {
        //쓰러뜨린 몬스터들의 캐릭터를 중복없이 배열에 추가
        List<Character> list = data.monsters;

        foreach (Character character in list)
        {
            if (character != null && !list.Contains(character))
                DefeatedMonsters.Add(character);
        }
    }

    public void StageClear()
    {
        //몬스터를 모두 쓰러뜨리고 승리
        AddDefeatedMonsters(this.StageDatas[Stage]);
        Stage++;

        if (Stage > StageDatas.Length-1)
        {
            print("Clear");
            return;
        }

        MoveScene(1);
    }

    public void GameOver()
    {
        //HP가 0이되어 패배
    }
}
