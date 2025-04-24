using UnityEngine;

[System.Serializable]
public class RelativeDistanceCriterion
{
    public string description = "right wrist near hip";
    public PoseLandmarkName landmark1 = PoseLandmarkName.RightWrist;
    public PoseLandmarkName landmark2 = PoseLandmarkName.RightHip;
    [Min(0f)] public float maxDistance = 0.1f;
    public bool isEnabled = true;

    [Tooltip("Is the criterion MANDATORY for posture recognition?")]
    public bool isCompletionCriterion = false;
    [Tooltip("Weighted score if this criterion is used for scoring (0-100).")]
    [Range(0, 100)] public float scoreWeight = 10;

    [Tooltip("An error message will be displayed if this criterion is not met.")]
    public string messageError = "The joint angle is not in the correct position.";

    public int Index1 => (int)landmark1;
    public int Index2 => (int)landmark2;

    public bool IsDistanceValid(float measuredDistance)
    {
        if (!isEnabled) return true;
        if (measuredDistance < 0) return false;

        return measuredDistance <= maxDistance;
    }
}