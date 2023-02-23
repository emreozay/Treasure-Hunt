using UnityEngine;

public class SaveLevelObject : MonoBehaviour
{
    public LevelObjectType type;
}

public enum LevelObjectType
{
    LogHorizontal,
    LogVertical,
    StoneSimple,
    StoneHorizontal,
    StoneVertical,
    TreeGreen,
    TreeOrange,
    TreeRed,
    TreeYellow,
}

//Bunu baþka yere koyman gerekebilir
[System.Serializable]
public class SaveLevelPrefab
{
    public LevelObjectType type;
    public GameObject prefab;
    public Texture2D texture;

    public SaveLevelPrefab(LevelObjectType _type)
    {
        this.type = _type;
    }
}