#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = FindObjectOfType<LevelManager>();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        for (int i = 0; i < levelManager.PrefabLength(); i++)
        {
            if (GUILayout.Button(levelManager.GetPrefabTexture(i), GUILayout.Width(40), GUILayout.Height(40)))
            {
                levelManager.CreateObject(i);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Level"))
        {
            Debug.Log("Saved!");
            levelManager.SaveLevel();

        }
        if (GUILayout.Button("Load Level"))
        {
            Debug.Log("Loaded!");
            levelManager.LoadCurrentLevel();
        }
        GUILayout.EndHorizontal();
    }
}
#endif