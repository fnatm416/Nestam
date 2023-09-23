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
    [Header("Difficulty")]
    [SerializeField] float Easy_Value;
    [SerializeField] float Normal_Value;
    [SerializeField] float Hard_Value;
    public float MonsterPower
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return Easy_Value;
                case Difficulty.Normal:
                    return Normal_Value;
                case Difficulty.Hard:
                    return Hard_Value;
            }
            return 1.0f;
        }
    }

    public const float GetHitDamage = 20.0f;
    public const float HitResetTime = 3.0f;
    public const float RecoveryTime = 2.0f;

    [Header("Prefab")]
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

        SceneManager.sceneLoaded += (scene, mode) => { Time.timeScale = 1.0f; };

        Init();
    }

    void Update()
    {
        MonsterCheck();
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

    void MonsterCheck()
    {
        Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
        int sum = 0;
        foreach(Monster monster in monsters)
        {
            if (monster.state == Monster.State.Die)
                continue;

            sum++;
        }

        MonsterCount = sum;
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
            SceneManager.LoadScene("Ending");
        else
            SceneManager.LoadScene("Select");
    }

    public void GamePause(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = value ? 0 : 1.0f;
    }
}
