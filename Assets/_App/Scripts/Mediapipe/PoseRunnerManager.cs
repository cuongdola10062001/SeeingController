using Mediapipe;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Experimental;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PoseRunnerManager : VisionTaskApiRunner<PoseLandmarker>
{
    [SerializeField] private PoseLandmarkerResultAnnotationController _customPoseLandmarkerResultAnnotationController;
    [SerializeField] private InputManager inputManager;

    private TextureFramePool _textureFramePool;

    public readonly PoseLandmarkDetectionConfig config = new PoseLandmarkDetectionConfig();
    public event Action<PoseLandmarkerResult> OnPoseResultUpdated;

    public override void Stop()
    {

        base.Stop();
        _textureFramePool?.Dispose();
        _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
        Debug.Log($"Delegate = {config.Delegate}");
        Debug.Log($"Image Read Mode = {config.ImageReadMode}");
        Debug.Log($"Model = {config.ModelName}");
        Debug.Log($"Running Mode = {config.RunningMode}");
        Debug.Log($"NumPoses = {config.NumPoses}");
        Debug.Log($"MinPoseDetectionConfidence = {config.MinPoseDetectionConfidence}");
        Debug.Log($"MinPosePresenceConfidence = {config.MinPosePresenceConfidence}");
        Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");
        Debug.Log($"OutputSegmentationMasks = {config.OutputSegmentationMasks}");

        yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

        var options = config.GetPoseLandmarkerOptions(config.RunningMode == Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnPoseLandmarkDetectionOutput : null);
        taskApi = PoseLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
        var imageSource = ImageSourceProvider.ImageSource;

        yield return imageSource.Play();

        if (!imageSource.isPrepared)
        {
            Debug.LogError("Failed to start ImageSource, exiting...");
            yield break;
        }

        // Use RGBA32 as the input format.
        // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
        _textureFramePool = new TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

        // NOTE: The screen will be resized later, keeping the aspect ratio.
        screen.Initialize(imageSource);

        SetupAnnotationController(_customPoseLandmarkerResultAnnotationController, imageSource);
        _customPoseLandmarkerResultAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);

        var transformationOptions = imageSource.GetTransformationOptions();
        var flipHorizontally = transformationOptions.flipHorizontally;
        var flipVertically = transformationOptions.flipVertically;

        // Always setting rotationDegrees to 0 to avoid the issue that the detection becomes unstable when the input image is rotated.
        // https://github.com/homuler/MediaPipeUnityPlugin/issues/1196
        var imageProcessingOptions = new Mediapipe.Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: 0);

        AsyncGPUReadbackRequest req = default;
        var waitUntilReqDone = new WaitUntil(() => req.done);
        var waitForEndOfFrame = new WaitForEndOfFrame();
        var result = PoseLandmarkerResult.Alloc(options.numPoses, options.outputSegmentationMasks);

        // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
        var canUseGpuImage = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && GpuManager.GpuResources != null;
        using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

        while (true)
        {
            if (isPaused)
            {
                yield return new WaitWhile(() => isPaused);
            }

            if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            // Build the input Image
            Image image;
            switch (config.ImageReadMode)
            {
                case ImageReadMode.GPU:
                    if (!canUseGpuImage)
                    {
                        throw new System.Exception("ImageReadMode.GPU is not supported");
                    }
                    textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    image = textureFrame.BuildGPUImage(glContext);
                    // TODO: Currently we wait here for one frame to make sure the texture is fully copied to the TextureFrame before sending it to MediaPipe.
                    // This usually works but is not guaranteed. Find a proper way to do this. See: https://github.com/homuler/MediaPipeUnityPlugin/pull/1311
                    yield return waitForEndOfFrame;
                    break;
                case ImageReadMode.CPU:
                    yield return waitForEndOfFrame;
                    textureFrame.ReadTextureOnCPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    image = textureFrame.BuildCPUImage();
                    textureFrame.Release();
                    break;
                case ImageReadMode.CPUAsync:
                default:
                    req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                    yield return waitUntilReqDone;

                    if (req.hasError)
                    {
                        Debug.LogWarning($"Failed to read texture from the image source");
                        continue;
                    }
                    image = textureFrame.BuildCPUImage();
                    textureFrame.Release();
                    break;
            }

            switch (taskApi.runningMode)
            {
                case Mediapipe.Tasks.Vision.Core.RunningMode.IMAGE:
                    if (taskApi.TryDetect(image, imageProcessingOptions, ref result))
                    {
                        _customPoseLandmarkerResultAnnotationController.DrawNow(result);
                    }
                    else
                    {
                        _customPoseLandmarkerResultAnnotationController.DrawNow(default);
                    }
                    DisposeAllMasks(result);
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.VIDEO:
                    if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
                    {
                        _customPoseLandmarkerResultAnnotationController.DrawNow(result);
                    }
                    else
                    {
                        _customPoseLandmarkerResultAnnotationController.DrawNow(default);
                    }
                    DisposeAllMasks(result);
                    break;
                case Mediapipe.Tasks.Vision.Core.RunningMode.LIVE_STREAM:
                    taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
                    break;
            }
        }
    }

    private void OnPoseLandmarkDetectionOutput(PoseLandmarkerResult result, Image image, long timestamp)
    {
        OnPoseResultUpdated?.Invoke(result);
        inputManager.UpdateCurrentPoseTarget(result);
        _customPoseLandmarkerResultAnnotationController.DrawLater(result);
        DisposeAllMasks(result);
    }

    private void DisposeAllMasks(PoseLandmarkerResult result)
    {
        if (result.segmentationMasks != null)
        {
            foreach (var mask in result.segmentationMasks)
            {
                mask.Dispose();
            }
        }
    }

}
