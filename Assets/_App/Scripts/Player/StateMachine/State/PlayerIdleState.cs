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
        this.player.anim.gameObject.transform.position = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (poseLandmarkerResult.poseLandmarks == null) return;

        /*if (PoseConditions.IsCrouching(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.crouchState);
        }*/
        var direction = sideJumpDetector.DetectDirection(poseLandmarkerResult.poseLandmarks[0].landmarks);

        if (StanceEvaluatorManager.Instance.IsCrouching(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.crouchState);
        }
        else if (jumpDetector.IsJumping(poseLandmarkerResult.poseLandmarks[0].landmarks))
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
        else if (StanceEvaluatorManager.Instance.IsPunchLeft(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookLeftState);
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickLeft(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.highKickLeftState);
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.highKickRightState);
        }

    }
}
