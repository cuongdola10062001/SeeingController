using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stance Profile", menuName = "Pose Tracking/Stance Profile", order = 1)]
public class StanceProfile : ScriptableObject
{
    [Header("General information")]
    public string stanceName = "New Stance";

    [Header("Joint Angle Criteria")]
    public List<AngleCriterion> angleCriteriaList = new List<AngleCriterion>();

    [Header("Relative Distance Criteria")]
    public List<RelativeDistanceCriterion> distanceCriteriaList = new List<RelativeDistanceCriterion>();

    [Header("Standard Y Position Relative Vs Init Stance")]
    public List<RelativeYPositionCriterion> yPositionStandardRelativeToInitStance = new List<RelativeYPositionCriterion>();

    [Header("Relative Y Position Current Criteria")]
    public List<CurrentRelativeYCriterion> currentYPositionCriteriaList = new List<CurrentRelativeYCriterion>();
}
