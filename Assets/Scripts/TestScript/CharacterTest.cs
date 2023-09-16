using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterTest : MonoBehaviour
{
    public Player Player;
    public Character PlayerCharacter;

    public float health;

    void Start()
    {
        Player = Instantiate(Player);
        PlayerCharacter = Instantiate(PlayerCharacter, Player.transform, false);
        Player.transform.position = GameObject.Find("PlayerPosition").transform.position;
        GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = Player.transform.Find("CameraRoot");

        Invoke("SetHealth", 1.0f);       
    }

    void SetHealth()
    {
        Player.health = this.health;
    }
}
