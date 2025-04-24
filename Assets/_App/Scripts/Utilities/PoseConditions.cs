using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;

public static class PoseConditions
{
    public static bool IsCrouching(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        if (landmarks == null)
            return false;

        int indexLeftHip = (int)PoseLandmarkName.LeftHip;
        int indexRightHip = (int)PoseLandmarkName.RightHip;
        int indexLeftKnee = (int)PoseLandmarkName.LeftKnee;
        int indexRightKnee = (int)PoseLandmarkName.RightKnee;

        float hipRightY = 10000, hipLeftY = 10000, kneeLeftY = 10000, kneeRightY = 10000;

        if (indexLeftHip > 0 && indexLeftHip < landmarks.Count && landmarks[indexLeftHip] != null)
            hipLeftY = landmarks[indexLeftHip].y;

        if (indexRightHip > 0 && indexRightHip < landmarks.Count && landmarks[indexRightHip] != null)
            hipRightY = landmarks[indexRightHip].y;

        if (indexLeftKnee > 0 && indexLeftKnee < landmarks.Count && landmarks[indexLeftKnee] != null)
            kneeLeftY = landmarks[indexLeftKnee].y;

        if (indexRightKnee > 0 && indexRightKnee < landmarks.Count && landmarks[indexRightKnee] != null)
            kneeRightY = landmarks[indexRightKnee].y;

        return hipLeftY > kneeLeftY || hipRightY > kneeRightY;
    }

    public static bool IsPunchingLeft(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        int indexLeftWrist = (int)PoseLandmarkName.LeftWrist;
        int indexLeftElbow = (int)PoseLandmarkName.LeftElbow;
        int indexLeftShoulder = (int)PoseLandmarkName.LeftShoulder;
        int indexNose = (int)PoseLandmarkName.Nose;
        int indexRightWrist = (int)PoseLandmarkName.RightWrist;
        int indexRightShoulder = (int)PoseLandmarkName.RightShoulder;

        float leftWristY = 10000, noseY = 10000, rightWristY = 1000, rightShoulderY = 1000;

        if (indexLeftWrist > 0 && indexLeftWrist < landmarks.Count && landmarks[indexLeftWrist] != null)
            leftWristY = landmarks[indexLeftWrist].y;

        if (indexNose > 0 && indexNose < landmarks.Count && landmarks[indexNose] != null)
            noseY = landmarks[indexNose].y;

        if (indexRightWrist > 0 && indexRightWrist < landmarks.Count && landmarks[indexRightWrist] != null)
            rightWristY = landmarks[indexRightWrist].y;

        if (indexRightShoulder > 0 && indexRightShoulder < landmarks.Count && landmarks[indexRightShoulder] != null)
            rightShoulderY = landmarks[indexRightShoulder].y;

        float angle = PoseUtilities.CalculateAngle(landmarks, indexLeftWrist, indexLeftElbow, indexLeftShoulder);

        return leftWristY <= noseY && rightWristY > rightShoulderY && angle >= 160f && angle <= 180f;
    }

    public static bool IsPunchingRight(IReadOnlyList<NormalizedLandmark> landmarks)
    {

        int indexRightWrist = (int)PoseLandmarkName.RightWrist;
        int indexNose = (int)PoseLandmarkName.Nose;

        float rightWristY = 10000, noseY = 10000;

        if (indexRightWrist > 0 && indexRightWrist < landmarks.Count && landmarks[indexRightWrist] != null)
            rightWristY = landmarks[indexRightWrist].y;

        if (indexNose > 0 && indexNose < landmarks.Count && landmarks[indexNose] != null)
            noseY = landmarks[indexNose].y;

        return rightWristY <= noseY;
    }
}