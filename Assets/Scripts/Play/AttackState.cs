using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    IAttackable owner;
    Character character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        owner = animator.transform.parent.GetComponent<IAttackable>();
        character = animator.GetComponent<Character>();
        owner.ComboAttack = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
        if (owner.ComboAttack)
        {
            character.PlayAnimation("Attack");
            return;
        }
    }
}