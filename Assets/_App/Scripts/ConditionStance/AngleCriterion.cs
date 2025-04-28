using UnityEngine;

[System.Serializable]
public class AngleCriterion
{
    [Tooltip("Type of joint angle to be checked")]
    public JointAngleType angleType = JointAngleType.LeftKnee;
    [Range(0f, 180f)] public float targetAngle = 90f;
    [Range(0f, 45f)] public float tolerance = 15f;

    public (int IndexA, int IndexB_Vertex, int IndexC) GetLandmarkIndices()
    {
        switch (angleType)
        {
            // Right Side 
            case JointAngleType.RightShoulder:
                return ((int)PoseLandmarkName.RightElbow, (int)PoseLandmarkName.RightShoulder, (int)PoseLandmarkName.RightHip); // 14, 12, 24
            case JointAngleType.RightElbow:
                return ((int)PoseLandmarkName.RightWrist, (int)PoseLandmarkName.RightElbow, (int)PoseLandmarkName.RightShoulder); // 16, 14, 12
            case JointAngleType.RightWrist:
                return ((int)PoseLandmarkName.RightThumb, (int)PoseLandmarkName.RightWrist, (int)PoseLandmarkName.RightElbow); // 22, 16, 14
            case JointAngleType.RightHip:
                return ((int)PoseLandmarkName.LeftHip, (int)PoseLandmarkName.RightHip, (int)PoseLandmarkName.RightKnee); // 23, 24, 26
            case JointAngleType.RightKnee:
                return ((int)PoseLandmarkName.RightHip, (int)PoseLandmarkName.RightKnee, (int)PoseLandmarkName.RightAnkle); // 24, 26, 28
            case JointAngleType.RightAnkle:
                return ((int)PoseLandmarkName.RightKnee, (int)PoseLandmarkName.RightAnkle, (int)PoseLandmarkName.RightFootIndex); // 26, 28, 32
            case JointAngleType.RightSideBending:
                return ((int)PoseLandmarkName.RightShoulder, (int)PoseLandmarkName.RightHip, (int)PoseLandmarkName.RightKnee); // 12, 24, 26

            // Left Side 
            case JointAngleType.LeftShoulder:
                return ((int)PoseLandmarkName.LeftElbow, (int)PoseLandmarkName.LeftShoulder, (int)PoseLandmarkName.LeftHip); // 13, 11, 23
            case JointAngleType.LeftElbow:
                return ((int)PoseLandmarkName.LeftWrist, (int)PoseLandmarkName.LeftElbow, (int)PoseLandmarkName.LeftShoulder); // 15, 13, 11
            case JointAngleType.LeftWrist:
                return ((int)PoseLandmarkName.LeftThumb, (int)PoseLandmarkName.LeftWrist, (int)PoseLandmarkName.LeftElbow); // 21, 15, 13
            case JointAngleType.LeftHip:
                return ((int)PoseLandmarkName.RightHip, (int)PoseLandmarkName.LeftHip, (int)PoseLandmarkName.LeftKnee); // 24, 23, 25
            case JointAngleType.LeftKnee:
                return ((int)PoseLandmarkName.LeftHip, (int)PoseLandmarkName.LeftKnee, (int)PoseLandmarkName.LeftAnkle); // 23, 25, 27
            case JointAngleType.LeftAnkle:
                return ((int)PoseLandmarkName.LeftKnee, (int)PoseLandmarkName.LeftAnkle, (int)PoseLandmarkName.LeftFootIndex); // 25, 27, 31
            case JointAngleType.LeftSideBending:
                return ((int)PoseLandmarkName.LeftShoulder, (int)PoseLandmarkName.LeftHip, (int)PoseLandmarkName.LeftKnee); // 11, 23, 25

            default:
                Debug.LogError($"Landmark index not defined for corner type: {angleType}");
                return (-1, -1, -1);
        }
    }

    public bool IsAngleValid(float measuredAngle)
    {
        if (measuredAngle < 0) return false;

        return Mathf.Abs(measuredAngle - targetAngle) <= tolerance;
    }
}