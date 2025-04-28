using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingState : PlayerState
{
    public PlayerWaitingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.stateTimer = this.player.waitingTime;

        UIManager.Instance.InstructText.text = StringUtilities.WaitingMove;
        UIManager.Instance.InstructText.gameObject.SetActive(true);
        UIManager.Instance.UISelection.gameObject.SetActive(false);
        this.player.anim.gameObject.SetActive(false);
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
            stateMachine.ChangeState(player.idleState);
            return;
        }

        this.stateTimer -= Time.deltaTime;
        if (this.stateTimer <= 0)
            stateMachine.ChangeState(player.notFullbodyState);
    }
}
