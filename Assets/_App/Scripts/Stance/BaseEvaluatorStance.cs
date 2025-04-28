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

        bool allMet = true;

        angleCriteriaStatus.Clear();
        if (stanceProfile.angleCriteriaList != null)
        {
            foreach (var criterion in stanceProfile.angleCriteriaList)
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

                float currentAngle = PoseUtilities.CalculateAngle(landmarksCheckNM, indexA, indexB_Vertex, indexC);

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
            foreach (var criterion in stanceProfile.distanceCriteriaList)
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
                foreach (var criterion in stanceProfile.yPositionStandardRelativeToInitStance)
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
            foreach (var criterion in stanceProfile.currentYPositionCriteriaList)
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

    protected void ResetBaseEvaluation()
    {
        angleCriteriaStatus.Clear();
        distanceCriteriaStatus.Clear();
        yPositionStandardRelativeToInitialStanceStatus.Clear();
        currentYPositionCriteriaStatus.Clear();
    }
}
