using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    public PoseDetectionFullBody poseDetectionFullBody;
    public PlayerStateMachine stateMachine;

    public PlayerFullBodyState fullbodyState { get; private set; }
    public PlayerNotFullBodyState notFullbodyState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerPunchHookLeftState punchHookLeftState { get; private set; }
    public PlayerPunchHookLeftState punchHookRightState { get; private set; }
    public PlayerHighKickLeftState highKickLeftState { get; private set; }
    public PlayerHighKickRightState highKickRightState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        fullbodyState = new PlayerFullBodyState(this, stateMachine, "None");
        notFullbodyState = new PlayerNotFullBodyState(this, stateMachine, "None");
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        punchHookLeftState = new PlayerPunchHookLeftState(this, stateMachine, "PunchHook_L");
        punchHookRightState = new PlayerPunchHookLeftState(this, stateMachine, "PunchHook_R");
        highKickLeftState = new PlayerHighKickLeftState(this, stateMachine, "HighKickRound_L");
        highKickRightState = new PlayerHighKickRightState(this, stateMachine, "HighKickRound_R");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
    }

    private void OnEnable()
    {
        poseDetectionFullBody.OnFullBodyStatusUpdated += HandleFullBodyStatusUpdated;
    }
    private void OnDisable()
    {
        poseDetectionFullBody.OnFullBodyStatusUpdated -= HandleFullBodyStatusUpdated;
    }

    private void Start()
    {
        stateMachine.Initialize(notFullbodyState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    private void HandleFullBodyStatusUpdated(bool isFullBody)
    {
        /*if (isFullBody)
        {
            stateMachine.ChangeState(idleState);
        }
        else
        {
            stateMachine.ChangeState(notFullbodyState);
        }*/
    }

    public void AnimationFinishTrigger()=> stateMachine.currentState.AnimationFinishTrigger();


}
