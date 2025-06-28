using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingObstacle))]
public class MovingObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        MovingObstacle movingObstacle = (MovingObstacle)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Waypoint Management", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Add Waypoint"))
        {
            movingObstacle.AddWaypoint();
            EditorUtility.SetDirty(movingObstacle);
        }
        
        if (GUILayout.Button("Clear All Waypoints"))
        {
            if (EditorUtility.DisplayDialog("Clear Waypoints", 
                "Are you sure you want to clear all waypoints? This action cannot be undone.", 
                "Clear", "Cancel"))
            {
                movingObstacle.ClearWaypoints();
                EditorUtility.SetDirty(movingObstacle);
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Setup Guide:\n" +
            "1. Create an empty GameObject and add this MovingObstacle script\n" +
            "2. Add your platform/obstacle as a child of this GameObject\n" +
            "3. Use 'Add Waypoint' to create waypoints (they'll be children too)\n" +
            "4. Position the waypoints where you want the obstacle to move\n" +
            "5. The obstacle will move between waypoints while the container stays put", MessageType.Info);
    }
} 