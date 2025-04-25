using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeRightState : PlayerAnimState
{
    public PlayerDodgeRightState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector3(this.player.sideJumpForce, rb.velocity.y, rb.velocity.z);
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
