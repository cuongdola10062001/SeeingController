using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using UnityEngine;

public class JumpDetector
{
    private struct FootSnapshot
    {
        public float time;
        public float leftY;
        public float rightY;
    }
    private readonly float minDeltaY = 0.06f;     // Tối thiểu giảm bao nhiêu thì tính là nhảy
    private readonly float maxWindowTime = 0.2f; // Tối đa thời gian để phát hiện giảm đột ngột

    private Queue<FootSnapshot> history = new Queue<FootSnapshot>();

    public bool IsJumping(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        if (landmarks == null || landmarks.Count <= (int)PoseLandmarkName.RightFootIndex)
            return false;

        float leftY = landmarks[(int)PoseLandmarkName.LeftFootIndex].y;
        float rightY = landmarks[(int)PoseLandmarkName.RightFootIndex].y;

        float now = Time.time;
        history.Enqueue(new FootSnapshot { time = now, leftY = leftY, rightY = rightY });

        // Loại bỏ snapshot quá cũ
        while (history.Count > 0 && now - history.Peek().time > maxWindowTime)
        {
            history.Dequeue();
        }

        // Duyệt snapshot cũ trong 0.1s gần đây
        foreach (var snapshot in history)
        {
            float deltaTime = now - snapshot.time;
            float leftDelta = snapshot.leftY - leftY;
            float rightDelta = snapshot.rightY - rightY;

            if (deltaTime <= maxWindowTime && leftDelta >= minDeltaY && rightDelta >= minDeltaY)
            {
                history.Clear(); // Reset sau khi nhận diện jump
                return true;
            }
        }

        return false;
    }
}
