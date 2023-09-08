using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������̽� : �÷��̾�� ���� ���������� �������� Ư������ �ۼ�
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