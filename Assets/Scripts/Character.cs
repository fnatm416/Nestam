using UnityEngine;

//�ɷ�ġ�� Ư���� ���� ĳ���͵��� ��ӽ��Ѽ� ���
public class Character : MonoBehaviour
{
    [Header("Stat")]
    public float health;
    public float power;
    public float speed;
    public float dashSpeed;
    public float dashTime;

    protected IPlayable owner;
    protected Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        owner = transform.parent.GetComponent<IPlayable>();
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
        owner.EndAttack();
    }
}