using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotFullBodyState : PlayerState
{
    public PlayerNotFullBodyState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.player.anim.gameObject.SetActive(false);

        UIManager.Instance.UISelection.gameObject.SetActive(false);

        UIManager.Instance.DesPoseFullbodyText.text = StringUtilities.MoveAway;
        UIManager.Instance.DesPoseFullbodyText.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (poseLandmarkerResult.poseLandmarks == null || poseLandmarkerResult.poseLandmarks[0].landmarks == null) return;


        if (InputManager.Instance.IsFullBody)
        {
            stateMachine.ChangeState(player.selectingState);
            return;
        }
    }
}
