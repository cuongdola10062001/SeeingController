using Mediapipe.Tasks.Vision.PoseLandmarker;

public class InputManager : Singleton<InputManager>
{
    public PoseLandmarkerResult CurrentPoseTarget => currentPoseTarget;
    private PoseLandmarkerResult currentPoseTarget;

    private bool isFullBody = false;
    public bool IsFullBody => isFullBody;

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

    public void UpdateIsFullbody(bool isFullbody)
    {
        lock (_currentTargetLock)
        {
            this.isFullBody = isFullbody;
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
