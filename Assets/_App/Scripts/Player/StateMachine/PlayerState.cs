using Mediapipe.Tasks.Vision.PoseLandmarker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody rb;

    protected PoseLandmarkerResult poseLandmarkerResult;

    protected float xInput;
    protected float yInput;
    protected string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void LateUpdate()
    {
        stateTimer -= Time.deltaTime;
        poseLandmarkerResult = InputManager.Instance.CurrentPoseTarget;

        if (poseLandmarkerResult.poseLandmarks == null ||
            poseLandmarkerResult.poseLandmarks.Count <= 0 ||
            poseLandmarkerResult.poseLandmarks[0].landmarks == null) return;
     
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
