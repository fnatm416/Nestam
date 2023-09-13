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
    public void GetHit(float damage);
}