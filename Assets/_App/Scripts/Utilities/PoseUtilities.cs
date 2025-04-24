using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using UnityEngine;

public static class PoseUtilities
{
    public static float CalculateAngle(IReadOnlyList<NormalizedLandmark> landmarks, int indexA, int indexB_Vertex, int indexC, bool use3D = false)
    {
        if (landmarks == null || indexA < 0 || indexA >= landmarks.Count ||
            indexB_Vertex < 0 || indexB_Vertex >= landmarks.Count ||
            indexC < 0 || indexC >= landmarks.Count)
        {
            return -1f;
        }

        NormalizedLandmark lmA = landmarks[indexA];
        NormalizedLandmark lmB = landmarks[indexB_Vertex];
        NormalizedLandmark lmC = landmarks[indexC];

        if (lmA == null || lmB == null || lmC == null)
        {
            return -1f;
        }

        float angle = 0f;

        if (use3D)
        {
            Vector3 pA = GetVector(lmA, use3D);
            Vector3 pB = GetVector(lmB, use3D);
            Vector3 pC = GetVector(lmC, use3D);

            Vector3 vecBA = pA - pB;
            Vector3 vecBC = pC - pB;

            if (vecBA.sqrMagnitude == 0 || vecBC.sqrMagnitude == 0)
            {
                return -1f;
            }

            angle = Vector3.Angle(vecBA, vecBC);
        }
        else
        {
            Vector2 pA = GetVector(lmA, use3D);
            Vector2 pB = GetVector(lmB, use3D);
            Vector2 pC = GetVector(lmC, use3D);

            Vector2 vecBA = pA - pB;
            Vector2 vecBC = pC - pB;

            if (vecBA.sqrMagnitude == 0 || vecBC.sqrMagnitude == 0)
            {

                return -1f;
            }
            vecBA.Normalize();
            vecBC.Normalize();

            float dot = Vector2.Dot(vecBA, vecBC);

            dot = Mathf.Clamp(dot, -1.0f, 1.0f);
            float angleRad = Mathf.Acos(dot);

            angle = angleRad * Mathf.Rad2Deg;
        }

        return angle;

    }

    public static float CalculateDistance(IReadOnlyList<NormalizedLandmark> normalizedLandmarks, int index1, int index2, bool use3D = false)
    {

        if (normalizedLandmarks == null ||
           index1 < 0 || index1 >= normalizedLandmarks.Count ||
           index2 < 0 || index2 >= normalizedLandmarks.Count ||
            normalizedLandmarks[index1] == null || normalizedLandmarks[index2] == null)
        {
            return -1f;
        }

        Vector2 p1, p2;

        p1 = GetVector(normalizedLandmarks[index1], use3D);
        p2 = GetVector(normalizedLandmarks[index2], use3D);

        Vector2 vecXY = new Vector2(p1.x - p2.x, p1.y - p2.y);
        return vecXY.magnitude;
    }


    private static Vector3 GetVector(NormalizedLandmark p, bool use3D)
    {
        if (p == null) return Vector3.zero;
        // Quan trọng: Sử dụng z nếu use3D là true
        return use3D ? new Vector3(p.x, p.y, p.z) : new Vector3(p.x, p.y, 0);
    }
}