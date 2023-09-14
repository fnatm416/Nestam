using UnityEngine;

//�ɷ�ġ�� Ư���� ���� ĳ���͵��� ��ӽ��Ѽ� ���
public class Character : MonoBehaviour
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
    }

    public virtual void Attack()
    {
        PlayAnimation("Attack");
    }

    public void PlayAnimation(string name) { animator.SetTrigger(name); }
    public void PlayAnimation(string name, bool value) {  animator.SetBool(name, value); }  
    public void PlayAnimation(string name, float value) {  animator.SetFloat(name, value); }

    public void EndAttack()
    {
        attackable.EndAttack();
    }

    public void EndHit()
    {
        hittable.EndHit();
    }
}