using UnityEngine;

//능력치와 특성을 가진 캐릭터들을 상속시켜서 사용
public class Character : MonoBehaviour
{
    [Header("Stat")]
    public float health;
    public float power;
    public float speed;
    //대쉬
    public float dashDistance;
    public float dashTime;
    public float dashDelay;
    //공격
    public float attackRange;
    public float attackDelay;

    protected IAttackable attackable;
    protected IHittable hittable;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        attackable = transform.parent.GetComponent<IAttackable>();
        hittable = transform.parent.GetComponent<IHittable>();
    }

    public void PlayAnimation(string name) { animator.SetTrigger(name); }
    public void PlayAnimation(string name, bool value) {  animator.SetBool(name, value); }  
    public void PlayAnimation(string name, float value) {  animator.SetFloat(name, value); }

    public void Attack() { PlayAnimation("Attack"); }   //공격애니메이션 실행    
    public void EndAttack() { attackable.EndAttack(); } //공격모션 종료
    public void EndHit() { hittable.EndHit(); } //맞는모션 종료
}