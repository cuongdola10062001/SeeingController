using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;
using UnityEngine;

public class LeftHandStanceGestureRecognizer : BaseEvaluatorStance
{
    /*[SerializeField] private CustomPoseLandmarkerRunner poseRunner;
    [SerializeField] private StanceProfile stanceProfileStanceProfile;

    private bool isStanceComplete = false;

    private float stanceCompleteTime = -1;
    private float resetDelay = 1.0f;

    public event Action OnStanceSuccess;

    private void OnEnable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated += HandlePoseResult;
        }
    }

    private void OnDisable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated -= HandlePoseResult;
        }
    }

    private void HandlePoseResult(PoseLandmarkerResult result, long time)
    {
        if (!(GameManager.Instance.CurrentState == AppState.SelectingStance || GameManager.Instance.CurrentState == AppState.Finish)) return;
        if (this.stanceProfileStanceProfile == null) return;
        if (result.poseLandmarks == null || result.poseLandmarks.Count == 0) return;

        NormalizedLandmarks normalizedLm = result.poseLandmarks[0];

        if (!isStanceComplete)
        {
            bool allCriteriaMetStanceInit = false;
            allCriteriaMetStanceInit = EvaluateStance(normalizedLm.landmarks, stanceProfileStanceProfile);
            if (allCriteriaMetStanceInit)
            {
                stanceCompleteTime = time;
                OnStanceSuccess?.Invoke();
                isStanceComplete = true;
            }
        }
        else
        {
            if (time >= stanceCompleteTime + resetDelay * 1000)
            {
                isStanceComplete = false;
                stanceCompleteTime = -1f;
            }
        }
    }

    private void ResetInitialization()
    {
        isStanceComplete = false;
        stanceCompleteTime = -1;
        this.ResetBaseEvaluation();
    }*/
}