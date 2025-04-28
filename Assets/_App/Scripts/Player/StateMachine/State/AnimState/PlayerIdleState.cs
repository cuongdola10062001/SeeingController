using Mediapipe;
using Mediapipe.Tasks.Components.Containers;
using UnityEngine;

public class PlayerIdleState : PlayerAnimState
{
    private JumpDetector jumpDetector = new JumpDetector();
    private SideJumpDetector sideJumpDetector = new SideJumpDetector();

    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector3.zero;

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
        if(landmarks == null || landmarks.Count<=0) return;
        var direction = sideJumpDetector.DetectDirection(landmarks);

        if (StanceEvaluatorManager.Instance.IsCrouching(landmarks))
        {
            stateMachine.ChangeState(player.crouchState);
        }
        else if (jumpDetector.IsJumping(landmarks))
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if (direction == SideJumpDirection.Left)
        {
            stateMachine.ChangeState(player.dodgeLeftState);
        }
        else if (direction == SideJumpDirection.Right)
        {
            stateMachine.ChangeState(player.dodgeRightState);
        }
        else if (StanceEvaluatorManager.Instance.IsPunchLeft(landmarks))
        {
            stateMachine.ChangeState(player.punchHookLeftState);
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickLeft(landmarks))
        {
            stateMachine.ChangeState(player.highKickLeftState);
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickRight(landmarks))
        {
            stateMachine.ChangeState(player.highKickRightState);
        }

    }
}
