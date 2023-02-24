using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameLevel gameLevel;

    private GameObject environmentParent;
    private GameObject holeParent;
    private GameObject boostParent;

    [SerializeField]
    private List<SaveLevelPrefab> prefabList = new List<SaveLevelPrefab>();

    private void Start()
    {
        environmentParent = GameObject.Find("Environment");
        holeParent = GameObject.Find("Holes");
        boostParent = GameObject.Find("Boosts");
    }

    private void UpdatePrefabList()
    {

    }

    public void SaveLevel()
    {
        if (gameLevel == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        gameLevel.ClearLevelObjectList();

        SaveLevelObject[] levelObjects = FindObjectsOfType<SaveLevelObject>();
        foreach (SaveLevelObject levelObject in levelObjects)
        {
            gameLevel.AddLevelObjectInfo(levelObject);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameLevel);
#endif
    }

    public void LoadCurrentLevel(GameLevel level, bool loadAsSandbox = false)
    {
        if (level == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        ClearLevel();

        foreach (LevelObjectInfo levelObject in level.levelObjectList)
        {
            GameObject prefab = null;
            foreach (SaveLevelPrefab levelPrefab in prefabList)
            {
                if (levelObject.type == levelPrefab.type)
                {
                    prefab = levelPrefab.prefab;
                    break;
                }
            }

            if (prefab == null)
            {
                Debug.LogWarning("Couldn't find prefab of type: " + levelObject.type);
                continue;
            }

            GameObject newInstance = Instantiate(prefab); //You can use object pooling for this

            newInstance.transform.position = levelObject.position;
        }
    }

    public void LoadCurrentLevel()
    {
        if (gameLevel == null)
        {
            Debug.LogError("No Game Level Object!");
            return;
        }

        ClearLevel();
        Debug.Log("Length: " + prefabList.Count);

        foreach (LevelObjectInfo levelObject in gameLevel.levelObjectList)
        {
            GameObject prefab = null;
            foreach (SaveLevelPrefab levelPrefab in prefabList)
            {
                //Debug.Log(levelObject.type + " - " + levelPrefab.type);
                if (levelObject.type == levelPrefab.type)
                {
                    prefab = levelPrefab.prefab;
                    break;
                }
            }

            if (prefab == null)
            {
                Debug.LogWarning("Couldn't find prefab of type: " + levelObject.type);
                continue;
            }

            GameObject newInstance = Instantiate(prefab); //You can use object pooling for this

            newInstance.transform.position = levelObject.position;
        }
    }

    public void LoadLevel(GameLevel level)
    {
        this.gameLevel = level;
        LoadCurrentLevel(); //bak buna
    }

    public Texture GetPrefabTexture(int textureIndex)
    {
        return prefabList[textureIndex].texture;
    }

    public int PrefabLength()
    {
        return prefabList.Count;
    }

    public void CreateObject(int objectIndex)
    {
        if (environmentParent == null)
            environmentParent = GameObject.Find("Environment");
        if (holeParent == null)
            holeParent = GameObject.Find("Holes");
        if (boostParent == null)
            boostParent = GameObject.Find("Boosts");

        Vector3 newPosition = transform.position = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        GameObject newObject = Instantiate(prefabList[objectIndex].prefab, newPosition, Quaternion.identity);

        if (objectIndex < 9)
            newObject.transform.SetParent(environmentParent.transform);
        else if (objectIndex < 11)
            newObject.transform.SetParent(boostParent.transform);
        else if (objectIndex == 11)
            newObject.transform.SetParent(holeParent.transform);
    }

    private void ClearLevel()
    {
        SaveLevelObject[] levelObjects = FindObjectsOfType<SaveLevelObject>();
        foreach (SaveLevelObject levelObject in levelObjects)
        {
            if (levelObject == null)
                continue;

            if (Application.isEditor)
                DestroyImmediate(levelObject.gameObject);
            else
                Destroy(levelObject.gameObject);
        }
    }
}
