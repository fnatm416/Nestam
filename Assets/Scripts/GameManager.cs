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

    public Transform playerPosition; //�÷��̾� ���� ��ġ
    public Player player;   //���� �÷��̾�
    public Character playerCharacter;   //���� �÷��̾��� ĳ����

    public Transform[] monsterPositions;    //���� ���� ��ġ
    public Monster[] monsters;  //����� ���͵�

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
        //�ʱ�ȭ �۾�
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //�÷��̾� �� ĳ���� ����
        player = Instantiate(player);
        Instantiate(playerCharacter, player.transform, false);
        player.transform.position = playerPosition.position;
        virtualCamera.Follow = player.transform.Find("CameraRoot");
    }

    public void StageClear()
    {
        //�÷��̾ ���� ����ġ�� �̰��� ��(Select��)
    }

    public void GameOver()
    {
        //�÷��̾ ������ �׾��� ��
    }
    #endregion
}