using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    public Difficulty difficulty;
    public float MonsterPower
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return 0.1f;
                case Difficulty.Normal:
                    return 0.75f;
                case Difficulty.Hard:
                    return 1.0f;
            }
            return 1.0f;
        }
    }

    public const float GetHitDamage = 20.0f;
    public const float HitResetTime = 3.0f;
    public const float RecoveryTime = 2.0f;

    [Header("Prefab")]
    public SceneAsset[] Scenes;
    public Player PlayerPrefab;
    public Monster MonsterPrefab;
    public StageData[] StageDatas;

    [Header("State")]
    public Character[] DefaultCharacters;
    public Character PlayerCharacter;
    public int Stage;
    public List<Character> DefeatedMonsters;

    public int MonsterCount { get; set; }
    bool isPlay;
    public bool IsPlay
    {
        get
        {
            return isPlay;
        }
        set
        {
            isPlay = value;
            Cursor.lockState = isPlay ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

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
        Init();
    }

    public void Init()
    {
        IsPlay = false;
        Stage = 1;
        DefeatedMonsters.Clear();
        for (int i = 0; i < DefaultCharacters.Length; i++)
        {
            DefeatedMonsters.Add(DefaultCharacters[i]);
        }
    }

    public void MoveScene(int index)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(Scenes[index].name);
    }

    public void AddDefeatedMonsters(StageData data)
    {
        //쓰러뜨린 몬스터들의 캐릭터를 중복없이 배열에 추가
        List<Character> list = data.monsters;

        foreach (Character character in list)
        {
            if (character == null)
                continue;

            if (!DefeatedMonsters.Contains(character))
                DefeatedMonsters.Add(character);
        }
    }

    public void StageClear()
    {
        //몬스터를 모두 쓰러뜨리고 승리
        AddDefeatedMonsters(this.StageDatas[Stage - 1]);
        Stage++;

        if (Stage > StageDatas.Length)
            MoveScene(3);
        else
            MoveScene(1);
    }

    public void GamePause(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = value ? 0 : 1.0f;
    }
}
