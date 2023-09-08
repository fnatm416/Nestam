using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    //공격가능한 개체
    public bool comboAttack { get; set; }
    public void EndAttack();
}