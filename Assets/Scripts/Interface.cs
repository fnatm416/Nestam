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
    public float hitPoint { get; set; } //��ӽ��̰�, GetHit�ϰų� �ð������� �ʱ�ȭ
    public bool recovery { get; set; }  //true�ε��� hitPoint�� ����������
    public void GetDamage(float damage);
    public IEnumerator HitDelay();
    public void EndHit();
    public IEnumerator HitRecovery();
}