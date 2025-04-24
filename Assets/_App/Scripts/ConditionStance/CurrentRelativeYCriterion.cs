using UnityEngine;

[System.Serializable]
public class CurrentRelativeYCriterion
{
    public string description = "New Current Relative Y Criterion";

    public PoseLandmarkName landmark1 = PoseLandmarkName.LeftIndex;

    public PoseLandmarkName landmark2 = PoseLandmarkName.Nose;

    public YComparisonType comparisonMethod = YComparisonType.ApproximateRatio;

    [Range(0f, 0.2f)]
    public float yTolerance = 0.05f;

    public bool isEnabled = true;

    [Tooltip("Is the criterion MANDATORY for posture recognition?")]
    public bool isCompletionCriterion = false;
    [Tooltip("Weighted score if this criterion is used for scoring (0-100).")]
    [Range(0, 100)] public float scoreWeight = 10;

    [Tooltip("An error message will be displayed if this criterion is not met.")]
    public string messageError = "The joint angle is not in the correct position.";

    public int Index1 => (int)landmark1;
    public int Index2 => (int)landmark2;

 
    public bool IsYPositionValid(float y1, float y2)
    {
        if (!isEnabled) return true;

        switch (comparisonMethod)
        {
            case YComparisonType.LowerThanOrEqualRatio:
                // Y1 <= Y2 + tolerance (Nhớ Y có thể hướng xuống, nên <= nghĩa là cao hơn hoặc bằng)
                return y1 <= y2 + yTolerance;

            case YComparisonType.HigherThanOrEqualRatio:
                // Y1 >= Y2 - tolerance (Y cao hơn là giá trị nhỏ hơn)
                return y1 >= y2 - yTolerance;

            case YComparisonType.ApproximateRatio:
                // |Y1 - Y2| <= tolerance
                return Mathf.Abs(y1 - y2) <= yTolerance;

            default:
                return false;
        }
    }
}