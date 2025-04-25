using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity.Sample;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetectionFullBody :MonoBehaviour
{
    [SerializeField] private PoseRunnerManager poseRunner;
    [SerializeField] private InputManager inputManager;

    private float landmarkVisibilityThreshold = 0.8f;
    private float frameMargin = 0.00f;
    private bool wasFullBodyLast = false;
    public bool IsFullBody => isFullBody;
    private bool isFullBody = false;

    public event Action<bool> OnFullBodyStatusUpdated;

    protected virtual void OnEnable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated += this.HandlePoseResult;
        }
    }

    protected virtual void OnDisable()
    {
        if (poseRunner != null)
        {
            poseRunner.OnPoseResultUpdated -= this.HandlePoseResult;
        }
    }

    private void HandlePoseResult(PoseLandmarkerResult result)
    {
        if (result.poseLandmarks == null || result.poseLandmarks[0].landmarks == null) return;

        bool currentFullBodyStatus = false;
        if (result.poseLandmarks != null && result.poseLandmarks.Count > 0 && result.poseLandmarks[0].landmarks != null)
        {
            currentFullBodyStatus = IsFullBodyVisible(result.poseLandmarks[0].landmarks);
        }

        wasFullBodyLast = isFullBody;
        isFullBody = currentFullBodyStatus;
        inputManager.UpdateIsFullbody(isFullBody);

        if (isFullBody && !wasFullBodyLast)
        {
            // isFullBody  False => True
            OnFullBodyStatusUpdated?.Invoke(isFullBody);
        }
        else if (!isFullBody && wasFullBodyLast)
        {
            // isFullBody  True => Flase
            OnFullBodyStatusUpdated?.Invoke(isFullBody);

        }
    }

    private bool IsFullBodyVisible(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        if (!IsLandmarkValid(landmarks, PoseLandmarkName.Nose)) return false;

        bool leftFootValid = IsLandmarkValid(landmarks, PoseLandmarkName.LeftFootIndex);
        bool rightFootValid = IsLandmarkValid(landmarks, PoseLandmarkName.RightFootIndex);
        if (!leftFootValid && !rightFootValid) return false;

        return true;
    }

    private bool IsLandmarkValid(IReadOnlyList<NormalizedLandmark> landmarks, PoseLandmarkName landmarkName)
    {
        if (landmarks == null) return false;
        int index = (int)landmarkName;
        if (index < 0 || landmarks.Count <= index) return false;

        NormalizedLandmark lm = landmarks[index];
        if (lm == null) return false;

        float visibility = (float)lm.visibility > 0 ? (float)lm.visibility : (float)lm.presence;
        if (visibility < landmarkVisibilityThreshold) return false;

        if (lm.x < frameMargin || lm.x > (1.0f - frameMargin) ||
            lm.y < frameMargin || lm.y > (1.0f - frameMargin)) return false;

        return true;
    }
}
