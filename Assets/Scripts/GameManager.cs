using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public const float GetHitDamage = 20.0f;
    public const float HitResetTime = 3.0f;
    public const float RecoveryTime = 2.0f;

    public CinemachineVirtualCamera virtualCamera;

    public Transform playerPosition; //플레이어 스폰 위치
    public Player player;   //현재 플레이어
    public Character playerCharacter;   //현재 플레이어의 캐릭터

    public Transform[] monsterPositions;    //몬스터 스폰 위치
    public Monster[] monsters;  //상대할 몬스터들

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
        GameStart();
    }

    #region Methods
    public void GameStart()
    {
        //초기화 작업
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //플레이어 및 캐릭터 생성
        player = Instantiate(player);
        Instantiate(playerCharacter, player.transform, false);
        player.transform.position = playerPosition.position;
        virtualCamera.Follow = player.transform.Find("CameraRoot");
    }

    public void StageClear()
    {
        //플레이어가 적을 물리치고 이겼을 때(Select씬)
    }

    public void GameOver()
    {
        //플레이어가 적에게 죽었을 때
    }
    #endregion
}