// RelativeYPositionCriterion.cs
using UnityEngine;

public enum YComparisonType
{
    LowerThanOrEqualRatio,
    HigherThanOrEqualRatio,
    ApproximateRatio
}

[System.Serializable]
public class RelativeYPositionCriterion
{
    [Tooltip("Criteria description")]
    public string description = "New Relative Y Criterion";

    [Tooltip("Landmark in CURRENT position to be checked.")]
    public PoseLandmarkName landmarkToCheck = PoseLandmarkName.LeftShoulder;

    [Tooltip("Corresponding Landmark in INITIAL POSE")]
    public PoseLandmarkName referenceLandmark = PoseLandmarkName.LeftShoulder;

    [Tooltip("How to compare Y positions.")]
    public YComparisonType comparisonType = YComparisonType.LowerThanOrEqualRatio;

    [Tooltip("The target ratio between current Y and reference Y (e.g. 0.9 for 10% lower). The meaning of this value depends on the Comparison Type.")]
    [Range(0f, 2.0f)]
    public float targetRatio = 0.9f;

    [Tooltip("Tolerance allowed when comparing proportions (e.g. 0.05 allows 5% deviation).")]
    [Range(0f, 0.1f)]
    public float ratioTolerance = 0.02f;

    public int IndexToCheck => (int)landmarkToCheck;
    public int IndexReference => (int)referenceLandmark;

    public bool IsYPositionValid(float currentY, float referenceY)
    {
        if (Mathf.Abs(referenceY) < 0.01f)
        {
            return false;
        }

        //float actualRatio = currentY / referenceY;
        float actualRatio = referenceY / currentY;

        //Debug.LogWarning("currentY: " + currentY + "     referenceY: " + referenceY + "     actualRatio:" + actualRatio);

        switch (comparisonType)
        {
            case YComparisonType.LowerThanOrEqualRatio:
                return actualRatio <= targetRatio + ratioTolerance;

            case YComparisonType.HigherThanOrEqualRatio:
                return actualRatio >= targetRatio - ratioTolerance;

            case YComparisonType.ApproximateRatio:
                return Mathf.Abs(actualRatio - targetRatio) <= ratioTolerance;

            default:
                return false;
        }
    }
}