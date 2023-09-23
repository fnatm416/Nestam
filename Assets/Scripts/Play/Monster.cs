using System.Collections;
using UnityEngine;

//AI를 가지고 플레이어를 공격하는 적
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

        Health -= damage;

        if (Health <= 0)
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
    public float rotateSpeed;   //캐릭터 회전속도

    [Header("Component")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [Header("MonsterInfo")]
    public Character Character;
    public float Health;
    public float Power;
    public float Speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDelay;
    [SerializeField] public float dashDistance;
    [SerializeField] public float dashTime;
    [SerializeField] public float dashDelay;
    [SerializeField] public bool canAttack;
    [SerializeField] public bool canDash;
    public enum State
    {
        Idle,
        Move,
        Attack,
        Dash,
        Hit,
        Die
    }
    public State state { get; private set; }
    [SerializeField] GameObject target;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlay)
            return;

        AddGravity();
        UpdateState();
    }

    #region FSM
    public void ChangeState(State newState)
    {
        if (state != newState)
        {
            state = newState;

            if (state == State.Move) { Character.PlayAnimation("Move", true); }
            else { Character.PlayAnimation("Move", false); }

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
                    Character.Attack();

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
                    Character.OnInterrupt();
                    Character.PlayAnimation("Hit");

                    break;
                }
            case State.Die:
                {
                    StopCoroutine(HitTimer());
                    Character.OnInterrupt();
                    Character.PlayAnimation("Die");

                    break;
                }
        }
    }

    public void UpdateState()
    {
        if (Health <= 0)
            ChangeState(State.Die);

        switch (state)
        {
            case State.Idle:
                {
                    target = FindTarget();
                    if (target != null)
                    {
                        //공격
                        if (TargetDisatance() > attackRange) { ChangeState(State.Move); }
                        //이동
                        else { if (canAttack) { ChangeState(State.Attack); } }
                    }

                    break;
                }
            case State.Move:
                {
                    //대기
                    if (FindTarget() == null)
                    {
                        target = null;
                        ChangeState(State.Idle);
                        break;
                    }

                    if (TargetDisatance() <= attackRange)
                    {
                        //공격
                        if (canAttack)
                        {
                            ChangeState(State.Attack);
                        }
                        else
                            ChangeState(State.Idle);
                    }
                    else
                    {
                        //대쉬
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
                    if (TargetDisatance() <= Character.attackRange)
                    {
                        if (FindTarget() != null && ComboAttack == false)
                        {
                            ComboAttack = true;
                            //플레이어가 공격시 바라볼 방향을 지정
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
        //초기화        
        Character = transform.GetComponentInChildren<Character>();
        Character.GetComponent<CharacterController>().enabled = false;

        //캐릭터의 "캐릭터컨트롤러"를 참조
        controller = this.gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.stepOffset = 0;
        controller.center = Character.GetComponent<CharacterController>().center * Character.transform.localScale.x;
        controller.radius = Character.GetComponent<CharacterController>().radius * Character.transform.localScale.x;
        controller.height = Character.GetComponent<CharacterController>().height * Character.transform.localScale.x;

        //상태 및 스텟 초기화
        ChangeState(State.Idle);
        Health = Character.Health * GameManager.Instance.MonsterPower;
        Power = Character.Power * GameManager.Instance.MonsterPower;
        Speed = Character.Speed * GameManager.Instance.MonsterPower;
        attackRange = Character.attackRange;
        attackDelay = Character.attackDelay;
        canAttack = true;
        dashDistance = Character.DashDistance;
        dashTime = Character.DashTime;
        dashDelay = Character.DashDelay;
        canDash = true;

        //인터페이스 초기화
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
                //타겟이 죽으면 null
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
        controller.Move(transform.forward * Speed * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        Character.PlayAnimation("Dash", true);
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

        Character.PlayAnimation("Dash", false);
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