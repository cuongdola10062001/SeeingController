using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using UnityEngine;

public class JumpDetector
{
    private struct FootSnapshot
    {
        public float time;
        public float avgFootY;
    }

    private Queue<FootSnapshot> snapshotHistory = new Queue<FootSnapshot>();
    private const float windowTime = 0.15f;
    private const float jumpDeltaThreshold = 0.06f;

    public bool IsJumping(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        if (landmarks == null || landmarks.Count <= (int)PoseLandmarkName.RightFootIndex)
            return false;

        float leftY = landmarks[(int)PoseLandmarkName.LeftFootIndex].y;
        float rightY = landmarks[(int)PoseLandmarkName.RightFootIndex].y;
        float avgY = (leftY + rightY) / 2f;

        float now = Time.time;
        snapshotHistory.Enqueue(new FootSnapshot { time = now, avgFootY = avgY });

        // Loại bỏ snapshot cũ hơn 0.15s
        while (snapshotHistory.Count > 0 && now - snapshotHistory.Peek().time > windowTime)
        {
            snapshotHistory.Dequeue();
        }

        if (snapshotHistory.Count == 0) return false;

        float earliestY = snapshotHistory.Peek().avgFootY;

        float deltaY = avgY - earliestY;

        return deltaY < -jumpDeltaThreshold; 
    }
}
