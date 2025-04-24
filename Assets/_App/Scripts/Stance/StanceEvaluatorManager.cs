using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using UnityEngine;

public class StanceEvaluatorManager : BaseEvaluatorStance
{
    public static StanceEvaluatorManager Instance { get; private set; }

    [Header("Profiles")]
    public StanceProfile punchLeftProfile;
    public StanceProfile punchRightProfile;
    public StanceProfile crouchProfile;
    public StanceProfile highKickLeftProfile;
    public StanceProfile HighKickRightProfile;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsPunchLeft(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        return EvaluateStance(landmarks, punchLeftProfile);
    }

    public bool IsPunchRight(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        return EvaluateStance(landmarks, punchRightProfile);
    }

    public bool IsCrouching(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        return EvaluateStance(landmarks, crouchProfile);
    }

    public bool IsHighKickLeft(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        return EvaluateStance(landmarks, highKickLeftProfile);
    }

    public bool IsHighKickRight(IReadOnlyList<NormalizedLandmark> landmarks)
    {
        return EvaluateStance(landmarks, HighKickRightProfile);
    }
}
