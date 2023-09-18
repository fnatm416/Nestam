using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


//사용자가 플레이하는 플레이어
public class Player : MonoBehaviour, IAttackable, IHittable
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
        yield return null;
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
    public float MinAngle;  //마우스 최소각도
    public float MaxAngle;  //마우스 최대각도
    public float Sensitivity;   //마우스 감도
    public float RotateSpeed;   //캐릭터 회전속도

    [Header("Component")]
    [SerializeField] GameObject cameraRoot;
    [SerializeField] Character character;
    [SerializeField] CharacterController controller;

    [Header("InputSystem")]
    public Vector2 InputVec;
    public float MouseX;
    public float MouseY;
    public bool AttackPress;
    public bool DashPress;

    [Header("PlayerInfo")] 
    public float health;
    public float power;
    public float speed;
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
        Dash,
        Hit,
        Die
    }
    [SerializeField] State state;

    void Start()
    {
        Init();
    }

    void Update()
    {
        AddGravity();
        UpdateState();
    }

    void LateUpdate()
    {
        CameraRotation();
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
                    AttackPress = false;
                    ComboAttack = false;
                    controller.Move(Vector3.zero);
                    character.Attack();

                    break;
                }
            case State.Dash:
                {
                    DashPress = false;
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
                    // 공격
                    if (canAttack && AttackPress)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                        break;
                    }
                    else { AttackPress = false; }


                    // 이동
                    if (InputVec != Vector2.zero)
                    {
                        ChangeState(State.Move);
                        break;
                    }

                    //대쉬
                    if (canDash && DashPress)
                    {
                        canDash = false;
                        ChangeState(State.Dash);
                    }
                    else { DashPress = false; }

                    break;
                }
            case State.Move:
                {
                    // 공격
                    if (canAttack && AttackPress)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                        break;
                    }
                    else { AttackPress = false; }

                    // 이동
                    if (InputVec == Vector2.zero)
                    {
                        ChangeState(State.Idle);
                        break;
                    }

                    //대쉬
                    if (canDash && DashPress)
                    {
                        canDash = false;
                        ChangeState(State.Dash);
                        break;
                    }
                    else { DashPress = false; }

                    Move();

                    break;
                }
            case State.Attack:
                {
                    //대쉬
                    if (DashPress) { DashPress = false; }

                    //공격
                    if (AttackPress)
                    {
                        AttackPress = false;

                        if (ComboAttack == false)
                        {
                            ComboAttack = true;
                            //플레이어가 공격시 바라볼 방향을 지정
                            Vector3 inputDirection = new Vector3(InputVec.x, 0, InputVec.y);
                            Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);
                            transform.forward = (inputDirection == Vector3.zero) ?
                                transform.forward : Quaternion.LookRotation(cameraForward) * inputDirection;
                        }
                    }

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
        character = transform.GetComponentInChildren<Character>();
        character.GetComponent<CharacterController>().enabled = false;

        //캐릭터의 "캐릭터컨트롤러"를 참조
        controller = this.gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.center = character.GetComponent<CharacterController>().center;
        controller.radius = character.GetComponent<CharacterController>().radius;
        controller.height = character.GetComponent<CharacterController>().height;

        //상태 및 스텟 초기화
        ChangeState(State.Idle);
        health = character.Health;
        power = character.Power;
        speed = character.Speed;
        attackDelay = character.attackDelay;
        canAttack = true;
        dashDistance = character.DashDistance;
        dashTime = character.DashTime;
        dashDelay = character.DashDelay;
        canDash = true;

        //인터페이스 초기화
        TargetTag = "Monster";
        HitPoint = 0;
        Recovery = false;
    }

    void AddGravity()
    {
        //중력 적용
        controller.Move(new Vector3(0, Physics.gravity.y, 0));
    }

    void Move()
    {
        //플레이어의 이동방향을 지정
        Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);
        Vector3 inputDirection = new Vector3(InputVec.x, 0, InputVec.y);
        Vector3 targetDirection = Quaternion.LookRotation(cameraForward) * inputDirection;

        //플레이어의 각도 회전을 지정
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.one);
        if (targetDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(targetDirection);

        //이동상태가 아니라면 함수 중지
        if (state != State.Move)
            return;

        //회전
        if (InputVec != Vector2.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);

        //이동
        controller.Move(targetDirection.normalized * speed * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        character.PlayAnimation("Dash", true);

        Vector3 inputDirection = new Vector3(InputVec.x, 0, InputVec.y);
        Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);

        transform.forward = (inputDirection == Vector3.zero) ?
            transform.forward : Quaternion.LookRotation(cameraForward) * inputDirection;

        float dashPower = dashDistance / dashTime;

        float elapsedTime = 0;
        while (elapsedTime < dashTime)
        {
            elapsedTime += Time.deltaTime;

            controller.Move(transform.forward * dashPower * Time.deltaTime);

            yield return null;
        }

        DashPress = false;
        character.PlayAnimation("Dash", false);
        StartCoroutine(DashDelay());
        ChangeState(State.Idle);
    }

    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }

    void CameraRotation()
    {
        MouseY = Mathf.Clamp(MouseY, MinAngle, MaxAngle);

        cameraRoot.transform.rotation = Quaternion.Euler(MouseY, MouseX, 0f);
    }
    #endregion

    #region InputSystem
    void OnMove(InputValue value)
    {
        InputVec = value.Get<Vector2>();
    }
    void OnLook(InputValue value)
    {
        MouseX += value.Get<Vector2>().x * Sensitivity;
        MouseY -= value.Get<Vector2>().y * Sensitivity;
    }

    void OnAttack(InputValue value)
    {
        AttackPress = value.isPressed;
    }

    void OnDash(InputValue value)
    {
        DashPress = value.isPressed;
    }
    #endregion
}