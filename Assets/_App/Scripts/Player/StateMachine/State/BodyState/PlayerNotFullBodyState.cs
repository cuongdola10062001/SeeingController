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

        UIManager.Instance.InstructText.text = StringUtilities.MoveAway;
        UIManager.Instance.InstructText.gameObject.SetActive(true);
        this.player.anim.gameObject.SetActive(false);
        UIManager.Instance.UISelection.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (InputManager.Instance.IsFullBody)
        {
            stateMachine.ChangeState(player.selectingState);
            return;
        }
    }
}
