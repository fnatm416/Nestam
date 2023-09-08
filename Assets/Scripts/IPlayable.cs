using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Move,
    Attack,
    Dash
}

public interface IPlayable
{
    public State state { get; set; }
    public CharacterController controller { get; set; }
    public bool comboAttack { get; set; }

    void ChangeState(State state);
    void InitState(State state);
    void UpdateState();
}