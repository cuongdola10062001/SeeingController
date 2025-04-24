using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BaseEvaluatorStance : MonoBehaviour
{
    protected Dictionary<AngleCriterion, bool> angleCriteriaStatus = new Dictionary<AngleCriterion, bool>();
    protected Dictionary<RelativeDistanceCriterion, bool> distanceCriteriaStatus = new Dictionary<RelativeDistanceCriterion, bool>();
    protected Dictionary<RelativeYPositionCriterion, bool> yPositionStandardRelativeToInitialStanceStatus = new Dictionary<RelativeYPositionCriterion, bool>();
    protected Dictionary<CurrentRelativeYCriterion, bool> currentYPositionCriteriaStatus = new Dictionary<CurrentRelativeYCriterion, bool>();

    protected virtual bool EvaluateStance(IReadOnlyList<NormalizedLandmark> landmarksCheckNM, StanceProfile stanceProfile, IReadOnlyList<NormalizedLandmark> stanceInitLandmarksNM = null)
    {
        if (stanceProfile == null) return false;

        if (!stanceProfile.HasAnyEnabledCompletionCriteria())
        {
            return true;
        }

        /*if ((stanceProfile.angleCriteriaList?.Count ?? 0) == 0 && (stanceProfile.distanceCriteriaList?.Count ?? 0) == 0)
        {
            return true;
        }*/

        bool allMet = true;

        angleCriteriaStatus.Clear();
        if (stanceProfile.angleCriteriaList != null)
        {
            foreach (var criterion in stanceProfile.angleCriteriaList.Where(c => c.isEnabled && c.isCompletionCriterion))
            {
                var (indexA, indexB_Vertex, indexC) = criterion.GetLandmarkIndices();


                if (indexA < 0 || indexB_Vertex < 0 || indexC < 0 ||
                    indexA >= landmarksCheckNM.Count || indexB_Vertex >= landmarksCheckNM.Count || indexC >= landmarksCheckNM.Count ||
                    landmarksCheckNM[indexA] == null || landmarksCheckNM[indexB_Vertex] == null || landmarksCheckNM[indexC] == null)
                {
                    angleCriteriaStatus[criterion] = false;
                    allMet = false;
                    continue;
                }

                float currentAngle = PoseUtilities.CalculateAngle(landmarksCheckNM, indexA, indexB_Vertex, indexC, criterion.isUse3D);

                bool isMet = criterion.IsAngleValid(currentAngle);
                angleCriteriaStatus[criterion] = isMet;

                if (!isMet)
                {
                    allMet = false;
                }
            }
        }

        // Check distance
        distanceCriteriaStatus.Clear();
        if (stanceProfile.distanceCriteriaList != null)
        {
            foreach (var criterion in stanceProfile.distanceCriteriaList.Where(c => c.isEnabled && c.isCompletionCriterion))
            {
                int index1 = criterion.Index1;
                int index2 = criterion.Index2;

                if (index1 < 0 || index2 < 0 ||
                index1 >= landmarksCheckNM.Count || index2 >= landmarksCheckNM.Count ||
                    landmarksCheckNM[index1] == null || landmarksCheckNM[index2] == null)
                {
                    distanceCriteriaStatus[criterion] = false;
                    allMet = false;
                    continue;
                }

                float currentDistance = PoseUtilities.CalculateDistance(landmarksCheckNM, index1, index2);
                //Debug.LogWarning("currentDistance: "+ currentDistance);
                bool isMet = criterion.IsDistanceValid(currentDistance);
                distanceCriteriaStatus[criterion] = isMet;

                if (!isMet)
                {
                    allMet = false;
                }
            }
        }

        if (stanceInitLandmarksNM != null)
        {
            yPositionStandardRelativeToInitialStanceStatus.Clear();
            if (stanceProfile.yPositionStandardRelativeToInitStance != null)
            {
                foreach (var criterion in stanceProfile.yPositionStandardRelativeToInitStance.Where(c => c.isEnabled && c.isCompletionCriterion))
                {
                    int indexToCheck = criterion.IndexToCheck;
                    int indexReference = criterion.IndexReference;

                    if (indexToCheck < 0 || indexReference < 0 ||
                        indexToCheck >= landmarksCheckNM.Count || indexReference >= stanceInitLandmarksNM.Count ||
                        landmarksCheckNM[indexToCheck] == null || stanceInitLandmarksNM[indexReference] == null)
                    {
                        yPositionStandardRelativeToInitialStanceStatus[criterion] = false;
                        allMet = false;
                        continue;
                    }
                    float currentY = landmarksCheckNM[indexToCheck].y;
                    float referenceY = stanceInitLandmarksNM[indexReference].y;

                    bool isMet = criterion.IsYPositionValid(currentY, referenceY);
                    yPositionStandardRelativeToInitialStanceStatus[criterion] = isMet;

                    if (!isMet)
                    {
                        allMet = false;
                    }
                }
            }
        }

        currentYPositionCriteriaStatus.Clear();
        if (stanceProfile.currentYPositionCriteriaList != null)
        {
            foreach (var criterion in stanceProfile.currentYPositionCriteriaList.Where(c => c.isEnabled && c.isCompletionCriterion))
            {
                int index1 = criterion.Index1;
                int index2 = criterion.Index2;

                if (index1 < 0 || index2 < 0 ||
                    index1 >= landmarksCheckNM.Count || index2 >= landmarksCheckNM.Count ||
                    landmarksCheckNM[index1] == null || landmarksCheckNM[index2] == null)
                {
                    currentYPositionCriteriaStatus[criterion] = false; //
                    allMet = false;
                    continue;
                }

                float y1 = landmarksCheckNM[index1].y;
                float y2 = landmarksCheckNM[index2].y;

                bool isMet = criterion.IsYPositionValid(y1, y2);
                currentYPositionCriteriaStatus[criterion] = isMet;

                if (!isMet)
                {
                    allMet = false;
                }
            }
        }

        return allMet;
    }

    protected virtual (float score, string errorDetails) CalculateStanceScoreAndFeedback(IReadOnlyList<NormalizedLandmark> currentLandmarks, StanceProfile profile, IReadOnlyList<NormalizedLandmark> initialLandmarks = null)
    {
        if (profile == null)
        {
            Debug.LogError("CalculateStanceScoreInternal: StanceProfile is null!");
            return (0f,"");
        }
        if (currentLandmarks == null || currentLandmarks.Count == 0)
        {
            Debug.LogWarning($"CalculateStanceScoreInternal for '{profile.stanceName}': No current landmarks provided.");
            return (0f, "");
        }

        float totalWeightedScore = 0f;      // Tổng điểm đạt được (đã nhân trọng số)
        //float totalMaxScorePossible = 0f;   // Tổng trọng số của tất cả tiêu chí được bật
        StringBuilder errors = new StringBuilder();

        // --- 1. Chấm điểm Góc ---
        if (profile.angleCriteriaList != null)
        {
            foreach (var criterion in profile.angleCriteriaList.Where(c => c.isEnabled))
            {
                //totalMaxScorePossible += criterion.scoreWeight; // Cộng trọng số vào điểm tối đa
                var (indexA, indexB_Vertex, indexC) = criterion.GetLandmarkIndices();

                if (indexA >= 0 && indexB_Vertex >= 0 && indexC >= 0 &&
                    indexA < currentLandmarks.Count && indexB_Vertex < currentLandmarks.Count && indexC < currentLandmarks.Count &&
                    currentLandmarks[indexA] != null && currentLandmarks[indexB_Vertex] != null && currentLandmarks[indexC] != null)
                {
                    float currentAngle = PoseUtilities.CalculateAngle(currentLandmarks, indexA, indexB_Vertex, indexC, criterion.isUse3D);
                    if (criterion.IsAngleValid(currentAngle))
                    {
                        totalWeightedScore += criterion.scoreWeight;
                    }
                    else
                    {
                        errors.AppendLine($" - {criterion.messageError}");
                    }
                }
            }
        }

        // --- 2. Chấm điểm Khoảng cách ---
        if (profile.distanceCriteriaList != null)
        {
            foreach (var criterion in profile.distanceCriteriaList.Where(c => c.isEnabled))
            {
                //totalMaxScorePossible += criterion.scoreWeight;
                int index1 = criterion.Index1;
                int index2 = criterion.Index2;

                if (index1 >= 0 && index2 >= 0 &&
                    index1 < currentLandmarks.Count && index2 < currentLandmarks.Count &&
                    currentLandmarks[index1] != null && currentLandmarks[index2] != null)
                {
                    // Tính khoảng cách (truyền world landmarks nếu cần)
                    float currentDistance = PoseUtilities.CalculateDistance(
                        currentLandmarks, index1, index2
                    );

                    if (criterion.IsDistanceValid(currentDistance))
                    {
                        totalWeightedScore += criterion.scoreWeight;
                    }
                    else
                    {
                        errors.AppendLine($" - {criterion.messageError}");
                    }
                }
            }
        }

        // --- 3. Chấm điểm Y vs Init ---
        if (profile.yPositionStandardRelativeToInitStance != null)
        {
            foreach (var criterion in profile.yPositionStandardRelativeToInitStance.Where(c => c.isEnabled))
            {
                //totalMaxScorePossible += criterion.scoreWeight;
                // Chỉ chấm điểm nếu có dữ liệu initial landmarks
                if (initialLandmarks != null)
                {
                    int indexToCheck = criterion.IndexToCheck;
                    int indexReference = criterion.IndexReference;

                    if (indexToCheck >= 0 && indexReference >= 0 &&
                        indexToCheck < currentLandmarks.Count && indexReference < initialLandmarks.Count &&
                        currentLandmarks[indexToCheck] != null && initialLandmarks[indexReference] != null)
                    {
                        float currentY = currentLandmarks[indexToCheck].y;
                        float referenceY = initialLandmarks[indexReference].y;
                        if (criterion.IsYPositionValid(currentY, referenceY))
                        {
                            totalWeightedScore += criterion.scoreWeight;
                        }
                        else
                        {
                            errors.AppendLine($" - {criterion.messageError}");
                        }
                    }
                }
            }
        }

        // --- 4. Chấm điểm Current Y ---
        if (profile.currentYPositionCriteriaList != null)
        {
            foreach (var criterion in profile.currentYPositionCriteriaList.Where(c => c.isEnabled))
            {
                //totalMaxScorePossible += criterion.scoreWeight;
                int index1 = criterion.Index1;
                int index2 = criterion.Index2;

                if (index1 >= 0 && index2 >= 0 &&
                    index1 < currentLandmarks.Count && index2 < currentLandmarks.Count &&
                    currentLandmarks[index1] != null && currentLandmarks[index2] != null)
                {
                    float y1 = currentLandmarks[index1].y;
                    float y2 = currentLandmarks[index2].y;
                    if (criterion.IsYPositionValid(y1, y2))
                    {
                        totalWeightedScore += criterion.scoreWeight;
                    }
                    else
                    {
                        errors.AppendLine($" - {criterion.messageError}");
                    }
                }
            }
        }

        /*if (totalMaxScorePossible <= 0)
        {
            // Tránh chia cho 0 và xử lý trường hợp không có tiêu chí nào có trọng số
            // Debug.LogWarning($"ScoreCalculator: Profile '{profile.stanceName}' has no enabled criteria with score weight > 0. Returning 100%.");
            return 100f; // Hoặc 0f tùy theo logic bạn muốn
        }*/

        //float finalScore = (totalWeightedScore / totalMaxScorePossible) * 100f;

        return (totalWeightedScore, errors.ToString());
    }

    protected void ResetBaseEvaluation()
    {
        angleCriteriaStatus.Clear();
        distanceCriteriaStatus.Clear();
        yPositionStandardRelativeToInitialStanceStatus.Clear();
        currentYPositionCriteriaStatus.Clear();
    }
}
