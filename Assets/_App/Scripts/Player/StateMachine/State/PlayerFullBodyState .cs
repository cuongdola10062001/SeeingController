using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFullBodyState : PlayerState
{
    public PlayerFullBodyState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

    public override void Update()
    {
        base.Update();

        if (!PoseDetectionFullBody.Instance.IsFullBody)
        {
            stateMachine.ChangeState(player.notFullbodyState);
            return;
        }
    }
}
