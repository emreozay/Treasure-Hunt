using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameLevel gameLevel;
    [SerializeField]
    private GameObject mapPrefab;
    [SerializeField]
    private GameObject mapBoundaries;

    private GameObject environmentParent;
    private GameObject holeParent;
    private GameObject boostParent;

    [SerializeField]
    private List<SaveLevelPrefab> prefabList = new List<SaveLevelPrefab>();

    private SpriteRenderer mapSpriteRenderer;
    private GameObject boundaries;

    private Vector2 mapSizeMultiplier = Vector2.one;

    private bool newLevel = false;

    private void Start()
    {
        environmentParent = GameObject.Find("Environment");
        holeParent = GameObject.Find("Holes");
        boostParent = GameObject.Find("Boosts");

        LoadCurrentLevel();
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

        gameLevel.mapSizeMultiplier = mapSizeMultiplier;

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

        newLevel = true;
        LoadMap();

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
        newLevel = true;
        LoadMap();

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
            int levelObjectTypeIndex = (int)levelObject.type;
            SetParent(newInstance.transform, levelObjectTypeIndex);

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
        Vector3 newPosition = transform.position = new Vector3(Random.Range(-15f, 15f), Random.Range(-20f, 20f), 0);
        GameObject newObject = Instantiate(prefabList[objectIndex].prefab, newPosition, Quaternion.identity);

        SetParent(newObject.transform, objectIndex);
    }

    public Vector2 SetMapSize(Vector2 sizeMultiplier)
    {
        if (newLevel)
        {
            newLevel = false;
            return mapSizeMultiplier;
        }

        mapSizeMultiplier = sizeMultiplier;
        Vector2 mapDefaultSize = new Vector2(35f, 45f);

        if (mapSpriteRenderer != null)
            mapSpriteRenderer.size = mapDefaultSize * sizeMultiplier;

        if (boundaries != null)
            boundaries.transform.localScale = sizeMultiplier;

        return mapSizeMultiplier;
    }

    public void LoadMap()
    {
        mapSizeMultiplier = gameLevel.mapSizeMultiplier;
        mapSpriteRenderer = Instantiate(mapPrefab).GetComponent<SpriteRenderer>();
        mapSpriteRenderer.size *= mapSizeMultiplier;

        boundaries = Instantiate(mapBoundaries);
        boundaries.transform.localScale = mapSizeMultiplier;
    }

    public void ClearLevel()
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

        GameObject[] mapObjects = GameObject.FindGameObjectsWithTag("Map");
        GameObject[] boundaryObjects = GameObject.FindGameObjectsWithTag("Boundary");

        for (int i = 0; i < mapObjects.Length; i++)
        {
            if (Application.isEditor)
                DestroyImmediate(mapObjects[i]);
            else
                Destroy(mapObjects[i]);
        }

        for (int i = 0; i < boundaryObjects.Length; i++)
        {
            if (Application.isEditor)
                DestroyImmediate(boundaryObjects[i]);
            else
                Destroy(boundaryObjects[i]);
        }
    }

    public Vector2 GetMapSize()
    {
        return mapSizeMultiplier;
    }

    private void SetParent(Transform newObject, int index)
    {
        if (environmentParent == null)
            environmentParent = GameObject.Find("Environment");
        if (holeParent == null)
            holeParent = GameObject.Find("Holes");
        if (boostParent == null)
            boostParent = GameObject.Find("Boosts");

        if (index < 9)
            newObject.SetParent(environmentParent.transform);
        else if (index < 11)
            newObject.SetParent(boostParent.transform);
        else if (index == 11)
            newObject.SetParent(holeParent.transform);
    }
}