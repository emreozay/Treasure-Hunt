#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private LevelManager levelManager;

    private Vector2 mapSizeMultiplier = Vector2.one;

    private void OnEnable()
    {
        //levelManager = FindObjectOfType<LevelManager>();
        levelManager = (LevelManager)target;
        mapSizeMultiplier = levelManager.GetMapSize();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        mapSizeMultiplier.x = EditorGUILayout.Slider("Map width size multiplier", mapSizeMultiplier.x, 1f, 2.5f);
        mapSizeMultiplier.y = EditorGUILayout.Slider("Map height size multiplier", mapSizeMultiplier.y, 1f, 2.5f);

        mapSizeMultiplier = levelManager.SetMapSize(mapSizeMultiplier);

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