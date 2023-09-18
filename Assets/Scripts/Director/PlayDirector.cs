using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDirector : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform[] monsterPositions;


    void Start()
    {
        CreatePlayer();
        CreateMonsters();
    }

    void Update()
    {

    }

    void CreatePlayer()
    {
        GameManager gm = GameManager.Instance;

        Player player = Instantiate(gm.PlayerPrefab);
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

        for (int i = 0; i < gm.StageDatas[gm.Stage].monsters.Count; i++)
        {
            if (gm.StageDatas[gm.Stage].monsters[i] == null)
                continue;

            Monster monster = Instantiate(gm.MonsterPrefab);
            Instantiate(gm.StageDatas[gm.Stage].monsters[i], monster.transform, false);
            monster.transform.position = monsterPositions[i].transform.position;
        }
    }
}
