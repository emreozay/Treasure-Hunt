using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameLevel", menuName = "New Game Level")]
public class GameLevel : ScriptableObject
{
    public int levelIndex;

    public AILevel enemy1;
    public AILevel enemy2;
    public AILevel enemy3;

    public Vector2 mapSizeMultiplier = Vector2.one;

    public List<LevelObjectInfo> levelObjectList = new List<LevelObjectInfo>();

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