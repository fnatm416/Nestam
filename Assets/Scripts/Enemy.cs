using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//AI를 가지고 플레이어를 공격하는 적
public class Enemy : MonoBehaviour, IPlayable
{
    #region IPlayable
    public Character character { get; set; }
    public CharacterController controller { get; set; }
    public bool comboAttack { get; set; }

    public float health { get; set; }
    public float power { get; set; }
    public float speed { get; set; }

    public void EndAttack()
    {

    }
    #endregion

    public enum State
    {
        Idle,
        Trace,
        Attack,
        Dash
    }
    State state;

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateState();
    }

    #region FSM
    public void ChangeState(State newState)
    {
        if (state != newState)
        {
            state = newState;
            InitState(state);
        }
    }

    public void InitState(State newState)
    {
        switch (state)
        {
            case State.Idle:
                {
                    break;
                }
            case State.Trace:
                {
                    break;
                }
            case State.Attack:
                {
                    break;
                }
            case State.Dash:
                {
                    break;
                }
        }
    }

    public void UpdateState()
    {
        switch (state)
        {
            case State.Idle:
                {
                    break;
                }
            case State.Trace:
                {
                    break;
                }
            case State.Attack:
                {
                    break;
                }
            case State.Dash:
                {
                    break;
                }
        }
    }
    #endregion

    #region Methods
    public void Init()
    {
        //캐릭터 초기화        
        character = transform.GetComponentInChildren<Character>();
        character.GetComponent<CharacterController>().enabled = false;

        controller = gameObject.AddComponent<CharacterController>();
        controller.center = character.GetComponent<CharacterController>().center;
        controller.radius = character.GetComponent<CharacterController>().radius;
        controller.height = character.GetComponent<CharacterController>().height;

        state = State.Idle;
        speed = character.speed;
    }
    #endregion
}
