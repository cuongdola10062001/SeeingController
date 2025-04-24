using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    public PlayerStateMachine stateMachine;

    public PlayerFullBodyState fullbodyState { get; private set; }
    public PlayerNotFullBodyState notFullbodyState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerPunchHookLeftState punchHookLeftState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        fullbodyState = new PlayerFullBodyState(this, stateMachine, "Idle");
        notFullbodyState = new PlayerNotFullBodyState(this, stateMachine, "Idle");
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        punchHookLeftState = new PlayerPunchHookLeftState(this, stateMachine, "PunchHook_L");
    }

    private void Start()
    {
        stateMachine.Initialize(notFullbodyState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void AnimationFinishTrigger()=> stateMachine.currentState.AnimationFinishTrigger();


}
