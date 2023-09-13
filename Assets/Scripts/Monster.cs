using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//AI를 가지고 플레이어를 공격하는 적
public class Monster : MonoBehaviour, IAttackable, IHittable
{
    #region IAttackable
    public bool comboAttack { get; set; }
    public string targetTag { get; set; }
    public void EndAttack()
    {
        ChangeState(State.Idle);
        StartCoroutine(AttackDelay());
    }
    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }
    #endregion

    #region IHittable
    public void GetHit(float damage)
    {
        health -= damage;
    }
    #endregion

    [Header("Controll")]
    public float rotateSpeed;   //캐릭터 회전속도

    [SerializeField] Character character;
    [SerializeField] CharacterController controller;

    [SerializeField] float health;
    [SerializeField] float power;
    [SerializeField] float speed;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay;
    [SerializeField] float dashDistance;
    [SerializeField] float dashTime;
    [SerializeField] float dashDelay;
    [SerializeField] bool canAttack;
    [SerializeField] bool canDash;

    public enum State
    {
        Idle,
        Move,
        Attack,
        Dash
    }
    [SerializeField] State state;
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
                    character.PlayAnimation("Move", false);
                    controller.Move(Vector3.zero);
                    character.Attack();
                    break;
                }
            case State.Dash:
                {
                    character.PlayAnimation("Move", false);
                    StartCoroutine(Dash());
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
                    if (target)
                    {
                        if (TargetDisatance() <= attackRange)
                        {
                            if (canAttack)
                            {
                                canAttack = false;
                                ChangeState(State.Attack);
                            }
                        }
                        else
                        {
                            if (TargetDisatance() > dashDistance && canDash)
                            {
                                canDash = false;
                                ChangeState(State.Dash);
                            }
                            else { MoveToTarget(); }
                        }
                    }
                    else { ChangeState(State.Idle); }

                    break;
                }
            case State.Attack:
                {
                    if (TargetDisatance() <= character.attackRange) { comboAttack = true; }
                    else { comboAttack = false; }

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
        //상태 및 스텟 초기화
        ChangeState(State.Idle);
        this.health = character.health;
        this.power = character.power;
        this.speed = character.speed;
        this.attackRange = character.attackRange;
        this.attackDelay = character.attackDelay;
        canAttack = true;
        this.dashDistance = character.dashDistance;
        this.dashTime = character.dashTime;
        this.dashDelay = character.dashDelay;
        canDash = true;
        targetTag = "Player";
    }

    void AddGravity()
    {
        if (!controller.isGrounded)
            controller.Move(new Vector3(0, Physics.gravity.y, 0));
    }

    void FindTarget()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.tag == targetTag)
            {
                target = obj;
                ChangeState(State.Move);
                return;
            }
        }
    }

    float TargetDisatance()
    {
        if (target)
            return Vector3.Distance(target.transform.position, this.transform.position);
        else
            return Mathf.Infinity;
    }

    void MoveToTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        character.PlayAnimation("Dash", true);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.forward = direction;

        float dashPower = dashDistance / dashTime;

        float elapsedTime = 0;
        while (elapsedTime < dashTime)
        {
            elapsedTime += Time.deltaTime;

            controller.Move(transform.forward * dashPower * Time.deltaTime);

            yield return null;
        }

        character.PlayAnimation("Dash", false);
        ChangeState(State.Idle);
        StartCoroutine(DashDelay());
    }

    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }
    #endregion
}