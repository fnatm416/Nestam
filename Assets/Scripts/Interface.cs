using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    //���ݰ����� ��ü
    public bool comboAttack { get; set; }
    public void EndAttack();
}