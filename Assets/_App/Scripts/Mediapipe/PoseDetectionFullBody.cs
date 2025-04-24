using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PoseDetectionFullBody : Singleton<PoseDetectionFullBody>
{
    private float landmarkVisibilityThreshold = 0.8f;
    private float frameMargin = 0.00f;
    private bool isFullBody = false;
    private bool wasFullBodyLast = false;
    public bool IsFullBody => isFullBody;

    public event Action<bool> OnFullBodyStatusUpdated;

    private void Update()
    {
        this.HandlePoseResult(InputManager.Instance.CurrentPoseTarget);
    }

    private void HandlePoseResult(PoseLandmarkerResult result)
    {

        bool currentFullBodyStatus = false;
        if (result.poseLandmarks != null && result.poseLandmarks.Count > 0 && result.poseLandmarks[0].landmarks != null)
        {
            currentFullBodyStatus = IsFullBodyVisible(result.poseLandmarks[0].landmarks);
        }

        wasFullBodyLast = isFullBody;
        isFullBody = currentFullBodyStatus;

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
        int indexLeftFoot = (int)PoseLandmarkName.LeftFootIndex;
        Debug.LogWarning("LeftFootIndex y: " + landmarks[indexLeftFoot].y);
        if (!IsLandmarkValid(landmarks, PoseLandmarkName.Nose)) return false;

        bool leftShoulderValid = IsLandmarkValid(landmarks, PoseLandmarkName.LeftShoulder);
        bool rightShoulderValid = IsLandmarkValid(landmarks, PoseLandmarkName.RightShoulder);
        if (!leftShoulderValid && !rightShoulderValid) return false;

        bool leftHipValid = IsLandmarkValid(landmarks, PoseLandmarkName.LeftHip);
        bool rightHipValid = IsLandmarkValid(landmarks, PoseLandmarkName.RightHip);
        if (!leftHipValid && !rightHipValid) return false;

        bool leftKneeValid = IsLandmarkValid(landmarks, PoseLandmarkName.LeftKnee);
        bool rightKneeValid = IsLandmarkValid(landmarks, PoseLandmarkName.RightKnee);
        if (!leftKneeValid && !rightKneeValid) return false;

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
