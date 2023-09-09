using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//AI를 가지고 플레이어를 공격하는 적
public class Monster : MonoBehaviour, IAttackable
{
    public bool comboAttack { get; set; }
    public void EndAttack() { }

    [Header("Controll")]
    public float rotateSpeed;   //캐릭터 회전속도

    [SerializeField] Character character;
    [SerializeField] CharacterController controller;
    [SerializeField] float health;
    [SerializeField] float power;
    [SerializeField] float speed;

    public enum State
    {
        Idle,
        Move,
        Attack,
        Dash
    }
    [SerializeField] State state;
    [SerializeField] string targetTag;
    [SerializeField] GameObject target;

    void Start()
    {
        Init();
    }

    void Update()
    {
        AddGravity();
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
                    character.PlayAnimation("Move", false);
                    break;
                }
            case State.Move:
                {
                    character.PlayAnimation("Move", true);
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
                    FindTarget();
                    break;
                }
            case State.Move:
                {
                    if (target) { MoveToTarget(); }
                    else { ChangeState(State.Idle); }
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
        //초기화        
        character = transform.GetComponentInChildren<Character>();
        character.GetComponent<CharacterController>().enabled = false;

        //캐릭터의 "캐릭터컨트롤러"를 참조
        controller = gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.center = character.GetComponent<CharacterController>().center;
        controller.radius = character.GetComponent<CharacterController>().radius;
        controller.height = character.GetComponent<CharacterController>().height;

        //상태 및 스텟 초기화
        ChangeState(State.Idle);
        speed = character.speed;
    }

    void AddGravity()
    {
        if (!controller.isGrounded)
            controller.Move(new Vector3(0, Physics.gravity.y, 0));
    }

    void FindTarget()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject obj in objects)
        {
            if(obj.tag == targetTag)
            {
                target = obj;
                ChangeState(State.Move);
                return;
            }    
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
        controller.Move(transform.forward * speed * Time.deltaTime);
    }
    #endregion
}