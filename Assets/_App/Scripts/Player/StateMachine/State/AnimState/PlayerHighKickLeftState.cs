using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHighKickLeftState : PlayerAnimState
{
    public PlayerHighKickLeftState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        
        if (this.IsCurrentAnimationDone())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
