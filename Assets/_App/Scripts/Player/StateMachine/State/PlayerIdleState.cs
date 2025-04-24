using Mediapipe.Tasks.Components.Containers;
using UnityEngine;

public class PlayerIdleState : PlayerAnimState
{
    private JumpDetector jumpDetector = new JumpDetector();

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
        /* if(xInput != 0 )
             stateMachine.ChangeState(player.moveState);*/
        if (poseLandmarkerResult.poseLandmarks == null) return;

        if (PoseConditions.IsCrouching(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.crouchState);
            //return;
        }
        else if (jumpDetector.IsJumping(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            //stateMachine.ChangeState();
        }
        else if (StanceEvaluatorManager.Instance.IsPunchLeft(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookLeftState);
            //return;
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
            //return;
        }
        else if (StanceEvaluatorManager.Instance.IsPunchRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.punchHookRightState);
            //return;
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickLeft(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.highKickLeftState);
            //return;
        }
        else if (StanceEvaluatorManager.Instance.IsHighKickRight(poseLandmarkerResult.poseLandmarks[0].landmarks))
        {
            stateMachine.ChangeState(player.highKickRightState);
            //return;
        }

    }
}
