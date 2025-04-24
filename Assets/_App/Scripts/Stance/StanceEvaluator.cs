using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StanceEvaluator : BaseEvaluatorStance
{
    /*[SerializeField] private CustomPoseLandmarkerRunner poseRunner;

    private StanceProfile currentEvaluatorProfile;
    private IReadOnlyList<NormalizedLandmark> _initialLandmarks = null;
    private bool isStanceComplete = false;

    public event Action<float, string> OnStanceCompletedScoredAndFeedback;
    public event Action<string> OnStanceUnsuccess;

    public void SetProfile(StanceProfile profile)
    {
        if (currentEvaluatorProfile != profile)
        {
            currentEvaluatorProfile = profile;
        }
    }

    private void OnEnable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated += HandlePoseResult;
        }
    }

    protected virtual void OnDisable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated -= HandlePoseResult;
        }
    }

    protected virtual void HandlePoseResult(PoseLandmarkerResult result, long time)
    {
        if (!(GameManager.Instance.CurrentState == AppState.Evaluating)) return;
        if (currentEvaluatorProfile == null) return;
        if (result.poseLandmarks == null || result.poseLandmarks.Count == 0) return;

        if (_initialLandmarks == null && HasYPositionCriteria())
        {
            var initPoseResult = GameManager.Instance.LandmarkerResultStandTraight;
            if (initPoseResult.poseLandmarks != null && initPoseResult.poseLandmarks.Count > 0)
            {
                _initialLandmarks = initPoseResult.poseLandmarks[0].landmarks;
            }
        }

        *//*if (_initialLandmarks == null)
        {
            var initPoseResult = GameManager.Instance.LandmarkerResultStandTraight;
            _initialLandmarks = initPoseResult.poseLandmarks[0].landmarks;
        }*//*

        NormalizedLandmarks normalizedLm = result.poseLandmarks[0];

        *//*if (!isStanceComplete)
        {*//*
            bool allCriteriaMet = EvaluateStance(normalizedLm.landmarks, currentEvaluatorProfile, _initialLandmarks);

            if (allCriteriaMet)
            {
                (float score, string errorFeedback) scoreResult = CalculateStanceScoreAndFeedback(normalizedLm.landmarks, currentEvaluatorProfile , _initialLandmarks);
                OnStanceCompletedScoredAndFeedback?.Invoke(scoreResult.score, scoreResult.errorFeedback);
                isStanceComplete = true;
            }
        *//*}*//*
    }


    protected bool HasYPositionCriteria()
    {
        return currentEvaluatorProfile?.yPositionStandardRelativeToInitStance != null && currentEvaluatorProfile.yPositionStandardRelativeToInitStance.Count > 0 && currentEvaluatorProfile.yPositionStandardRelativeToInitStance.Any(c => c.isEnabled);
    }

    private void ResetStanceEvaluator()
    {
        currentEvaluatorProfile = null;
        isStanceComplete = false;

        this.ResetBaseEvaluation();
    }*/
}

