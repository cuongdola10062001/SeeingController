using UnityEngine;

[System.Serializable]
public class RelativeDistanceCriterion
{
    public string description = "right wrist near hip";
    public PoseLandmarkName landmark1 = PoseLandmarkName.RightWrist;
    public PoseLandmarkName landmark2 = PoseLandmarkName.RightHip;
    [Min(0f)] public float maxDistance = 0.1f;

    public int Index1 => (int)landmark1;
    public int Index2 => (int)landmark2;

    public bool IsDistanceValid(float measuredDistance)
    {
        if (measuredDistance < 0) return false;

        return measuredDistance <= maxDistance;
    }
}