using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Components.Containers;

public enum SideJumpDirection
{
    None,
    Left,
    Right
}

public class SideJumpDetector
{
    private struct HipSnapshot
    {
        public float time;
        public float midX;
    }

    private Queue<HipSnapshot> history = new Queue<HipSnapshot>();

    private readonly float thresholdX = 0.06f;     // Độ lệch X tối thiểu (có thể ~20cm nếu quy đổi)
    private readonly float maxWindowTime = 0.2f;

    public SideJumpDirection DetectDirection(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        if (landmarks == null || landmarks.Count <= (int)PoseLandmarkName.RightHip)
            return SideJumpDirection.None;

        float leftX = landmarks[(int)PoseLandmarkName.LeftHip].x;
        float rightX = landmarks[(int)PoseLandmarkName.RightHip].x;
        float midX = (leftX + rightX) / 2f;

        float now = Time.time;

        history.Enqueue(new HipSnapshot { time = now, midX = midX });

        while (history.Count > 0 && now - history.Peek().time > maxWindowTime)
        {
            history.Dequeue();
        }

        foreach (var snapshot in history)
        {
            float deltaX = midX - snapshot.midX;
            float deltaTime = now - snapshot.time;

            if (deltaTime <= maxWindowTime && Mathf.Abs(deltaX) >= thresholdX)
            {
                history.Clear();
                return deltaX > 0 ? SideJumpDirection.Right : SideJumpDirection.Left;
            }
        }

        return SideJumpDirection.None;
    }
}
