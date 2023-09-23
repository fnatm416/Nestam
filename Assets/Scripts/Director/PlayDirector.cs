using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] TextMeshProUGUI chapterUI;

    [SerializeField] FadeEffect startUI;
    [SerializeField] FadeEffect panel;
    [SerializeField] ShakeEffect shakeEffect;

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

        StartCoroutine(ShowGameStart());
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

    IEnumerator ShowGameStart()
    {
        startUI.FadeOut(0.5f);
        yield return new WaitForSeconds(startUI.fadeTime);
        startUI.FadeIn(1.0f);
        yield return new WaitForSeconds(startUI.fadeTime);
        GameManager.Instance.IsPlay = true;
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
        GameManager.Instance.IsPlay = false;
        winUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        panel.FadeOut(1.0f);
        yield return new WaitForSeconds(panel.fadeTime);

        GameManager.Instance.StageClear();
    }

    public void StageLose()
    {
        StartCoroutine(StageLoseRoutine());
    }

    IEnumerator StageLoseRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.IsPlay = false;
        loseUI.SetActive(true);
    }

    public void Continue(bool value)
    {
        string sceneName = value ? "Select" : "Title";
        SceneManager.LoadScene(sceneName);
    }

    public void Pause(bool value)
    {
        player.Pause = value;
        player.AttackPress = value;
        pauseUI.SetActive(value);
        chapterUI.text = "STAGE " + GameManager.Instance.Stage;
        GameManager.Instance.GamePause(value);
    }

    public void CameraShake()
    {
        shakeEffect.CameraShake();
    }
}