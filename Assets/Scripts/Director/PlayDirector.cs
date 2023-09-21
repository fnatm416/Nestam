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

    [SerializeField] GameObject WinUi;
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
        WinUi.SetActive(false);
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
        WinUi.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        fadeEffect.FadeOut();
        yield return new WaitForSeconds(fadeEffect.fadeTime);
        GameManager.Instance.StageClear();
    }
}