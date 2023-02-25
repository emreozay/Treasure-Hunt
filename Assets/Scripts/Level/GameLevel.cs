using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameLevel", menuName = "New Game Level")]
public class GameLevel : ScriptableObject
{
    public List<LevelObjectInfo> levelObjectList = new List<LevelObjectInfo>();
    public Vector2 mapSizeMultiplier = Vector2.one;
    public int levelIndex;

    public void ClearLevelObjectList()
    {
        levelObjectList.Clear();
    }

    public void AddLevelObjectInfo(SaveLevelObject levelObject)
    {
        LevelObjectInfo levelObjectInfo = new LevelObjectInfo(levelObject);
        levelObjectList.Add(levelObjectInfo);
    }

    public Vector3 GetPlayerStartPosition()
    {
        return Vector3.zero;
    }

    public Vector3 GetSandwichStartPosition()
    {
        return Vector3.zero;
    }
}
//Baþka yere taþýman gerekebilir!
[System.Serializable]
public class LevelObjectInfo
{
    public LevelObjectType type;
    public Vector3 position;

    public LevelObjectInfo()
    {

    }

    public LevelObjectInfo(SaveLevelObject levelObject)
    {
        this.type = levelObject.type;
        this.position = levelObject.transform.position;
    }
}