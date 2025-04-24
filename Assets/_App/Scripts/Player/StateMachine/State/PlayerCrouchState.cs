using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerAnimState
{
    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Crouch State");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (poseLandmarkerResult.poseLandmarks == null) return;
        if (!PoseConditions.IsCrouching(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
