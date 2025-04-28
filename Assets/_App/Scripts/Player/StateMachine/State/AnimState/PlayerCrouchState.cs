using Mediapipe;
using Mediapipe.Tasks.Components.Containers;
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (poseLandmarkerResult.poseLandmarks == null) return;
        var landmarks = poseLandmarkerResult.poseLandmarks[0].landmarks;
        if (landmarks == null || landmarks.Count <= 0) return;

        if (!StanceEvaluatorManager.Instance.IsCrouching(landmarks))
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
