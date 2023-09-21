using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

//AI�� ������ �÷��̾ �����ϴ� ��
public class Monster : MonoBehaviour, IAttackable, IHittable
{
    #region IAttackable
    public bool ComboAttack { get; set; }
    public string TargetTag { get; set; }
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
    public bool IsDie { get; set; }
    public float HitPoint { get; set; }
    public bool Recovery { get; set; }
    public void GetDamage(float damage)
    {
        if (IsDie)
            return;

        health -= damage;

        if (health <= 0)
        {
            IsDie = true;
            ChangeState(State.Die);
            return;
        }

        if (Recovery == false)
        {
            StopCoroutine(HitTimer());

            HitPoint += damage;

            if (HitPoint >= GameManager.GetHitDamage)
            {
                HitPoint = 0;
                ChangeState(State.Hit);
                return;
            }

            StartCoroutine(HitTimer());
        }
    }

    public IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(GameManager.HitResetTime);
        HitPoint = 0;
    }
    public void EndHit()
    {
        canAttack = true;
        ChangeState(State.Idle);
        StartCoroutine(HitRecovery());
    }
    public IEnumerator HitRecovery()
    {
        yield return new WaitForSeconds(GameManager.RecoveryTime);
        Recovery = false;
    }
    #endregion

    [Header("Controll")]
    public float rotateSpeed;   //ĳ���� ȸ���ӵ�

    [Header("Component")]
    [SerializeField] Character character;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [Header("MonsterInfo")]
    [SerializeField] float health;
    [SerializeField] float power;
    [SerializeField] float speed;
    public float attackRange;
    public float attackDelay;
    public float dashDistance;
    public float dashTime;
    public float dashDelay;
    public bool canAttack;
    public bool canDash;
    public enum State
    {
        Idle,
        Move,
        Attack,
        Dash,
        Hit,
        Die
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

            if (state == State.Move) { character.PlayAnimation("Move", true); }
            else { character.PlayAnimation("Move", false); }

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
            case State.Move:
                {
                    break;
                }
            case State.Attack:
                {
                    canAttack = false;
                    controller.Move(Vector3.zero);
                    character.Attack();

                    break;
                }
            case State.Dash:
                {
                    StartCoroutine(Dash());

                    break;
                }
            case State.Hit:
                {
                    Recovery = true;
                    character.OnInterrupt();
                    character.PlayAnimation("Hit");

                    break;
                }
            case State.Die:
                {
                    StopCoroutine(HitTimer());
                    character.OnInterrupt();
                    character.PlayAnimation("Die");

                    GameManager.Instance.MonsterCount--;
                    if (GameManager.Instance.MonsterCount<=0) { GameManager.Instance.StageClear(); }

                    break;
                }
        }
    }

    public void UpdateState()
    {
        if (health <= 0)
            ChangeState(State.Die);

        switch (state)
        {
            case State.Idle:
                {
                    target = FindTarget();
                    if (target != null) 
                    { 
                        //����
                        if (TargetDisatance() > attackRange) { ChangeState(State.Move); }
                        //�̵�
                        else { if (canAttack) { ChangeState(State.Attack); } }
                    }

                    break;
                }
            case State.Move:
                {
                    //���
                    if (FindTarget() == null)
                    {
                        target = null;
                        ChangeState(State.Idle);
                        break;
                    }
                    
                    if (TargetDisatance() <= attackRange)
                    {
                        //����
                        if (canAttack)
                        {
                            ChangeState(State.Attack);
                        }
                        else
                            ChangeState(State.Idle);
                    }
                    else
                    {
                        //�뽬
                        if (canDash && TargetDisatance() > (dashDistance + 0.25f))
                        {
                            canDash = false;
                            ChangeState(State.Dash);
                        }
                        else { MoveToTarget(); }
                    }

                    break;
                }
            case State.Attack:
                {
                    if (TargetDisatance() <= character.attackRange)
                    {
                        if (FindTarget() != null && ComboAttack == false)
                        {
                            ComboAttack = true;
                            //�÷��̾ ���ݽ� �ٶ� ������ ����
                            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
                            transform.forward = targetDirection;
                        }
                    }
                    else { ComboAttack = false; }

                    break;
                }
            case State.Dash:
                {
                    break;
                }
            case State.Die:
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
        controller = this.gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.stepOffset = 0;
        controller.center = character.GetComponent<CharacterController>().center * character.transform.localScale.x;
        controller.radius = character.GetComponent<CharacterController>().radius * character.transform.localScale.x;
        controller.height = character.GetComponent<CharacterController>().height * character.transform.localScale.x;

        //���� �� ���� �ʱ�ȭ
        ChangeState(State.Idle);
        health = character.Health;
        power = character.Power;
        speed = character.Speed;
        attackRange = character.attackRange;
        attackDelay = character.attackDelay;
        canAttack = true;
        dashDistance = character.DashDistance;
        dashTime = character.DashTime;
        dashDelay = character.DashDelay;
        canDash = true;

        //�������̽� �ʱ�ȭ
        TargetTag = "Player";
        HitPoint = 0;
        Recovery = false;
    }

    void AddGravity()
    {
        if (!controller.isGrounded)
            controller.Move(new Vector3(0, Physics.gravity.y, 0));
    }

    GameObject FindTarget()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.tag == TargetTag)
            {
                //Ÿ���� ������ null
                IHittable iHittable = obj.GetComponent<IHittable>();
                bool targetDie = (iHittable != null && iHittable.IsDie) ? true : false;
                if (targetDie) { return null; }

                return obj;
            }
        }

        return null;
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