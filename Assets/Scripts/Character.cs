using UnityEngine;

//�ɷ�ġ�� Ư���� ���� ĳ���͵��� ��ӽ��Ѽ� ���
public abstract class Character : MonoBehaviour
{
    [Header("Stat")]
    public float health;
    public float power;
    public float speed;
    //�뽬
    public float dashDistance;
    public float dashTime;
    public float dashDelay;
    //����
    public float attackRange;
    public float attackDelay;

    protected IAttackable attackable;
    protected IHittable hittable;
    protected Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        attackable = transform.parent.GetComponent<IAttackable>();
        hittable = transform.parent.GetComponent<IHittable>();

        Init();
    }

    //�ִϸ��̼� ȣ��(�����ε�)
    public void PlayAnimation(string name) { animator.SetTrigger(name); }
    public void PlayAnimation(string name, bool value) {  animator.SetBool(name, value); }  
    public void PlayAnimation(string name, float value) {  animator.SetFloat(name, value); }

    public void Attack() { PlayAnimation("Attack"); }   //���ݾִϸ��̼� ����    
    public void EndAttack() { attackable.EndAttack(); } //���ݸ�� ����
    public void EndHit() { hittable.EndHit(); } //�´¸�� ����


    public abstract void Init();  //ĳ���� ���� �ʱ�ȭ
    public abstract void OnInterrupt(); //ĳ���Ͱ� ���ع޾��� �� ���� ( GetHit, Die )
}