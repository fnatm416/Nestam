using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//AI�� ������ �÷��̾ �����ϴ� ��
public class Monster : MonoBehaviour, IAttackable
{
    public bool comboAttack { get; set; }
    public void EndAttack() { }

    [SerializeField] Character character;
    [SerializeField] CharacterController controller;
    [SerializeField] float health;
    [SerializeField] float power;
    [SerializeField] float speed;

    public enum State
    {
        Idle,
        Trace,
        Attack,
        Dash
    }
    [SerializeField] State state;
    [SerializeField] LayerMask targetLayer;
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
                    FindTarget();
                    break;
                }
            case State.Trace:
                {
                    TraceTarget();
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
        //�ʱ�ȭ        
        character = transform.GetComponentInChildren<Character>();
        character.GetComponent<CharacterController>().enabled = false;

        //ĳ������ "ĳ������Ʈ�ѷ�"�� ����
        controller = gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.center = character.GetComponent<CharacterController>().center;
        controller.radius = character.GetComponent<CharacterController>().radius;
        controller.height = character.GetComponent<CharacterController>().height;

        //���� �� ���� �ʱ�ȭ
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
        if (target != null)
            ChangeState(State.Trace);

        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.layer == targetLayer)
            {
                target = obj;
                ChangeState(State.Trace);
                return;
            }
        }
    }

    void TraceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.forward = direction;
        controller.Move(direction * speed * Time.deltaTime);
    }
    #endregion
}