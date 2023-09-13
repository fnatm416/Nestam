using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    //���ݰ����� ��ü
    public bool comboAttack { get; set; }
    public string targetTag { get; set; }
    public void EndAttack();
    public IEnumerator AttackDelay();
}

public interface IHittable
{
    //���ݴ��ϴ� ��ü
    public void GetHit(float damage);
}