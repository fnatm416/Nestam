using System;
using System.Collections;

//���ݰ����� ��ü
public interface IAttackable
{  
    public bool ComboAttack { get; set; }   //�޺����� ����
    public string TargetTag { get; set; }   //Ÿ���̵� �±��� �̸�

    public void EndAttack();    //��������
    public IEnumerator AttackDelay();   //���� ����ð� ����
}

//���ݴ��ϴ� ��ü
public interface IHittable
{
    public bool IsDie { get; set; } //�׾����� üũ
    public float HitPoint { get; set; } //��ӽ��̰�, GetHit�ϰų� �ð������� �ʱ�ȭ
    public bool Recovery { get; set; }  //true�ε��� hitPoint�� ����������
    public void GetDamage(float damage);    //���ݹ޾����� ����
    public IEnumerator HitTimer();  //���ݹް� �ð��� ����ϸ� hitPoint �ʱ�ȭ
    public void EndHit();   //�´¸���� ������ ���۰��� ���·� ���ư�
    public IEnumerator HitRecovery();   //hit�� ���۵ǰ��� recovery�� �����ð� Ȱ��ȭ
}