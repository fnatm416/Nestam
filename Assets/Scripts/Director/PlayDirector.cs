using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayDirector : MonoBehaviour
{
    public static PlayDirector Instance;

    [SerializeField] Transform playerPosition;
    [SerializeField] Transform[] monsterPositions;

    [SerializeField] Slider playerHealth;
    [SerializeField] Slider monsterHealth;

    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] FadeEffect fadeEffect;

    Player player;
    List<Monster> monsters = new List<Monster>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void Init()
    {
        winUI.SetActive(false);
        loseUI.SetActive(false);
        pauseUI.SetActive(false);
        CreatePlayer();
        CreateMonsters();
    }

    void CreatePlayer()
    {
        GameManager gm = GameManager.Instance;

        player = Instantiate(gm.PlayerPrefab);
        Instantiate(gm.PlayerCharacter, player.transform, false);
        player.transform.position = playerPosition.position;

        CinemachineVirtualCamera camera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        Transform cameraRoot = player.transform.Find("CameraRoot");
        camera.Follow = cameraRoot;
        camera.LookAt = cameraRoot;
    }

    void CreateMonsters()
    {
        GameManager gm = GameManager.Instance;
        int index = gm.Stage - 1;

        for (int i = 0; i < gm.StageDatas[index].monsters.Count; i++)
        {
            if (gm.StageDatas[index].monsters[i] == null)
                continue;

            Monster monster = Instantiate(gm.MonsterPrefab);    
            Instantiate(gm.StageDatas[index].monsters[i], monster.transform, false);
            monster.transform.position = monsterPositions[i].transform.position;
            monsters.Add(monster);
 
            gm.MonsterCount++;
        }
    }

    void UpdateHealthBar()
    {
        if (!player.Character)
            return;

        playerHealth.value = player.Health / player.Character.Health;

        float sumHealth = 0;
        float sumMaxHealth = 0;
        foreach (Monster monster in monsters)
        {
            sumHealth += monster.Health;
            sumMaxHealth += monster.Character.Health * GameManager.Instance.MonsterPower;
        }
        monsterHealth.value = sumHealth / sumMaxHealth;
    }

    public void StageWin()
    {
        StartCoroutine(StageWinRoutine());
    }

    IEnumerator StageWinRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        winUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        fadeEffect.FadeOut();
        yield return new WaitForSeconds(fadeEffect.fadeTime);
        GameManager.Instance.StageClear();
    }

    public void StageLose()
    {
        StartCoroutine(StageLoseRoutine());
    }

    IEnumerator StageLoseRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        loseUI.SetActive(true);
    }

    public void Continue(bool value)
    {
        int index = value ? 1 : 0;
        GameManager.Instance.MoveScene(index);
    }

    public void Pause(bool value)
    {
        player.Pause = value;
        player.AttackPress = value;
        pauseUI.SetActive(value);
        GameManager.Instance.GamePause(value);
    }
}