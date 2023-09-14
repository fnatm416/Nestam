using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    //공격가능한 개체
    public bool comboAttack { get; set; }
    public string targetTag { get; set; }

    public void EndAttack();
    public IEnumerator AttackDelay();
}

public interface IHittable
{
    //공격당하는 개체
    public float hitPoint { get; set; } //계속쌓이고, GetHit하거나 시간지나면 초기화
    public bool recovery { get; set; }  //true인동안 hitPoint가 쌓이지않음
    public void GetDamage(float damage);
    public IEnumerator HitDelay();
    public void EndHit();
    public IEnumerator HitRecovery();
}