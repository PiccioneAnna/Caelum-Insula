using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGeneration))]
public class DungeonEditor : Editor
{
    DungeonGeneration dungeonGeneration;

    public override void OnInspectorGUI()
    {
        using var check = new EditorGUI.ChangeCheckScope();
        base.OnInspectorGUI();
        if (check.changed && dungeonGeneration.autoGenerate)
        {
            dungeonGeneration.GenerateDungeon();
        }

        if (GUILayout.Button("Generate Dungeon"))
        {
            dungeonGeneration.GenerateDungeon();
        }
    }

    private void OnEnable()
    {
        dungeonGeneration = (DungeonGeneration)target;
    }
}
