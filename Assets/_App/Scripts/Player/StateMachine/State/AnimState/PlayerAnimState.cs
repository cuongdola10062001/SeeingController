using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : PlayerState
{
    public PlayerAnimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.player.anim.gameObject.SetActive(true);
        UIManager.Instance.UISelection.gameObject.SetActive(false);
        UIManager.Instance.InstructText.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (!InputManager.Instance.IsFullBody)
        {
            stateMachine.ChangeState(player.waitingState);
            return;
        }
    }

    protected bool IsCurrentAnimationDone()
    {
        var animState = player.anim.GetCurrentAnimatorStateInfo(0);
        return animState.IsName(this.animBoolName) && animState.normalizedTime >= 1f;
    }
}
