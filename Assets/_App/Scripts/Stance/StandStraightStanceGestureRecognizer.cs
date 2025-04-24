using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;
using UnityEngine;

public class StandStraightStanceGestureRecognizer : BaseEvaluatorStance
{
    /*[SerializeField] private CustomPoseLandmarkerRunner poseRunner;
    [SerializeField] private StanceProfile stanceProfileStanceProfile;

    private bool isStanceComplete = false;
    private PoseLandmarkerResult landmarkerResult = default;

    public PoseLandmarkerResult LandmarkerResult => isStanceComplete ? landmarkerResult : default;

    public event Action<PoseLandmarkerResult> OnStanceSuccess;

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

      *//*  if (!isStanceComplete)
        {*//*
            bool allCriteriaMetStanceInit = false;
            allCriteriaMetStanceInit = EvaluateStance(normalizedLm.landmarks, stanceProfileStanceProfile);
            if (allCriteriaMetStanceInit)
            {
                result.CloneTo(ref landmarkerResult);
                OnStanceSuccess?.Invoke(result);
                isStanceComplete = true;
            }
       *//* }*//*
    }

    private void ResetInitialization()
    {
        isStanceComplete = false;
        landmarkerResult = default;
        this.ResetBaseEvaluation();
    }*/
}