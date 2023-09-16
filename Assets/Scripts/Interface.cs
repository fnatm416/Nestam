using System;
using System.Collections;

//공격가능한 개체
public interface IAttackable
{  
    public bool ComboAttack { get; set; }   //콤보연계 여부
    public string TargetTag { get; set; }   //타겟이될 태그의 이름

    public void EndAttack();    //공격종료
    public IEnumerator AttackDelay();   //공격 재사용시간 적용
}

//공격당하는 개체
public interface IHittable
{
    public bool IsDie { get; set; } //죽었는지 체크
    public float HitPoint { get; set; } //계속쌓이고, GetHit하거나 시간지나면 초기화
    public bool Recovery { get; set; }  //true인동안 hitPoint가 쌓이지않음
    public void GetDamage(float damage);    //공격받았을때 실행
    public IEnumerator HitTimer();  //공격받고 시간이 경과하면 hitPoint 초기화
    public void EndHit();   //맞는모션이 끝나고 조작가능 상태로 돌아감
    public IEnumerator HitRecovery();   //hit가 시작되고나서 recovery를 일정시간 활성화
}