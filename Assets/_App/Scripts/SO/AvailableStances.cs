using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AvailableStances", menuName = "Pose Tracking/Available Stances", order = 2)]
public class AvailableStances : ScriptableObject
{
    public List<StanceProfile> availableWorkoutStances = new List<StanceProfile>();
}