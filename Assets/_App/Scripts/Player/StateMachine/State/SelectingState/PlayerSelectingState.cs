using UnityEngine;

public class PlayerSelectingState : PlayerFullBodyState
{
    private float cooldownTimer = 0;
    private float cooldownDuration = 1f;

    private int currentRoomIndex = 0;
    private int maxRoomIndex => this.player.RoomList.Count - 1;

    public PlayerSelectingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        currentRoomIndex = this.player.CurrentRoomIndex;
        cooldownDuration = this.player.cooldownSelectDuration;
        UIManager.Instance.UISelection.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (poseLandmarkerResult.poseLandmarks == null || poseLandmarkerResult.poseLandmarks[0].landmarks == null) return;

        var landmarks = poseLandmarkerResult.poseLandmarks[0].landmarks;
        if (landmarks == null || landmarks.Count <= 0) return;

        if (StanceEvaluatorManager.Instance.IsStandStraight(landmarks))
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        if (StanceEvaluatorManager.Instance.IsLeftHand(landmarks))
        {
            if (currentRoomIndex > 0)
            {
                currentRoomIndex--;
                this.player.SetCurrentRoomIndex(currentRoomIndex);
            }

            cooldownTimer = cooldownDuration;
            return;
        }

        if (StanceEvaluatorManager.Instance.IsRightHand(landmarks))
        {
            if (currentRoomIndex < maxRoomIndex)
            {
                currentRoomIndex++;
                this.player.SetCurrentRoomIndex(currentRoomIndex);
            }

            cooldownTimer = cooldownDuration;
            return;
        }

    }
}
