using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인터페이스 : 플레이어와 적이 공통적으로 가져야할 특성들을 작성
public interface IPlayable
{
    public Character character { get; set; }
    public CharacterController controller { get; set; }
    public bool comboAttack { get; set; }

    public float health { get; set; }
    public float power { get; set; }
    public float speed { get; set; }

    public void EndAttack();
}