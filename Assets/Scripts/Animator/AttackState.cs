using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    IPlayable owner;
    Character character;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        owner = animator.transform.parent.GetComponent<IPlayable>();
        character = animator.GetComponent<Character>();
        owner.comboAttack = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (owner.comboAttack)
        {
            character.PlayAnimation("Attack");
            return;
        }        
    }
}