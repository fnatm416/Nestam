using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviour, IPlayable
{
    [Header("Controll")]
    public float minAngle;  //마우스 최소각도
    public float maxAngle;  //마우스 최대각도
    public float sensitivity;   //마우스 감도
    public float rotateSpeed;   //캐릭터 회전속도

    [Header("Component")]
    public GameObject cameraRoot;

    [Header("Stat")]
    public Character character;
    public float health;
    public float power;
    public float speed;

    [Header("InputSystem")]
    public Vector2 inputVec;
    public float mouseX;
    public float mouseY;
    public bool attack;
    public bool dash;

    public State state { get; set; }
    public CharacterController controller { get; set; }
    public bool comboAttack { get; set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
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
                    // 공격
                    if (attack)
                    {
                        ChangeState(State.Attack);
                    }
                    // 이동
                    else if (inputVec != Vector2.zero)
                    {
                        ChangeState(State.Move);
                    }
                    //대쉬
                    else if (dash)
                    {
                        ChangeState(State.Dash);
                    }
                    break;
                }
            case State.Move:
                {
                    // 공격
                    if (attack)
                    {
                        ChangeState(State.Attack);
                    }
                    // 이동
                    else if (inputVec == Vector2.zero)
                    {
                        ChangeState(State.Idle);
                    }
                    //대쉬
                    else if (dash)
                    {
                        ChangeState(State.Dash);
                    }
                    else
                    {
                        Move();
                    }
                    break;
                }
            case State.Attack:
                {
                    //대쉬
                    if (dash)
                    {
                        dash = false;
                    }
                    else if(attack)
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
        //캐릭터 초기화
        state = State.Idle;
        speed = character.speed;
    }

    void Move()
    {
        //플레이어의 이동방향을 지정
        Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);
        Vector3 inputDirection = new Vector3(inputVec.x, 0, inputVec.y);
        Vector3 targetDirection = Quaternion.LookRotation(cameraForward) * inputDirection;

        //플레이어의 각도 회전을 지정
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.one);
        if (targetDirection != Vector3.zero)
            targetRotation = Quaternion.LookRotation(targetDirection);

        //이동상태가 아니라면 함수 중지
        if (state != State.Move)
            return;
            
        //회전
        if (inputVec != Vector2.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //이동
        controller.Move(targetDirection.normalized * speed * Time.deltaTime);
    }

    void Attack()
    {    
        attack = false;
        controller.Move(Vector3.zero);
        character.Attack();
    }

    IEnumerator  Dash()
    {
        character.PlayAnimation("Dash", true);

        Vector3 inputDirection = new Vector3(inputVec.x, 0, inputVec.y);
        Vector3 cameraForward = new Vector3(cameraRoot.transform.forward.x, 0, cameraRoot.transform.forward.z);

        transform.forward = (inputDirection == Vector3.zero) ? 
            transform.forward : Quaternion.LookRotation(cameraForward) * inputDirection;

        float elapsedTime = 0;
        while(elapsedTime < character.dashTime)
        {
            elapsedTime += Time.deltaTime;

            controller.Move(transform.forward.normalized * character.dashSpeed * Time.deltaTime);

            yield return null;
        }

        dash = false;
        character.PlayAnimation("Dash", false);
        ChangeState(State.Idle);
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