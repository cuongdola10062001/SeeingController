using Mediapipe.Tasks.Vision.PoseLandmarker;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    public GameObject model;
    public PoseDetectionFullBody poseDetectionFullBody;

    public float sideJumpForce =  4f;
    public float cooldownSelectDuration =  0.5f;

    [SerializeField] private List<RoomProfile> roomList = new List<RoomProfile>();
    public List<RoomProfile> RoomList => roomList;
    private int currentRoomIndex = 0;
    public int CurrentRoomIndex=> currentRoomIndex ;
    public void SetCurrentRoomIndex(int newIndex)
    {
        if(newIndex<0 || newIndex >= roomList.Count || newIndex ==currentRoomIndex) return;
        currentRoomIndex = newIndex;
        OnChangeRoomIndex?.Invoke(currentRoomIndex);
    }
    public event Action<int> OnChangeRoomIndex;


    #region State Machine
    public PlayerStateMachine stateMachine;
    public PlayerFullBodyState fullbodyState { get; private set; }
    public PlayerNotFullBodyState notFullbodyState { get; private set; }
    public PlayerSelectingState selectingState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerPunchHookLeftState punchHookLeftState { get; private set; }
    public PlayerPunchHookLeftState punchHookRightState { get; private set; }
    public PlayerHighKickLeftState highKickLeftState { get; private set; }
    public PlayerHighKickRightState highKickRightState { get; private set; }
    public PlayerDodgeLeftState dodgeLeftState { get; private set; }
    public PlayerDodgeRightState dodgeRightState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        fullbodyState = new PlayerFullBodyState(this, stateMachine, "None");
        selectingState = new PlayerSelectingState(this, stateMachine, "None");
        notFullbodyState = new PlayerNotFullBodyState(this, stateMachine, "None");
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        punchHookLeftState = new PlayerPunchHookLeftState(this, stateMachine, "PunchHook_L");
        punchHookRightState = new PlayerPunchHookLeftState(this, stateMachine, "PunchHook_R");
        highKickLeftState = new PlayerHighKickLeftState(this, stateMachine, "HighKickRound_L");
        highKickRightState = new PlayerHighKickRightState(this, stateMachine, "HighKickRound_R");
        dodgeLeftState = new PlayerDodgeLeftState(this, stateMachine, "Dodge_L");
        dodgeRightState = new PlayerDodgeRightState(this, stateMachine, "Dodge_R");
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
        UIManager.Instance.UISelection.Setup(roomList);
    }

    private void LateUpdate()
    {
        stateMachine.currentState.LateUpdate();
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

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();


}
