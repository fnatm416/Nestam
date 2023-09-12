using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//����ڰ� �÷����ϴ� �÷��̾�
public class Player : MonoBehaviour, IAttackable
{
    public bool comboAttack { get; set; }
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

    [Header("Controll")]
    public float minAngle;  //���콺 �ּҰ���
    public float maxAngle;  //���콺 �ִ밢��
    public float sensitivity;   //���콺 ����
    public float rotateSpeed;   //ĳ���� ȸ���ӵ�

    [Header("Component")]
    public GameObject cameraRoot;

    [Header("InputSystem")]
    public Vector2 inputVec;
    public float mouseX;
    public float mouseY;
    public bool attack;
    public bool dash;

    [SerializeField] Character character;
    [SerializeField] CharacterController controller;
    [SerializeField] float health;
    [SerializeField] float power;
    [SerializeField] float speed;
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
    State state;
    [SerializeField] LayerMask targetLayer;

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
            InitState(state);
        }
    }
    public void InitState(State newState)
    {
        switch (state)
        {
            case State.Idle:
                {
                    attack = false;
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
                    attack = false;
                    comboAttack = false;

                    character.PlayAnimation("Move", false);
                    Attack();
                    break;
                }
            case State.Dash:
                {
                    dash = false;
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
                    // ����
                    if (canAttack && attack)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                    }
                    // �̵�
                    else if (inputVec != Vector2.zero)
                    {
                        ChangeState(State.Move);
                    }
                    //�뽬
                    else if (dash)
                    {
                        if (canDash)
                        {
                            canDash = false;
                            ChangeState(State.Dash);
                        }
                        else { dash = false; }
                    }
                    break;
                }
            case State.Move:
                {
                    // ����
                    if (canAttack && attack)
                    {
                        canAttack = false;
                        ChangeState(State.Attack);
                    }
                    // �̵�
                    else if (inputVec == Vector2.zero)
                    {
                        ChangeState(State.Idle);
                    }
                    //�뽬
                    else if (dash)
                    {
                        if (canDash)
                        {
                            canDash = false;
                            ChangeState(State.Dash);
                        }
                        else { dash = false; }
                    }
                    else
                    {
                        Move();
                    }
                    break;
                }
            case State.Attack:
                {
                    //�뽬
                    if (dash)
                    {
                        dash = false;
                    }
                    else if (attack)
                    {
                        attack = false;
                        comboAttack = true;
                    }
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
        this.health = character.health;
        this.power = character.power;
        this.speed = character.speed;
        this.attackDelay = character.attackDelay;
        canAttack = true;
        this.dashDistance = character.dashDistance;       
        this.dashTime = character.dashTime;       
        this.dashDelay = character.dashDelay;       
        canDash = true;
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
        Vector3 inputDirection = new Vector3(inputVec.x, 0, inputVec.y);
        Vector3 targetDirection = Quaternion.LookRotation(cameraForward) * inputDirection;

        //�÷��̾��� ���� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.one);
        if (targetDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(targetDirection);

        //�̵����°� �ƴ϶�� �Լ� ����
        if (state != State.Move)
            return;

        //ȸ��
        if (inputVec != Vector2.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //�̵�
        controller.Move(targetDirection.normalized * speed * Time.deltaTime);
    }

    void Attack()
    {
        attack = false;
        controller.Move(Vector3.zero);
        character.Attack();
    }

    IEnumerator Dash()
    {
        character.PlayAnimation("Dash", true);

        Vector3 inputDirection = new Vector3(inputVec.x, 0, inputVec.y);
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

        dash = false;
        character.PlayAnimation("Dash", false);
        ChangeState(State.Idle);
        StartCoroutine(DashDelay());
    }

    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
    }

    void CameraRotation()
    {
        mouseY = Mathf.Clamp(mouseY, minAngle, maxAngle);

        cameraRoot.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }
    #endregion

    #region InputSystem
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    void OnLook(InputValue value)
    {
        mouseX += value.Get<Vector2>().x * sensitivity;
        mouseY -= value.Get<Vector2>().y * sensitivity;
    }

    void OnAttack(InputValue value)
    {
        attack = value.isPressed;
    }

    void OnDash(InputValue value)
    {
        dash = value.isPressed;
    }
    #endregion
}