using Mediapipe.Tasks.Vision.PoseLandmarker;

public class InputManager : Singleton<InputManager>
{
    public PoseLandmarkerResult CurrentPoseTarget => currentPoseTarget;
    private PoseLandmarkerResult currentPoseTarget;

    private readonly object _currentTargetLock = new object();
    private bool isStale = false;

    private void Update()
    {
        if (isStale)
        {
            SyncNow();
        }
    }

    public void UpdateCurrentPoseTarget(PoseLandmarkerResult newTarget)
    {
        lock (_currentTargetLock)
        {
            newTarget.CloneTo(ref currentPoseTarget);
            isStale = true;
        }
    }

    protected virtual void SyncNow()
    {
        lock (_currentTargetLock)
        {
            isStale = false;
        }
    }

}
