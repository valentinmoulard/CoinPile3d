using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSet))]
public class LevelSet_Editor : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();
    //
    //     LevelSet levelSet = (LevelSet)target;
    //     
    //     if (GUILayout.Button("Setup Level Set"))
    //     {
    //         levelSet.SetupLevelSet();
    //     }
    // }
}
