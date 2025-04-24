using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimState : PlayerFullBodyState
{
    public PlayerAnimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        this.player.anim.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

       
    }
}
