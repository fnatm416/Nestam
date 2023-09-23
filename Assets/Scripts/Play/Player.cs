using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


//����ڰ� �÷����ϴ� �÷��̾�
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
    public float MinAngle;  //���콺 �ּҰ���
    public float MaxAngle;  //���콺 �ִ밢��
    public float Sensitivity;   //���콺 ����
    public float RotateSpeed;   //ĳ���� ȸ���ӵ�

    [Header("Component")]
    [SerializeField] GameObject cameraRoot;
    [SerializeField] CharacterController controller;

    [Header("InputSystem")]
    public Vector2 InputVec;
    public float MouseX;
    public float MouseY;
    public bool AttackPress;
    public bool DashPress;
    public bool Pause;
    public bool MouseLock;

    [Header("PlayerInfo")]
    public Character Character;
    public float Health;
    public float Power;
    public float Speed;
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
    public State state { get; private set; }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Pause)
            return;

        AddGravity();
        UpdateState();
    }

    void LateUpdate()
    {
        if (Pause)
            return;

        CameraRotation();
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
                    AttackPress = false;
                    ComboAttack = false;
                    controller.Move(Vector3.zero);
                    Character.Attack();

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
        {
            //����
            ChangeState(State.Die);
            return;
        }

        if (!GameManager.Instance.IsPlay && GameManager.Instance.MonsterCount > 0)
        {
            //���� ���� �� ������ ����
            ChangeState(State.Idle);
            return;
        }
            

        switch (state)
        {
            case State.Idle:
                {
                    // ����
                    if (canAttack && AttackPress)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                        break;
                    }
                    else { AttackPress = false; }


                    // �̵�
                    if (InputVec != Vector2.zero)
                    {
                        ChangeState(State.Move);
                        break;
                    }

                    //�뽬
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
                    // ����
                    if (canAttack && AttackPress)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                        break;
                    }
                    else { AttackPress = false; }

                    // �̵�
                    if (InputVec == Vector2.zero)
                    {
                        ChangeState(State.Idle);
                        break;
                    }

                    //�뽬
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
                    //�뽬
                    if (DashPress) { DashPress = false; }

                    //����
                    if (AttackPress)
                    {
                        AttackPress = false;

                        if (ComboAttack == false)
                        {
                            ComboAttack = true;
                            //�÷��̾ ���ݽ� �ٶ� ������ ����
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
        //�ʱ�ȭ
        Pause = false;
        MouseLock = true;
        Cursor.lockState = CursorLockMode.Locked;

        Character = transform.GetComponentInChildren<Character>();
        Character.GetComponent<CharacterController>().enabled = false;

        //ĳ������ "ĳ������Ʈ�ѷ�"�� ����
        controller = this.gameObject.AddComponent<CharacterController>();
        controller.slopeLimit = 0;
        controller.stepOffset = 0;
        controller.center = Character.GetComponent<CharacterController>().center * Character.transform.localScale.x;
        controller.radius = Character.GetComponent<CharacterController>().radius * Character.transform.localScale.x;
        controller.height = Character.GetComponent<CharacterController>().height * Character.transform.localScale.x;

        //���� �� ���� �ʱ�ȭ
        ChangeState(State.Idle);
        Health = Character.Health;
        Power = Character.Power;
        Speed = Character.Speed;
        attackDelay = Character.attackDelay;
        canAttack = true;
        dashDistance = Character.DashDistance;
        dashTime = Character.DashTime;
        dashDelay = Character.DashDelay;
        canDash = true;

        //�������̽� �ʱ�ȭ
        TargetTag = "Monster";
        HitPoint = 0;
        Recovery = false;
    }

    void AddGravity()
    {
        //�߷� ����
        controller.Move(new Vector3(0, Physics.gravity.y, 0));
    }

    void Move()
    {
        //�÷��̾��� �̵������� ����
        Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);
        Vector3 inputDirection = new Vector3(InputVec.x, 0, InputVec.y);
        Vector3 targetDirection = Quaternion.LookRotation(cameraForward) * inputDirection;

        //�÷��̾��� ���� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.one);
        if (targetDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(targetDirection);

        //�̵����°� �ƴ϶�� �Լ� ����
        if (state != State.Move)
            return;

        //ȸ��
        if (InputVec != Vector2.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);

        //�̵�
        controller.Move(targetDirection.normalized * Speed * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        Character.PlayAnimation("Dash", true);

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
        Character.PlayAnimation("Dash", false);
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
        if (state == State.Die)
            return;

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

    void OnPause()
    {
        if (!GameManager.Instance.IsPlay)
            return;

        this.Pause = !this.Pause;
        PlayDirector.Instance.Pause(this.Pause);
    }

    void OnMouseLock()
    {
        bool cursorLocked = (Cursor.lockState == CursorLockMode.Locked);
        Cursor.lockState = cursorLocked ? CursorLockMode.None : CursorLockMode.Locked;
    }
    #endregion
}